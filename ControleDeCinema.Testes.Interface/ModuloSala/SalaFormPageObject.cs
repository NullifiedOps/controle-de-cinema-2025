using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace ControleDeCinema.Testes.Interface.ModuloSala;

public class SalaFormPageObject
{
    private readonly IWebDriver driver;
    private readonly WebDriverWait wait;

    public SalaFormPageObject(IWebDriver driver)
    {
        this.driver = driver;

        wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

        wait.Until(d => d.FindElement(By.CssSelector("[data-se='form']")).Displayed);
    }

    public SalaFormPageObject PreencherNumeroSala(int numero)
    {
        wait.Until(d =>
            d.FindElement(By.Id("Numero")).Displayed &&
            d.FindElement(By.Id("Numero")).Enabled
        );

        var inputNome = driver?.FindElement(By.Id("Numero"));
        inputNome?.Clear();
        inputNome?.SendKeys(numero.ToString());

        return this;
    }

    public SalaFormPageObject PreencherCapacidade(int numero)
    {
        wait.Until(d =>
            d.FindElement(By.Id("Capacidade")).Displayed &&
            d.FindElement(By.Id("Capacidade")).Enabled
        );

        var inputNome = driver?.FindElement(By.Id("Capacidade"));
        inputNome?.Clear();
        inputNome?.SendKeys(numero.ToString());

        return this;
    }

    public GeneroFilmeIndexPageObject Confirmar()
    {
        new Actions(driver).ScrollByAmount(0, 500).Perform();

        wait.Until(d => d.FindElement(By.CssSelector("button[data-se='btnConfirmar']"))).Click();

        return new GeneroFilmeIndexPageObject(driver!);
    }
}
