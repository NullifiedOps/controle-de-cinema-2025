using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloSessao;
using ControleDeCinema.Testes.Integracao.Compartilhado;

namespace ControleDeCinema.Testes.Integracao.ModuloSessao;

[TestClass]
[TestCategory("Testes de Integração de Sessão")]
public sealed class RepositorioSessaoEmOrmTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Sessao_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("John Wick", 120, false, genero);
        var sala = new Sala(1, 100);
        
        var sessao = new Sessao(DateTime.UtcNow, 100, filme, sala);
        
        // Act
        repositorioSessao?.Cadastrar(sessao);
        dbContext?.SaveChanges();
        
        // Assert
        var registroSelecionado = repositorioSessao?.SelecionarRegistroPorId(sessao.Id);
        Assert.AreEqual(sessao, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Editar_Sessao_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("John Wick", 120, false, genero);
        var sala = new Sala(1, 100);
        
        var sessao = new Sessao(DateTime.UtcNow, 100, filme, sala);
        
        repositorioSessao?.Cadastrar(sessao);
        dbContext?.SaveChanges();
        
        var novoGenero = new GeneroFilme("Suspense");
        var novoFilme = new Filme("John Wick 2", 150, true, novoGenero);
        var novaSala = new Sala(2, 150);
        
        var sessaoEditada = new Sessao(DateTime.UtcNow, 150, novoFilme, novaSala);
        
        // Act
        var conseguiuEditar = repositorioSessao?.Editar(sessao.Id, sessaoEditada);
        dbContext?.SaveChanges();
        
        // Assert
        var registroSelecionado = repositorioSessao?.SelecionarRegistroPorId(sessao.Id);
        
        Assert.IsTrue(conseguiuEditar);
        Assert.AreEqual(sessao, registroSelecionado);
    }

    [TestMethod]
    public void Deve_Excluir_Sessao_Corretamente()
    {
        // Arrange
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("John Wick", 120, false, genero);
        var sala = new Sala(1, 100);
        
        var sessao = new Sessao(DateTime.UtcNow, 100, filme, sala);
        
        repositorioSessao?.Cadastrar(sessao);
        dbContext?.SaveChanges();
        
        // Act
        var conseguiuExcluir = repositorioSessao?.Excluir(sessao.Id);
        dbContext?.SaveChanges();
        
        // Assert
        var registroSelecionado = repositorioSessao?.SelecionarRegistroPorId(sessao.Id);
        
        Assert.IsTrue(conseguiuExcluir);
        Assert.IsNull(registroSelecionado);
    }

    [TestMethod]
    public void Deve_Selecionar_Todas_Sessoes_Corretamente()
    {
        // Arrange
        var genero1 = new GeneroFilme("Ação");
        var filme1 = new Filme("John Wick", 120, false, genero1);
        var sala1 = new Sala(1, 100);
        var sessao1 = new Sessao(DateTime.UtcNow, 100, filme1, sala1);
        
        var genero2 = new GeneroFilme("Comédia");
        var filme2 = new Filme("Deadpool", 110, true, genero2);
        var sala2 = new Sala(2, 150);
        var sessao2 = new Sessao(DateTime.UtcNow.AddHours(3), 150, filme2, sala2);
        
        var genero3 = new GeneroFilme("Drama");
        var filme3 = new Filme("A Vida é Bela", 130, false, genero3);
        var sala3 = new Sala(3, 200);
        var sessao3 = new Sessao(DateTime.UtcNow.AddHours(6), 200, filme3, sala3);

        List<Sessao> sessoesEsperadas = [sessao1, sessao2, sessao3];

        repositorioSessao?.CadastrarEntidades(sessoesEsperadas);
        dbContext?.SaveChanges();
        
        // Act
        var sessoesSelecionadas = repositorioSessao?.SelecionarRegistros();
        
        // Assert
        CollectionAssert.AreEquivalent(sessoesEsperadas, sessoesSelecionadas);  
    }
}
