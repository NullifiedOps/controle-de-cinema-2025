using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloAutentificacao;

namespace ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

[TestClass]
[TestCategory("Testes de interface de Gênero de Filme")]
public sealed class GeneroFilmeInterfaceTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_GeneroDeFilme()
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

        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);
        
        // Act
        generoFilmeIndex
            .ClickCadastrar()
            .PreencherDescricao("Ação")
            .Confirmar();
        
        // Assert
        Assert.IsTrue(generoFilmeIndex.ContemGenero("Ação"));
    }

    [TestMethod]
    public void Deve_Editar_GeneroDeFilme()
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

        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        generoFilmeIndex
            .ClickCadastrar()
            .PreencherDescricao("Ação")
            .Confirmar();

        // Act
        generoFilmeIndex
            .ClickEditar()
            .PreencherDescricao("Suspense")
            .Confirmar();

        // Assert
        Assert.IsTrue(generoFilmeIndex.ContemGenero("Suspense"));
    }

    [TestMethod]
    public void Deve_Excluir_GeneroDeFilme()
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

        var generoFilmeIndex = new GeneroFilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        var cadastro = generoFilmeIndex
            .ClickCadastrar();

        cadastro
            .PreencherDescricao("Ação")
            .Confirmar();

        // Act
        generoFilmeIndex
            .ClickExcluir()
            .Confirmar();

        // Assert
        Assert.IsFalse(generoFilmeIndex.ContemGenero("Ação"));
    }
}
