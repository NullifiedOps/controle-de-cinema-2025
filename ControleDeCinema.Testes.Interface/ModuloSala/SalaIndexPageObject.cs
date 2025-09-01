using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloSala;

public class SalaIndexPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public SalaIndexPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(40));
    }

    public SalaIndexPageObject IrPara(string enderecoBase)
    {
        driver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "salas"));

        return this;
    }

    public SalaFormPageObject ClickCadastrar()
    {
        wait.Until(d => d?.FindElement(By.CssSelector("[data-se='btnCadastrar']"))).Click();

        wait.Until(d => d.Url.Contains("/cadastrar"));

        return new SalaFormPageObject(driver!);
    }

    public SalaFormPageObject ClickEditar()
    {
        wait.Until(d => d?.FindElement(By.CssSelector(".card a[title='Edição']"))).Click();

        return new SalaFormPageObject(driver!);
    }

    public SalaFormPageObject ClickExcluir()
    {
        wait.Until(d => d?.FindElement(By.CssSelector(".card a[title='Exclusão']"))).Click();

        return new SalaFormPageObject(driver!);
    }

    public bool ContemSala(int numeroSala)
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        var verificador = $"# {numeroSala}";

        return driver.PageSource.Contains(verificador);
    }
}
