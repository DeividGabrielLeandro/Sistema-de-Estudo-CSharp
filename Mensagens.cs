namespace Init_db;

/// <summary>
/// Centraliza as mensagens exibidas pelo sistema.
/// </summary>
public class Mensagens
{
    #region Mensagens de erro

    public static void Erro_NumeroInvalido()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Digite um número válido!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }
    public static void Erro_PlanoNaoEncontrado(int id_estudo)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Nenhum plano de estudo encontrado com o ID {id_estudo}.");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();

    }
    public static void Erro_CampoVazio()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Este campo não pode ser vazio!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();

    }
    public static void Erro_InformacoesInvalidas()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine("Não foi possível concluir o cadastro. Verifique os dados informados ou utilize outras informações.\n");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();

    }
    public static void Erro_SemInformacoes()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nErro: Não há metas com os dados da pesquisa!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }
    public static void Erro_Cancelada()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nOperação cancelada.");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }
    public static void Erro_LoginErro()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine("Usuário ou senha inválidos!");
        Console.ResetColor();

    }
    #endregion

    #region Mensagens de sucesso


    public static void Sucesso_CadastroSucesso()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nCadastro concluído!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();

    }
    public static void Sucesso_LoginSucesso()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nLogin realizado com sucesso!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();

    }

    public static void Sucesso_MetaCadastrada()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nNova meta cadastrada com sucesso!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }
    public static void Sucesso_AtualizarMeta(string atualizacao)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n{atualizacao} atualizado(a) com sucesso!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }
    public static void Sucesso_FinalizarApagarMeta(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nMeta {mensagem} com sucesso!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }

    #endregion
    #region Mensagens neutras

    public static void Sair()
    {
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }
    #endregion
}