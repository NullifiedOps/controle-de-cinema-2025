using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloAutentificacao;

public class AutentificacaoIndexPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public AutentificacaoIndexPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    public AutentificacaoIndexPageObject IrParaLogin(string enderecoBase)
    {
        driver?.Navigate().GoToUrl(enderecoBase);

        return this;
    }

    public AutentificacaoIndexPageObject ClickUsuario()
    {
        wait.Until(d => d?.FindElement(By.CssSelector("button[data-se='btnUsuario']"))).Click();

        return this;
    }

    public AutentificacaoFormPageObject ClickLoggout()
    {
        wait.Until(d => d?.FindElement(By.CssSelector("button[data-se='btnLogout']"))).Click();

        return new AutentificacaoFormPageObject(driver!);
    }

    public AutentificacaoFormPageObject ClickCriarConta()
    {
        wait.Until(d => d?.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();

        return new AutentificacaoFormPageObject(driver!);
    }

    public bool ContemLogin(string email)
    {
        return wait.Until(d => d.PageSource.Contains(email));
    }

    public bool ContemErroEscrita(string erro)
    {
        return wait.Until(d => d.FindElement(By.CssSelector($"span[id='{erro}-error']")).Displayed);
    }

    public bool ContemErroAlert(string erroMsg)
    {
        wait.Until(d => d.FindElement(By.CssSelector($"div[data-se='alertCard']")).Displayed);

        var alert = driver.FindElement(By.CssSelector($"div[data-se='alertCard']")).Text;

        return alert.Contains(erroMsg);
    }
}
