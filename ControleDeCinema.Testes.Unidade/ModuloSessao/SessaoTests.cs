using ControleDeCinema.Dominio.ModuloFilme;
using ControleDeCinema.Dominio.ModuloGeneroFilme;
using ControleDeCinema.Dominio.ModuloSala;
using ControleDeCinema.Dominio.ModuloSessao;

namespace ControleDeCinema.Testes.Unidade.ModuloSessao;

[TestClass]
[TestCategory("Testes de Unidade de Sessão")]
public sealed class SessaoTests
{
    private Sessao sessao;
    private Sala sala;

    [TestInitialize]
    public void Setup()
    {
        var genero = new GeneroFilme("Ação");
        var filme = new Filme("John Wick", 120, false, genero);

        sala = new Sala(1, 3);
        sessao = new Sessao(DateTime.Now, 3, filme, sala);
    }

    [TestMethod]
    public void Deve_Gerar_Ingresso_Com_Assento_Disponivel()
    {
        // Arrange
        int assento = 1;

        // Act
        var ingresso = sessao.GerarIngresso(assento, false);

        // Assert
        Assert.AreEqual(assento, ingresso.NumeroAssento);
        Assert.AreEqual(1, sessao.Ingressos.Count);
        Assert.IsTrue(sessao.Ingressos.Contains(ingresso));
    }

    [TestMethod]
    public void Deve_Retornar_Assentos_Disponiveis_Corretamente()
    {
        // Arrange
        sessao.GerarIngresso(1, false);
        sessao.GerarIngresso(2, true);

        // Act
        var assentosDisponiveis = sessao.ObterAssentosDisponiveis();

        // Assert
        CollectionAssert.AreEquivalent(new[] { 3 }, assentosDisponiveis);
    }

    [TestMethod]
    public void Deve_Retornar_Quantidade_De_Ingressos_Disponiveis()
    {
        // Arrange
        sessao.GerarIngresso(1, false);

        // Act
        var quantidade = sessao.ObterQuantidadeIngressosDisponiveis();

        // Assert
        Assert.AreEqual(2, quantidade);
    }

    [TestMethod]
    public void Nao_Deve_Permitir_Gerar_Ingresso_Quando_Sessao_Esta_Lotada()
    {
        // Arrange
        sessao.GerarIngresso(1, false);
        sessao.GerarIngresso(2, true);
        sessao.GerarIngresso(3, false);

        // Act + Assert
        Assert.ThrowsException<InvalidOperationException>(() =>
            sessao.GerarIngresso(4, false)
        );
    }

    [TestMethod]
    public void Nao_Deve_Permitir_Gerar_Ingresso_Para_Assento_Ja_Ocupado()
    {
        // Arrange
        sessao.GerarIngresso(1, false);

        // Act + Assert
        Assert.ThrowsException<InvalidOperationException>(() =>
            sessao.GerarIngresso(1, true)
        );
    }

    [TestMethod]
    public void Deve_Encerrar_Sessao()
    {
        // Act
        sessao.Encerrar();

        // Assert
        Assert.IsTrue(sessao.Encerrada);
    }
}
