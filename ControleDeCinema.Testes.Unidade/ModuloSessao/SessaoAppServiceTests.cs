using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Testes.Unidade.Compartilhado;
using Moq;

namespace ControleDeCinema.Testes.Unidade.ModuloSessao;

[TestClass]
[TestCategory("Testes de Unidade de Sessão")]
public sealed class SessaoAppServiceTests : TestFixture
{
    [TestMethod]
    public void Cadastrar_Deve_Retornar_Ok_Quando_Sessao_For_Valida()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("John Wick", 120, false, genero);
        var sala = new Sala(1, 50);

        var sessao = new Sessao(DateTime.UtcNow, 50, filme, sala);
        var sessaoTeste = new Sessao(DateTime.UtcNow.AddHours(3), 50, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoTeste });

        // Act
        var resultado = sessaoAppService?.Cadastrar(sessao);

        // Assert
        repositorioSessaoMock?.Verify(r => r.Cadastrar(sessao), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);

        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Cadastrar_Deve_Retornar_Erro_Quando_Conflito_De_Horario_Sessao()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("John Wick", 120, false, genero);
        var sala = new Sala(1, 50);

        var sessao = new Sessao(DateTime.Today.AddHours(20), 50, filme, sala);
        var sessaoExistente = new Sessao(DateTime.Today.AddHours(21), 50, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>() { sessaoExistente });

        // Act
        var resultado = sessaoAppService?.Cadastrar(sessao);

        // Assert
        repositorioSessaoMock?.Verify(r => r.Cadastrar(It.IsAny<Sessao>()), Times.Never);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Never);

        Assert.IsNotNull(resultado);

        var mensagemErro = resultado.Errors.First()?.Message;

        Assert.AreEqual("Registro duplicado", mensagemErro);
        Assert.IsTrue(resultado.IsFailed);
    }

    [TestMethod]
    public void Cadastrar_Deve_Retornar_Erro_Quando_Excecao_For_Lancada()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("John Wick", 120, false, genero);
        var sala = new Sala(1, 50);

        var sessao = new Sessao(DateTime.UtcNow, 50, filme, sala);

        repositorioSessaoMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sessao>());

        unitOfWorkMock?
            .Setup(u => u.Commit())
            .Throws(new Exception("Erro inesperado"));

        // Act
        var resultado = sessaoAppService?.Cadastrar(sessao);

        // Assert
        unitOfWorkMock?.Verify(u => u.Rollback(), Times.Once);

        Assert.IsNotNull(resultado);

        var mensagemErro = resultado.Errors.First()?.Message;
        
        Assert.AreEqual("Ocorreu um erro interno do servidor", mensagemErro);
        Assert.IsTrue(resultado.IsFailed);
    }
}
