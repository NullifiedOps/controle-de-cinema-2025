using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Testes.Unidade.Compartilhado;
using Moq;

namespace ControleDeCinema.Testes.Unidade.ModuloSala;

[TestClass]
[TestCategory("Testes de Unidade de Sala")]
public sealed class SalaAppServiceTests : TestFixture
{
    [TestMethod]
    public void Cadastrar_Deve_Retornar_Ok_Quando_Sala_For_Valida()
    {
        // Arrange
        var sala = new Sala(1, 50);
        var salaTeste = new Sala(2, 100);
        
        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>() { salaTeste });
        
        // Act
        var resultado = salaAppService?.Cadastrar(sala);
        
        // Assert
        repositorioSalaMock?.Verify(r => r.Cadastrar(sala), Times.Once);
        unitOfWorkMock?.Verify(u => u.Commit(), Times.Once);
        
        Assert.IsNotNull(resultado);
        Assert.IsTrue(resultado.IsSuccess);
    }

    [TestMethod]
    public void Cadastrar_Deve_Retornar_Erro_Quando_Sala_For_Duplicada()
    {
        // Arrange
        var sala = new Sala(1, 50);
        var salaTeste = new Sala(1, 100);
        
        repositorioSalaMock?
            .Setup(r => r.SelecionarRegistros())
            .Returns(new List<Sala>() { salaTeste });
        
        // Act
        var resultado = salaAppService?.Cadastrar(sala);
        
        // Assert
        repositorioSalaMock?.Verify(r => r.Cadastrar(sala), Times.Never);
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
