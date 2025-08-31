using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

public class GeneroFilmeFormPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public GeneroFilmeFormPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        wait.Until(d => d.FindElement(By.CssSelector("[data-se='form']")).Displayed);
    }

    public GeneroFilmeFormPageObject PreencherDescricao(string descricao)
    {
        wait.Until(d =>
            d.FindElement(By.Id("Descricao")).Displayed &&
            d.FindElement(By.Id("Descricao")).Enabled
        );

        var inputNome = driver?.FindElement(By.Id("Descricao"));
        inputNome?.Clear();
        inputNome?.SendKeys(descricao);

        return this;
    }

    public GeneroFilmeIndexPageObject Confirmar()
    {
        new Actions(driver).ScrollByAmount(0, 500).Perform();

        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();

        return new GeneroFilmeIndexPageObject(driver!);
    }
}
