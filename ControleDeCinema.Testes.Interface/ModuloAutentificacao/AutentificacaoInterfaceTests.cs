using ControleDeCinema.Testes.Interface.Compartilhado;

namespace ControleDeCinema.Testes.Interface.ModuloAutentificacao;

[TestClass]
[TestCategory("Testes de interface de Autentificação")]
public sealed class AutentificacaoInterfaceTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Empresa_Com_Sucesso()
    {
        // Arrange
        var autentificacaoIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!);

        // Act
        autentificacaoIndex
            .ClickCriarConta()
            .PreencherEmail("cinema@gmail.com")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Empresa")
            .Confirmar();

        // Assert
        Assert.IsTrue(autentificacaoIndex.ComtemLogin("cinema@gmail.com"));
    }
}
