using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloAutentificacao;

namespace ControleDeCinema.Testes.Interface.ModuloSala;

[TestClass]
[TestCategory("Testes de interface de Sala")]
public sealed class SalaInterfaceTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Sala()
    {
        // Arrange
        var authIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!)
            .ClickCriarConta()
            .PreencherEmail("cinema@gmail.com")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Empresa")
            .Confirmar();

        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        // Act
        salaIndex
            .ClickCadastrar()
            .PreencherNumeroSala(1)
            .PreencherCapacidade(100)
            .Confirmar();

        // Assert
        Assert.IsTrue(salaIndex.ContemSala(1));
    }

    [TestMethod]
    public void Deve_Editar_Sala()
    {
        // Arrange
        var authIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!)
            .ClickCriarConta()
            .PreencherEmail("cinema@gmail.com")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Empresa")
            .Confirmar();

        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumeroSala(1)
            .PreencherCapacidade(100)
            .Confirmar();

        // Act
        salaIndex
            .ClickEditar()
            .PreencherNumeroSala(2)
            .PreencherCapacidade(150)
            .Confirmar();

        // Assert
        Assert.IsTrue(salaIndex.ContemSala(2));
    }

    [TestMethod]
    public void Deve_Excluir_Sala()
    {
        // Arrange
        var authIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!)
            .ClickCriarConta()
            .PreencherEmail("cinema@gmail.com")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Empresa")
            .Confirmar();

        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumeroSala(1)
            .PreencherCapacidade(100)
            .Confirmar();

        // Act
        salaIndex
            .ClickExcluir()
            .Confirmar();

        // Assert
        Assert.IsFalse(salaIndex.ContemSala(1));
    }
}
