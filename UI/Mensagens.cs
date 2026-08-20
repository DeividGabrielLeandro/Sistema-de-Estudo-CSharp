namespace Init_db;
using Spectre.Console;

/// <summary>
/// Centraliza as mensagens exibidas pelo sistema.
/// </summary>
public class Mensagens
{
    #region Mensagens de erro

    /// <summary>
    /// Exibe uma mensagem indicando erro por entrada de número inválido.
    /// </summary>
    public static void Erro_NumeroInvalido()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Digite um número válido!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }

    /// <summary>
    /// Exibe uma mensagem informando que o plano de estudo consultado não foi localizado.
    /// </summary>
    /// <param name="id_estudo">Identificador do plano de estudo buscado.</param>
    public static void Erro_PlanoNaoEncontrado(int id_estudo)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Nenhum plano de estudo encontrado com o ID {id_estudo}.");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();

    }

    /// <summary>
    /// Exibe uma mensagem indicando que um campo obrigatório foi deixado em branco.
    /// </summary>
    public static void Erro_CampoVazio()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Este campo não pode ser vazio!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();

    }

    /// <summary>
    /// Exibe uma mensagem informando falha durante o processo de cadastro devido a dados inválidos.
    /// </summary>
    public static void Erro_InformacoesInvalidas()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine("\nNão foi possível concluir o cadastro. Verifique os dados informados ou utilize outras informações.");
        Console.ResetColor();

    }

    /// <summary>
    /// Exibe uma mensagem indicando que nenhuma meta foi encontrada para os critérios de busca inseridos.
    /// </summary>
    public static void Erro_SemInformacoes()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nErro: Não há metas com os dados da pesquisa!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }

    /// <summary>
    /// Exibe uma mensagem avisando o cancelamento de uma operação.
    /// </summary>
    public static void Erro_Cancelada()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nOperação cancelada.");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }

    /// <summary>
    /// Exibe uma mensagem de falha de autenticação ao tentar realizar o login.
    /// </summary>
    public static void Erro_LoginErro()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine("Usuário ou senha inválidos!");
        Console.ResetColor();

    }
    #endregion

    #region Mensagens de sucesso

    /// <summary>
    /// Exibe uma mensagem informando que o cadastro foi realizado com êxito.
    /// </summary>
    public static void Sucesso_CadastroSucesso()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nCadastro concluído!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();

    }

    /// <summary>
    /// Exibe uma mensagem informando a autenticação do usuário efetuada com êxito.
    /// </summary>
    public static void Sucesso_LoginSucesso()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nLogin realizado com sucesso!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();

    }

    /// <summary>
    /// Exibe uma mensagem indicando que a inclusão de uma nova meta foi concluída com sucesso.
    /// </summary>
    public static void Sucesso_MetaCadastrada()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nNova meta cadastrada com sucesso!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }

    /// <summary>
    /// Exibe uma mensagem de confirmação após atualizar as informações de uma meta.
    /// </summary>
    /// <param name="atualizacao">Nome do campo ou dado que foi alterado.</param>
    public static void Sucesso_AtualizarMeta(string atualizacao)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n{atualizacao} atualizado(a) com sucesso!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }

    /// <summary>
    /// Exibe uma mensagem informando a conclusão ou remoção de uma meta.
    /// </summary>
    /// <param name="mensagem">Descrição do status/ação aplicada à meta.</param>
    public static void Sucesso_FinalizarApagarMeta(string mensagem)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nMeta {mensagem} com sucesso!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }

    /// <summary>
    /// Exibe uma mensagem de confirmação para a criação de uma nova categoria.
    /// </summary>
    public static void CriarCategoria()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nCategoria criada com sucesso!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }

    /// <summary>
    /// Exibe uma mensagem informando que a meta foi removida.
    /// </summary>
    public static void RemoverMeta()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nMeta removida com sucesso!!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }

    /// <summary>
    /// Exibe uma mensagem confirmando a alteração do título da categoria.
    /// </summary>
    public static void AtualizarNomeCatetoria()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\nTitulo atualizado com sucesso!!");
        Console.ResetColor();
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }

    #endregion
    #region Mensagens neutras

    /// <summary>
    /// Solicita a interação do usuário para retornar ao menu anterior.
    /// </summary>
    public static void Sair()
    {
        System.Console.WriteLine("\nAperte qualquer tecla para voltar.");
        Console.ReadKey();
    }
    #endregion

    /// <summary>
    /// Exibe um menu de confirmação perguntando se o usuário deseja tentar novamente ou sair.
    /// </summary>
    /// <param name="resposta">Variável auxiliar contendo a seleção do usuário.</param>
    /// <returns>Retorna a opção selecionada ("Tentar novamente" ou "Sair").</returns>
    public static string TentarNovamente(string resposta)
    {
        resposta = AnsiConsole.Prompt(new SelectionPrompt<string>()
             .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7] Tentar novamente?[/]\n[#D3CCC7]─────────────────────────────────[/]")
             .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
             .AddChoices("Tentar novamente","Sair")
             .HighlightStyle(new Style(
              foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold
             )));

        return resposta;
    }

    /// <summary>
    /// Exibe um prompt solicitando ao usuário a decisão de iniciar ou não um plano de estudo.
    /// </summary>
    /// <returns>Retorna a resposta escolhida ("Sim" ou "Não").</returns>
    public static string IniciarEstudo()
    {
        string resposta = "";
        resposta = AnsiConsole.Prompt(new SelectionPrompt<string>()
             .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7] Iniciar algum plano de estudo?[/]\n[#D3CCC7]─────────────────────────────────[/]")
             .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
             .AddChoices("Sim","Não")
             .HighlightStyle(new Style(
              foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold
             )));

        return resposta;
    }

    /// <summary>
    /// Exibe um prompt perguntando se o usuário deseja vincular uma meta à categoria selecionada.
    /// </summary>
    /// <returns>Retorna a resposta escolhida ("Sim" ou "Não").</returns>
    public static string AdicionarCategoria()
    {
        string resposta = "";
        resposta = AnsiConsole.Prompt(new SelectionPrompt<string>()
             .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]Selecione a meta que será vinculada à categoria[/]\n[#D3CCC7]─────────────────────────────────[/]")
             .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
             .AddChoices("Sim","Não")
             .HighlightStyle(new Style(
              foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold
             )));

        return resposta;
    }
}