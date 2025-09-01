using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloFilme;

public class FilmeIndexPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public FilmeIndexPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(40));
    }

    public FilmeIndexPageObject IrPara(string enderecoBase)
    {
        driver?.Navigate().GoToUrl(Path.Combine(enderecoBase, "filmes"));

        return this;
    }

    public FilmeFormPageObject ClickCadastrar()
    {
        wait.Until(d => d?.FindElement(By.CssSelector("[data-se='btnCadastrar']"))).Click();

        wait.Until(d => d.Url.Contains("/cadastrar"));

        return new FilmeFormPageObject(driver!);
    }

    public FilmeFormPageObject ClickEditar()
    {
        wait.Until(d => d?.FindElement(By.CssSelector(".card a[title='Edição']"))).Click();

        return new FilmeFormPageObject(driver!);
    }

    public FilmeFormPageObject ClickExcluir()
    {
        wait.Until(d => d?.FindElement(By.CssSelector(".card a[title='Exclusão']"))).Click();

        return new FilmeFormPageObject(driver!);
    }

    public bool ContemFilme(string nome)
    {
        wait.Until(d => d.FindElement(By.CssSelector("a[data-se='btnCadastrar']")).Displayed);

        return driver.PageSource.Contains(nome);
    }
}
