using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Testes.Unidade.Compartilhado;
using Moq;

namespace ControleDeCinema.Testes.Unidade.ModuloGeneroFilme;

[TestClass]
[TestCategory("Testes de Unidade de Gênero de Filme")]
public sealed class GeneroFilmeAppServiceTests : TestFixture
{
    [TestMethod]
    public void Cadastrar_Deve_Retornar_Ok_Quando_Genero_Filme_For_Valido()
    {
        // Arrange
        var genero = new GeneroFilme("Drama");

        var generoTeste = new GeneroFilme("Ação");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>() { generoTeste });

        // Act
        var resultado = generoFilmeAppService?.Cadastrar(genero);

        // Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Cadastrar(genero), Times.Once);

        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Cadastrar_Deve_Retornar_Erro_Quando_Genero_Filme_For_Duplicada()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        
        var generoTeste = new GeneroFilme("Ação");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>() { generoTeste });
        
        // Act
        var resultado = generoFilmeAppService?.Cadastrar(genero);
        
        // Assert
        repositorioGeneroFilmeMock?.Verify(r => r.Cadastrar(genero), Times.Never);
        
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);
        
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Cadastrar_Deve_Retornar_Erro_Quando_Excecao_For_Lancada()
    {
        // Arrange
        var genero = new GeneroFilme("Terror");

        repositorioGeneroFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<GeneroFilme>());

        unitOfWorkMock?
            .Setup(u => u.Commit())
            .Throws(new Exception("Erro Inesperado"));

        // Act
        var resultado = generoFilmeAppService?.Cadastrar(genero);

        // Assert
        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);

        Assert.IsNotNull(resultado);

        var mensagemErro = resultado.Errors.First()?.Message;

        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);
        Assert.IsTrue(resultado.IsFailed);
    }
}
