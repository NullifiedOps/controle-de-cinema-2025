using ControleDeCinema.Infraestrutura.Orm.Compartilhado;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System.Data.Common;
using Testcontainers.PostgreSql;

namespace ControleDeCinema.Testes.Interface.Compartilhado;

[TestClass]
public abstract class TestFixture
{
    protected static IWebDriver? driver;
    protected static ControleDeCinemaDbContext? dbContext;
    protected static string? enderecoBase;
    //protected static string? enderecoBase = "https://localhost:7131";

    private static IDatabaseContainer? dbContainer;
    private static readonly int dbPort = 5432;

    private static IContainer? appContainer;
    private static readonly int appPort = 8080;

    private static IContainer? seleniumContainer;
    private static readonly int seleniumPort = 4444;

    private static IConfiguration? configuracao;

    //private readonly static string connectionString =  "Host=localhost;Port=5432;Database=ControleDeCinemaDb;Username=postgres;Password=YourStrongPassword";

    [AssemblyInitialize]
    public static async Task ConfigurarTestes(TestContext _)
    {
        configuracao = new ConfigurationBuilder()
            .AddUserSecrets<TestFixture>()
            .AddEnvironmentVariables()
            .Build();

        var rede = new NetworkBuilder()
            .WithName(Guid.NewGuid().ToString("D"))
            .WithCleanUp(true)
            .Build();

        await InicializarBancoDadosAsync(rede);

        await InicializarAplicacaoAsync(rede);

        await InicializarWebDriverAsync(rede);
        //InicializarDriver();
    }

    [AssemblyCleanup]
    public static async Task EncerrarTestes()
    {
        //FinalizarDriver();
        await FinalizarWebDriverAsync();

        await EncerrarAplicacaoAsync();

        await EncerrarBancoDadosAsync();
    }

    [TestInitialize]
    public void InicializarTeste()
    {
        if (dbContainer is null)
            throw new ArgumentNullException("O banco de dados não foi inicializado.");

        dbContext = ControleDeCinemaDbContextFactory.CriarDbContext(dbContainer.GetConnectionString());
        //dbContext = ControleDeCinemaDbContextFactory.CriarDbContext(connectionString);

        ConfigurarTabelas(dbContext);

        driver!.Manage()
            .Cookies.DeleteAllCookies();
    }

    private static void InicializarDriver()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");

        driver = new ChromeDriver();
        driver.Manage().Window.FullScreen();
    }

    private static void FinalizarDriver()
    {
        driver?.Quit();
        driver?.Dispose();
    }

    private static async Task InicializarBancoDadosAsync(DotNet.Testcontainers.Networks.INetwork rede)
    {
        dbContainer = new PostgreSqlBuilder()
              .WithImage("postgres:16")
              .WithPortBinding(dbPort, true)
              .WithNetwork(rede)
              .WithNetworkAliases("controle-de-cinema-e2e-testdb")
              .WithName("controle-de-cinema-e2e-testdb")
              .WithDatabase("ControleDeCinemaDb")
              .WithUsername("postgres")
              .WithPassword("YourStrongPassword")
              .WithCleanUp(true)
              .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(dbPort))
              .Build();

        await dbContainer.StartAsync();
    }

    private static async Task InicializarAplicacaoAsync(DotNet.Testcontainers.Networks.INetwork rede)
    {
        var imagem = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithDockerfile("Dockerfile")
            .WithBuildArgument("RESOURCE_REAPER_SESSION_ID", ResourceReaper.DefaultSessionId.ToString("D"))
            .WithName("controle-de-cinema-app-e2e:latest")
            .Build();

        await imagem.CreateAsync().ConfigureAwait(false);

        var connectionStringRede = dbContainer?.GetConnectionString()
            .Replace(dbContainer.Hostname, "controle-de-cinema-e2e-testdb")
            .Replace(dbContainer.GetMappedPublicPort(dbPort).ToString(), "5432");

        appContainer = new ContainerBuilder()
            .WithImage(imagem)
            .WithPortBinding(appPort, true)
            .WithNetwork(rede)
            .WithNetworkAliases("controle-de-cinema-webapp")
            .WithName("controle-de-cinema-webapp")
            .WithEnvironment("SQL_CONNECTION_STRING", connectionStringRede)
            .WithEnvironment("NEWRELIC_LICENSE_KEY", configuracao?["NEWRELIC_LICENSE_KEY"])
            .WithWaitStrategy(Wait.ForUnixContainer()
                .UntilPortIsAvailable(appPort)
                .UntilHttpRequestIsSucceeded(r => r.ForPort((ushort)appPort).ForPath("/health"))
            )
            .WithCleanUp(true)
            .Build();

        await appContainer.StartAsync();

        enderecoBase = $"http://{appContainer.Name}:{appPort}";
        //enderecoBase = $"http://localhost:{appPort}";
    }

    private static async Task InicializarWebDriverAsync(DotNet.Testcontainers.Networks.INetwork rede)
    {
        seleniumContainer = new ContainerBuilder()
            .WithImage("selenium/standalone-chrome:nightly")
            .WithPortBinding(seleniumPort, true)
            .WithNetwork(rede)
            .WithNetworkAliases("controle-de-cinema-selenium-e2e")
            .WithExtraHost("host.docker.internal", "host-gateway")
            .WithName("gcontrole-de-cinema-selenium-e2e")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(seleniumPort))
            .Build();

        await seleniumContainer.StartAsync();

        var enderecoSelenium = new Uri($"http://{seleniumContainer.Hostname}:{seleniumContainer.GetMappedPublicPort(seleniumPort)}/wd/hub");

        var options = new ChromeOptions();
        options.AddArgument("--window-size=1920,1080");
        options.AddArgument("--start-maximized");

        driver = new RemoteWebDriver(enderecoSelenium, options);
        driver.Manage().Window.FullScreen();
    }

    private static async Task EncerrarBancoDadosAsync()
    {
        if (dbContainer is not null)
            await dbContainer.DisposeAsync();
    }

    private static async Task EncerrarAplicacaoAsync()
    {
        if (appContainer is not null)
            await appContainer.DisposeAsync();
    }

    private static async Task FinalizarWebDriverAsync()
    {
        driver?.Quit();
        driver?.Dispose();

        if (seleniumContainer is not null)
            await seleniumContainer.DisposeAsync();
    }

    private static void ConfigurarTabelas(ControleDeCinemaDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();

        dbContext.Filmes.RemoveRange(dbContext.Filmes);
        dbContext.GenerosFilme.RemoveRange(dbContext.GenerosFilme);
        dbContext.Salas.RemoveRange(dbContext.Salas);
        dbContext.Sessoes.RemoveRange(dbContext.Sessoes);
        
        dbContext.RoleClaims.RemoveRange(dbContext.RoleClaims);
        dbContext.Roles.RemoveRange(dbContext.Roles);
        dbContext.UserClaims.RemoveRange(dbContext.UserClaims);
        dbContext.Users.RemoveRange(dbContext.Users);
        dbContext.UserTokens.RemoveRange(dbContext.UserTokens);

        dbContext.SaveChanges();
    }
}
