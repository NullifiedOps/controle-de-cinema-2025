﻿using ControleDeCinema.Testes.Interface.Compartilhado;
using ControleDeCinema.Testes.Interface.ModuloAutentificacao;
using ControleDeCinema.Testes.Interface.ModuloFilme;
using ControleDeCinema.Testes.Interface.ModuloGeneroFilme;
using ControleDeCinema.Testes.Interface.ModuloSala;
using OpenQA.Selenium;

namespace ControleDeCinema.Testes.Interface.ModuloSessao;

[TestClass]
[TestCategory("Testes de interface de Sessão")]
public sealed class SessaoInterfaceTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Sessao()
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

        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumeroSala(1)
            .PreencherCapacidade(100)
            .Confirmar();

        var sessaoIndex = new SessaoIndexPageObject(driver!)
            .IrPara(enderecoBase!);
        
        // Act
        var horario = DateTime.Now.AddHours(5);

        sessaoIndex
            .ClickCadastrar()
            .PreencherDataHorarioInicio(horario)
            .PreencherNumeroMaximoIngressos(50)
            .SelecionarFilme("John Wick")
            .SelecionarSala(1)
            .Confirmar();

        // Assert
        Assert.IsTrue(sessaoIndex.ContemSessao("John Wick", horario));
    }

    [TestMethod]
    public void Deve_Editar_Sessao()
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

        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumeroSala(1)
            .PreencherCapacidade(100)
            .Confirmar();

        var sessaoIndex = new SessaoIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        sessaoIndex
            .ClickCadastrar()
            .PreencherDataHorarioInicio(DateTime.Now.AddHours(5))
            .PreencherNumeroMaximoIngressos(100)
            .SelecionarFilme("John Wick")
            .SelecionarSala(1)
            .Confirmar();

        // Act
        var horario = DateTime.Now.AddHours(6);

        sessaoIndex
            .ClickEditar()
            .PreencherDataHorarioInicio(horario)
            .PreencherNumeroMaximoIngressos(90)
            .SelecionarFilme("John Wick")
            .SelecionarSala(1)
            .Confirmar();

        // Assert
        Assert.IsTrue(sessaoIndex.ContemSessao("John Wick", horario));
    }

    [TestMethod]
    public void Deve_Excluir_Sessao()
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

        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumeroSala(1)
            .PreencherCapacidade(100)
            .Confirmar();

        var sessaoIndex = new SessaoIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        var horario = DateTime.Now.AddHours(5);

        sessaoIndex
            .ClickCadastrar()
            .PreencherDataHorarioInicio(horario)
            .PreencherNumeroMaximoIngressos(50)
            .SelecionarFilme("John Wick")
            .SelecionarSala(1)
            .Confirmar();

        // Act
        sessaoIndex
            .ClickEncerrar()
            .Confirmar()
            .IrPara(enderecoBase!);

        sessaoIndex
            .ClickExcluir()
            .Confirmar();

        // Assert
        Assert.IsFalse(sessaoIndex.ContemSessao("John Wick", horario));
    }

    [TestMethod]
    public void Deve_Visualizar_Detalhes_Sessao()
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

        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumeroSala(1)
            .PreencherCapacidade(100)
            .Confirmar();

        var sessaoIndex = new SessaoIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        var horario = DateTime.Now.AddHours(5);

        sessaoIndex
            .ClickCadastrar()
            .PreencherDataHorarioInicio(horario)
            .PreencherNumeroMaximoIngressos(50)
            .SelecionarFilme("John Wick")
            .SelecionarSala(1)
            .Confirmar();

        // Act
        sessaoIndex
            .ClickDetalhes();

        // Assert
        Assert.IsTrue(sessaoIndex.ContemDetalhes("John Wick", horario, 50));
    }

    [TestMethod]
    public void Deve_Validar_Campos_Obrigatorios_Sessao()
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

        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumeroSala(1)
            .PreencherCapacidade(100)
            .Confirmar();

        var sessaoIndex = new SessaoIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        // Act
        var horario = DateTime.Now.AddHours(5);

        sessaoIndex
            .ClickCadastrar()
            .PreencherDataHorarioInicio(horario)
            .SelecionarFilme("John Wick")
            .SelecionarSala(1)
            .Confirmar();

        // Assert
        Assert.IsTrue(sessaoIndex.ContemErroSpan("NumeroMaximoIngressos"));
    }

    [TestMethod]
    public void Deve_Validar_Horario_Conflitante_Sessao()
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

        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumeroSala(1)
            .PreencherCapacidade(100)
            .Confirmar();

        var sessaoIndex = new SessaoIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        var horario = DateTime.Now.AddHours(5);

        sessaoIndex
            .ClickCadastrar()
            .PreencherDataHorarioInicio(horario)
            .PreencherNumeroMaximoIngressos(50)
            .SelecionarFilme("John Wick")
            .SelecionarSala(1)
            .Confirmar();

        // Act
        sessaoIndex
            .ClickCadastrar()
            .PreencherDataHorarioInicio(horario.AddHours(1))
            .PreencherNumeroMaximoIngressos(50)
            .SelecionarFilme("John Wick")
            .SelecionarSala(1)
            .Confirmar();

        // Assert
        Assert.IsTrue(sessaoIndex.ContemErroAlert("Já existe uma sessão nesta sala que conflita com o horário informado."));
    }

    [TestMethod]
    public void Deve_Comprar_Ingresso_Sessao()
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

        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumeroSala(1)
            .PreencherCapacidade(2)
            .Confirmar();

        var sessaoIndex = new SessaoIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        var horario = DateTime.Now.AddHours(5);

        sessaoIndex
            .ClickCadastrar()
            .PreencherDataHorarioInicio(horario)
            .PreencherNumeroMaximoIngressos(2)
            .SelecionarFilme("John Wick")
            .SelecionarSala(1)
            .Confirmar();

        // Act
        authIndex
            .ClickUsuario()
            .ClickLoggout();

        authIndex
            .ClickCriarConta()
            .PreencherEmail("cliente@gmail.com")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        sessaoIndex
            .IrPara(enderecoBase!)
            .ClickComprarIngresso()
            .Confirmar();

        // Assert
        Assert.IsTrue(sessaoIndex.ContemIngresso("John Wick"));
    }

    [TestMethod]
    public void Deve_Validar_Ingresso_Sessao_Lotada()
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

        var salaIndex = new SalaIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        salaIndex
            .ClickCadastrar()
            .PreencherNumeroSala(1)
            .PreencherCapacidade(2)
            .Confirmar();

        var sessaoIndex = new SessaoIndexPageObject(driver!)
            .IrPara(enderecoBase!);

        var horario = DateTime.Now.AddHours(5);

        sessaoIndex
            .ClickCadastrar()
            .PreencherDataHorarioInicio(horario)
            .PreencherNumeroMaximoIngressos(2)
            .SelecionarFilme("John Wick")
            .SelecionarSala(1)
            .Confirmar();

        authIndex
            .ClickUsuario()
            .ClickLoggout();

        authIndex
            .ClickCriarConta()
            .PreencherEmail("cliente@gmail.com")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        sessaoIndex
            .IrPara(enderecoBase!);
        
        // Act
        sessaoIndex
            .ClickComprarIngresso()
            .Confirmar();

        sessaoIndex
            .ClickComprarIngresso()
            .Confirmar();

        sessaoIndex
            .ClickComprarIngresso()
            .Confirmar();

        // Assert
        Assert.IsTrue(sessaoIndex.ContemErroSpan("Assento"));
    }
}
