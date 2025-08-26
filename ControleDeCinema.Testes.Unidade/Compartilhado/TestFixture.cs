using ControledeCinema.Dominio.Compartilhado;
using ControleDeCinema.Aplicacao.ModuloAutenticacao;
using ControleDeCinema.Aplicacao.ModuloFilme;
using ControleDeCinema.Aplicacao.ModuloGeneroFilme;
using ControleDeCinema.Aplicacao.ModuloSala;
using ControleDeCinema.Aplicacao.ModuloSessao;
using ControleDeCinema.Dominio.ModuloAutenticacao;
using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloSessao;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace ControleDeCinema.Testes.Unidade.Compartilhado;

public abstract class TestFixture
{
    protected Mock<IRepositorioGeneroFilme>? repositorioGeneroFilmeMock;
    protected Mock<IRepositorioFilme>? repositorioFilmeMock;
    protected Mock<IRepositorioIngresso>? repositorioIngressoMock;
    protected Mock<IRepositorioSessao>? repositorioSessaoMock;
    protected Mock<IRepositorioSala>? repositorioSalaMock;

    protected Mock<ILogger<GeneroFilmeAppService>>? loggerGeneroFilmeMock;
    protected Mock<ILogger<FilmeAppService>>? loggerFilmeMock;
    protected Mock<ILogger<IngressoAppService>>? loggerIngressoMock;
    protected Mock<ILogger<SessaoAppService>>? loggerSessaoMock;
    protected Mock<ILogger<SalaAppService>>? loggerSalaMock;

    protected GeneroFilmeAppService? generoFilmeAppService;
    protected FilmeAppService? filmeAppService;
    protected IngressoAppService? ingressoAppService;
    protected SessaoAppService? sessaoAppService;
    protected SalaAppService? salaAppService;

    protected AutenticacaoAppService? autenticacaoAppService;

    protected Mock<UserManager<Usuario>>? userManager;
    protected Mock<SignInManager<Usuario>>? signInManager;
    protected Mock<RoleManager<Cargo>>? roleManager;

    protected Mock<ITenantProvider>? tenantProviderMock;
    protected Mock<IUnitOfWork>? unitOfWorkMock;

    [TestInitialize]
    public void Setup()
    {
        repositorioGeneroFilmeMock = new Mock<IRepositorioGeneroFilme>();
        repositorioFilmeMock = new Mock<IRepositorioFilme>();
        repositorioIngressoMock = new Mock<IRepositorioIngresso>();
        repositorioSessaoMock = new Mock<IRepositorioSessao>();
        repositorioSalaMock = new Mock<IRepositorioSala>();

        loggerGeneroFilmeMock = new Mock<ILogger<GeneroFilmeAppService>>();
        loggerFilmeMock = new Mock<ILogger<FilmeAppService>>();
        loggerIngressoMock = new Mock<ILogger<IngressoAppService>>();
        loggerSessaoMock = new Mock<ILogger<SessaoAppService>>();
        loggerSalaMock = new Mock<ILogger<SalaAppService>>();

        userManager = new Mock<UserManager<Usuario>>();
        signInManager = new Mock<SignInManager<Usuario>>();
        roleManager = new Mock<RoleManager<Cargo>>();

        tenantProviderMock = new Mock<ITenantProvider>();
        unitOfWorkMock = new Mock<IUnitOfWork>();

        generoFilmeAppService = new GeneroFilmeAppService(
            tenantProviderMock.Object,
            repositorioGeneroFilmeMock.Object, 
            unitOfWorkMock.Object, 
            loggerGeneroFilmeMock.Object
        );

        filmeAppService = new FilmeAppService(
            tenantProviderMock.Object,
            repositorioFilmeMock.Object, 
            unitOfWorkMock.Object, 
            loggerFilmeMock.Object
        );

        ingressoAppService = new IngressoAppService(
            tenantProviderMock.Object,
            repositorioIngressoMock.Object, 
            loggerIngressoMock.Object
        );

        sessaoAppService = new SessaoAppService(
            tenantProviderMock.Object,
            repositorioSessaoMock.Object, 
            unitOfWorkMock.Object, 
            loggerSessaoMock.Object
        );

        salaAppService = new SalaAppService(
            tenantProviderMock.Object,
            repositorioSalaMock.Object, 
            unitOfWorkMock.Object, 
            loggerSalaMock.Object
        );

        autenticacaoAppService = new AutenticacaoAppService(
            userManager.Object,
            signInManager.Object,
            roleManager.Object
        );
    }
}
