using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloAutentificacao;

public class AutentificacaoFormPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public AutentificacaoFormPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        wait.Until(d => d.FindElement(By.CssSelector("form")).Displayed);
    }

    public AutentificacaoFormPageObject PreencherEmail(string email)
    {
        wait.Until(d =>
            d.FindElement(By.Id("Email")).Displayed &&
            d.FindElement(By.Id("Email")).Enabled
        );

        var inputNome = driver?.FindElement(By.Id("Email"));
        inputNome?.Clear();
        inputNome?.SendKeys(email);

        return this;
    }

    public AutentificacaoFormPageObject PreencherSenha(string senha)
    {
        wait.Until(d =>
            d.FindElement(By.Id("Senha")).Displayed &&
            d.FindElement(By.Id("Senha")).Enabled
        );

        var inputNome = driver?.FindElement(By.Id("Senha"));
        inputNome?.Clear();
        inputNome?.SendKeys(senha);

        return this;
    }

    public AutentificacaoFormPageObject PreencherConfirmarSenha(string senha)
    {
        wait.Until(d =>
            d.FindElement(By.Id("ConfirmarSenha")).Displayed &&
            d.FindElement(By.Id("ConfirmarSenha")).Enabled
        );

        var inputNome = driver?.FindElement(By.Id("ConfirmarSenha"));
        inputNome?.Clear();
        inputNome?.SendKeys(senha);

        return this;
    }

    public AutentificacaoFormPageObject SelecionarTipoUsuario(string tipo)
    {
        wait.Until(d =>
            d.FindElement(By.Id("Tipo")).Displayed &&
            d.FindElement(By.Id("Tipo")).Enabled
        );

        var select = new SelectElement(driver.FindElement(By.Id("Tipo")));
        select.SelectByText(tipo);

        return this;
    }

    public AutentificacaoIndexPageObject Confirmar()
    {
        wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']"))).Click();

        return new AutentificacaoIndexPageObject(driver!);
    }
}
