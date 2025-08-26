using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Testes.Integracao.Compartilhado;

namespace ControleDeCinema.Testes.Integracao.ModuloFilme;

[TestClass]
[TestCategory("Testes de Integração de Filmes")]
public sealed class RepositorioFilmeEmOrmTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Filme_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");

        var filme = new Filme("John Wick", 120, false, genero);
        
        // Act
        repositorioFilme?.Cadastrar(filme);
        dbContext?.SaveChanges();
        
        // Assert
        var registroSelecionado = repositorioFilme?.SelecionarRegistroPorId(filme.Id);
        
        Assert.AreEqual(filme, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Editar_Filme_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        
        var filme = new Filme("John Wick", 120, false, genero);
        
        repositorioFilme?.Cadastrar(filme);
        dbContext?.SaveChanges();
        
        var generoEditado = new GeneroFilme("Suspense");
        
        var filmeEditado = new Filme("John Wick 2", 150, true, generoEditado);
        
        // Act
        var conseguiuEditar = repositorioFilme?.Editar(filme.Id, filmeEditado);
        dbContext?.SaveChanges();
        
        // Assert
        var registroSelecionado = repositorioFilme?.SelecionarRegistroPorId(filme.Id);
        
        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(filme, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Excluir_Filme_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        
        var filme = new Filme("John Wick", 120, false, genero);
        repositorioFilme?.Cadastrar(filme);
        dbContext?.SaveChanges();
        
        // Act
        var conseguiuExcluir = repositorioFilme?.Excluir(filme.Id);
        dbContext?.SaveChanges();
        
        // Assert
        var registroSelecionado = repositorioFilme?.SelecionarRegistroPorId(filme.Id);
        
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Todos_Filmes_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        
        var filme1 = new Filme("John Wick", 120, false, genero);
        var filme2 = new Filme("Mad Max", 130, true, genero);
        var filme3 = new Filme("Carros 2", 110, false, genero);
        
        List<Filme> filmesEsperados = [filme1, filme2, filme3];
        
        repositorioFilme?.CadastrarEntidades(filmesEsperados);
        dbContext?.SaveChanges();
        
        // Act
        var filmesSelecionados = repositorioFilme?.SelecionarRegistros();
        
        // Assert
        CollectionAssert.AreEquivalent(filmesEsperados, filmesSelecionados);
    }
}
