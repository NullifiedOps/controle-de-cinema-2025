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
}
