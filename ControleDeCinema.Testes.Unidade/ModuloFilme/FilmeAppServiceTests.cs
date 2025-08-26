using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Testes.Unidade.Compartilhado;
using Moq;

namespace ControleDeCinema.Testes.Unidade.ModuloFilme;

[TestClass]
[TestCategory("Testes de Unidade de Filme")]
public sealed class FilmeAppServiceTests : TestFixture
{
    [TestMethod]
    public void Cadastrar_Deve_Retornar_Ok_Quando_Filme_For_Valido()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");

        var filme = new Filme("John Wick", 130, false, genero);
        var filmeTeste = new Filme("John Wick 2", 150, true, genero);

        repositorioFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>() { filmeTeste });

        // Act
        var resultado = filmeAppService?.Cadastrar(filme);

        // Assert
        repositorioFilmeMock?.Verify(r => r.Cadastrar(filme), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Cadastrar_Deve_Retornar_Erro_Quando_Tempo_For_Negativo()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");

        var filme = new Filme("John Wick", -130, false, genero);

        repositorioFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>());

        // Act
        var resultado = filmeAppService?.Cadastrar(filme);

        // Assert
        repositorioFilmeMock?.Verify(r => r.Cadastrar(filme), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);

        var mensagemErro = resultado.Errors.First()?.Message;

        Assert.AreEqual("Requisição inválida", mensagemErro);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Cadastrar_Deve_Retornar_Erro_Quando_Filme_For_Duplicado()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");

        var filme = new Filme("John Wick", 130, false, genero);
        var filmeTeste = new Filme("John Wick", 130, false, genero);

        repositorioFilmeMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Filme>() { filmeTeste });

        // Act
        var resultado = filmeAppService?.Cadastrar(filme);

        // Assert
        repositorioFilmeMock?.Verify(r => r.Cadastrar(filme), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsFailed);
    }
    
    [TestMethod]
    public void Cadastrar_Deve_Retornar_Erro_Quando_Excecao_For_Lancada()
    {
        // Arrange
        var sala = new Sala(1, 100);

        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>());

        unitOfWorkMock?
            .Setup(u => u.Commit())
            .Throws(new Exception("Erro Inesperado"));

        // Act
        var resultado = salaAppService?.Cadastrar(sala);

        // Assert
        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);

        Assert.IsNotNull(resultado);

        var mensagemErro = resultado.Errors.First()?.Message;

        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);
        Assert.IsTrue(resultado.IsFailed);
    }
}
