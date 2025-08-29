using ControleDeCinema.Testes.Interface.Compartilhado;

namespace ControleDeCinema.Testes.Interface.ModuloAutentificacao;

[TestClass]
[TestCategory("Testes de interface de Autentificação")]
public sealed class AutentificacaoInterfaceTests : TestFixture
{
    [TestMethod]
    public void Deve_Cadastrar_Empresa()
    {
        // Arrange
        var autentificacaoIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!);

        // Act
        autentificacaoIndex
            .ClickCriarConta()
            .PreencherEmail("cinema@gmail.com")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Empresa")
            .Confirmar();

        // Assert
        Assert.IsTrue(autentificacaoIndex.ContemLogin("cinema@gmail.com"));
    }

    [TestMethod]
    public void Deve_Cadastrar_Cliente()
    {
        // Arrange
        var autentificacaoIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!);

        // Act
        autentificacaoIndex
            .ClickCriarConta()
            .PreencherEmail("cliente@gmail.com")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        // Assert
        Assert.IsTrue(autentificacaoIndex.ContemLogin("cliente@gmail.com"));
    }

    [TestMethod]
    public void Deve_Realizar_Login_Com_Crendenciais_Validas()
    {
        // Arrange
        var autentificacaoIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!);

        autentificacaoIndex
            .ClickCriarConta()
            .PreencherEmail("cliente@gmail.com")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        // Act
        autentificacaoIndex
            .ClickUsuario()
            .ClickLoggout()
            .PreencherEmail("cliente@gmail.com")
            .PreencherSenha("Senha12345")
            .Confirmar(); 

        // Assert
        Assert.IsTrue(autentificacaoIndex.ContemLogin("cliente@gmail.com"));
    }

    [TestMethod]
    public void Nao_Deve_Realizar_Login_Com_Crendenciais_Invalidas()
    {
        // Arrange
        var autentificacaoIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!);

        autentificacaoIndex
            .ClickCriarConta()
            .PreencherEmail("cliente@gmail.com")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        // Act
        var loginPage = autentificacaoIndex
            .ClickUsuario()
            .ClickLoggout();
        
        loginPage
            .PreencherEmail("clienteTeste@gmail.com")
            .PreencherSenha("12345Senha")
            .Confirmar();

        // Assert
        Assert.IsTrue(autentificacaoIndex.ContemErroAlert("Login ou senha incorretos."));
    }

    [TestMethod]
    public void Nao_Deve_Cadastrar_Usuario_Com_Email_Invalido()
    {
        // Arrange
        var autentificacaoIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!);

        // Act
        autentificacaoIndex
            .ClickCriarConta()
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        // Assert
        Assert.IsTrue(autentificacaoIndex.ContemErroEscrita("Email"));
    }

    [TestMethod]
    public void Nao_Deve_Cadastrar_Usuario_Com_Senha_Invalida()
    {
        // Arrange
        var autentificacaoIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!);

        // Act
        autentificacaoIndex
            .ClickCriarConta()
            .PreencherEmail("cliente@gmail.com")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        // Assert
        Assert.IsTrue(autentificacaoIndex.ContemErroEscrita("Senha"));
    }

    [TestMethod]
    public void Nao_Deve_Cadastrar_Usuario_Com_Formato_Email_Invalido()
    {
        // Arrange
        var autentificacaoIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!);

        // Act
        autentificacaoIndex
            .ClickCriarConta()
            .PreencherEmail("cliente")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        // Assert
        Assert.IsTrue(autentificacaoIndex.ContemErroEscrita("Email"));
    }

    [TestMethod]
    public void Nao_Deve_Cadastrar_Usuario_Com_Senha_Curta()
    {
        // Arrange
        var autentificacaoIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!);

        // Act
        autentificacaoIndex
            .ClickCriarConta()
            .PreencherEmail("cliente@gmail.com")
            .PreencherSenha("senha")
            .PreencherConfirmarSenha("senha")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        // Assert
        Assert.IsTrue(autentificacaoIndex.ContemErroAlert("A senha é muito curta."));
    }

    [TestMethod]
    public void Nao_Deve_Cadastrar_Usuario_Com_Senha_Sem_Numeros()
    {
        // Arrange
        var autentificacaoIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!);

        // Act
        autentificacaoIndex
            .ClickCriarConta()
            .PreencherEmail("cliente@gmail.com")
            .PreencherSenha("senhaaaa")
            .PreencherConfirmarSenha("senhaaaa")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        // Assert
        Assert.IsTrue(autentificacaoIndex.ContemErroAlert("A senha deve conter pelo menos um número."));
    }

    [TestMethod]
    public void Nao_Deve_Cadastrar_Usuario_Com_Senha_Sem_Letra_Maiuscula()
    {
        // Arrange
        var autentificacaoIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!);

        // Act
        autentificacaoIndex
            .ClickCriarConta()
            .PreencherEmail("cliente@gmail.com")
            .PreencherSenha("senha12345")
            .PreencherConfirmarSenha("senha12345")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        // Assert
        Assert.IsTrue(autentificacaoIndex.ContemErroAlert("A senha deve conter pelo menos uma letra maiúscula."));
    }

    [TestMethod]
    public void Nao_Deve_Cadastrar_Usuario_Com_Email_Duplicado()
    {
        // Arrange
        var autentificacaoIndex = new AutentificacaoIndexPageObject(driver!)
            .IrParaLogin(enderecoBase!);

        autentificacaoIndex
            .ClickCriarConta()
            .PreencherEmail("cliente@gmail.com")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        // Act
       autentificacaoIndex
            .ClickUsuario()
            .ClickLoggout()
            .ClickCriarContaForm()
            .PreencherEmail("cliente@gmail.com")
            .PreencherSenha("Senha12345")
            .PreencherConfirmarSenha("Senha12345")
            .SelecionarTipoUsuario("Cliente")
            .Confirmar();

        // Assert
        Assert.IsTrue(autentificacaoIndex.ContemErroAlert("Já existe um usuário com esse nome."));
    }
}
