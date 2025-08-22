using OpenQA.Selenium;
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

    public AutentificacaoFormPageObject ClickCriarConta()
    {
        wait.Until(d => d?.FindElement(By.CssSelector("a[data-se='btnCadastrar']"))).Click();

        return new AutentificacaoFormPageObject(driver!);
    }

    public bool ComtemLogin(string email)
    {
        return wait.Until(d => d.PageSource.Contains(email));
    }
}
