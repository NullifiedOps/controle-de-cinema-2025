using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Testes.Integracao.Compartilhado;

namespace ControleDeCinema.Testes.Integracao.ModuloGeneroFilme;

[TestClass]
[TestCategory("Testes de Integração de Gênero de Filme")]
public sealed class RepositorioGeneroFilmeEmOrmTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Genero_Filme_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Suspense");

        // Act
        repositorioGeneroFilme?.Cadastrar(genero);
        dbContext?.SaveChanges();

        // Assert
        var registroSelecionado = repositorioGeneroFilme?.SelecionarRegistroPorId(genero.Id);

        Assert.AreEqual(genero, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Editar_Genero_Filme_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        
        repositorioGeneroFilme?.Cadastrar(genero);
        dbContext?.SaveChanges();

        var generoEditado = new GeneroFilme("Aventura");

        // Act
        var conseguiuEditar = repositorioGeneroFilme?.Editar(genero.Id, generoEditado);
        dbContext?.SaveChanges();
        
        // Assert
        var registroSelecionado = repositorioGeneroFilme?.SelecionarRegistroPorId(genero.Id);
        
        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(genero, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Excluir_Genero_Filme_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Comédia");
        repositorioGeneroFilme?.Cadastrar(genero);
        dbContext?.SaveChanges();
        
        // Act
        var conseguiuExcluir = repositorioGeneroFilme?.Excluir(genero.Id);
        dbContext?.SaveChanges();
        
        // Assert
        var registroSelecionado = repositorioGeneroFilme?.SelecionarRegistroPorId(genero.Id);
        
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Generos_Filme_Corretamente()
    {
        // Arrange
        var genero1 = new GeneroFilme("Drama");
        var genero2 = new GeneroFilme("Romance");
        var genero3 = new GeneroFilme("Terror");

        List<GeneroFilme> generosEsperados = [genero1, genero2, genero3];

        repositorioGeneroFilme?.CadastrarEntidades(generosEsperados);
        dbContext?.SaveChanges();

        // Act
        var generosSelecionados = repositorioGeneroFilme?.SelecionarRegistros();

        // Assert
        CollectionAssert.AreEquivalent(generosEsperados, generosSelecionados);
    }
}
