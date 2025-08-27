using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloAutentificacao;
using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;

namespace ControleDeCinema.Testes.Interface.ModuloFilme;

[TestClass]
[TestCategory("Testes de interface de Filme")]
public sealed class FilmeInterfaceTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Filme()
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
        var filmeIndex = new FilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("John Wick")
            .PreencherDuracao(120)
            .SelecionarGenero("Ação")
            .Confirmar();

        // Assert
        Assert.IsTrue(filmeIndex.ContemFilme("John Wick"));
    }

    [TestMethod]
    public void Deve_Editar_Filme()
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

        var filmeIndex = new FilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("John Wick")
            .PreencherDuracao(120)
            .SelecionarGenero("Ação")
            .Confirmar();

        // Act
        filmeIndex
            .ClickEditar()
            .PreencherTitulo("John Wick 2")
            .PreencherDuracao(130)
            .SelecionarGenero("Ação")
            .Confirmar();

        // Assert
        Assert.IsTrue(filmeIndex.ContemFilme("John Wick 2"));
    }

    [TestMethod]
    public void Deve_Excluir_Filme()
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

        var filmeIndex = new FilmeIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        filmeIndex
            .ClickCadastrar()
            .PreencherTitulo("John Wick")
            .PreencherDuracao(120)
            .SelecionarGenero("Ação")
            .Confirmar();

        // Act
        filmeIndex
            .ClickExcluir()
            .Confirmar();

        // Assert
        Assert.IsFalse(filmeIndex.ContemFilme("John Wick"));
    }
}
