using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloFilme;

public class FilmeFormPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public FilmeFormPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        wait.Until(d => d.FindElement(By.CssSelector("[data-se='form']")).Displayed);
    }

    public FilmeFormPageObject PreencherTitulo(string titulo)
    {
        wait.Until(d =>
            d.FindElement(By.Id("Titulo")).Displayed &&
            d.FindElement(By.Id("Titulo")).Enabled
        );

        var inputNome = driver?.FindElement(By.Id("Titulo"));
        inputNome?.Clear();
        inputNome?.SendKeys(titulo);

        return this;
    }

    public FilmeFormPageObject PreencherDuracao(int duracao)
    {
        wait.Until(d =>
            d.FindElement(By.Id("Duracao")).Displayed &&
            d.FindElement(By.Id("Duracao")).Enabled
        );

        var inputNome = driver?.FindElement(By.Id("Duracao"));
        inputNome?.Clear();
        inputNome?.SendKeys(duracao.ToString());

        return this;
    }

    public FilmeFormPageObject SelecionarGenero(string genero)
    {
        wait.Until(d =>
            d.FindElement(By.Id("GeneroId")).Displayed &&
            d.FindElement(By.Id("GeneroId")).Enabled
        );

        var select = new SelectElement(driver.FindElement(By.Id("GeneroId")));
        select.SelectByText(genero);

        return this;
    }

    public FilmeIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();

        return new FilmeIndexPageObject(driver!);
    }
}
