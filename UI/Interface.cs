using System.Data;
using System.Data.Common;
using System.Runtime.ConstrainedExecution;
using Spectre.Console;

namespace Init_db;

//ABD1C6
public class Interface
{
    /// <summary>
    /// Move o cursor para o início do terminal e limpa completamente a tela,
    /// incluindo o histórico visível do console.
    /// </summary>
    public static void LimparTelaGeral()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write("\x1b[3J");
        Console.Clear();
    }

    /// <summary>
    /// Exibe um título centralizado entre linhas de separação,
    /// padronizando o cabeçalho das telas do sistema.
    /// </summary>
    /// <param name="titulo">Texto que será exibido como título.</param>
    public static void Titulo(string conteudo)
    {
        AnsiConsole.Clear();


        AnsiConsole.MarkupLine($"[#EF0606]{Textos.TituloAthena}[/]\n");

        // Linha de baixo
        AnsiConsole.Write(new Spectre.Console.Rule($"[#D3CCC7]{conteudo.ToUpper()}[/]")
        {
            Style = Style.Parse("#EF0606")
        });
        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Exibe informações sobre o projeto ATHENA.
    /// </summary>
    public static void SobreAthena()
    {
        LimparTelaGeral();
        Titulo("SOBRE O PROJETO");

        AnsiConsole.MarkupLine(Textos.Sobre);

        Mensagens.Sair();
    }


    /// <summary>
    /// Exibe o menu principal do sistema e direciona o usuário
    /// para as funcionalidades disponíveis.
    /// </summary>
    public static void MenuPrincipal()
    {
        Cliente cliente = new Cliente();
        bool clienteLogado = false;
        int id = -1;
        string opcao = "";
        while (!clienteLogado)
        {

            LimparTelaGeral();

            Interface.Titulo("SISTEMA DE GERENCIAMENTO DE ESTUDO");
            AnsiConsole.MarkupLine("Bem-vindo ao [#EFEEE8]Athena![/]\n");
            System.Console.WriteLine(Textos.MensagemInicial);

            opcao = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
            .HighlightStyle(new Style(foreground: Color.FromHex("#EF0606")))
            .AddChoices("Sobre o projeto", "Criar cadastro", "Fazer cadastro", "Sair")
            .HighlightStyle(new Style(
             foreground: Color.FromHex("#EF0606"), decoration: Decoration.Bold
)));

            switch (opcao)
            {
                case "Sobre o projeto":
                    Interface.SobreAthena();
                    break;
                case "Criar cadastro":

                    id = cliente.CadastrarCliente();
                    if (id != -1)
                    {
                        Interface.InterfaceLogin(id);
                    }
                    break;
                case "Fazer cadastro":

                    id = cliente.FazerLogin();

                    if (id != -1)
                    {
                        Interface.InterfaceLogin(id);
                    }
                    break;
                case "Sair":
                    Interface.LimparTelaGeral();
                    Environment.Exit(0);
                    break;
            }
        }
    }


    /// <summary>
    /// Exibe o painel principal do estudante, permitindo o acesso
    /// às funcionalidades disponíveis após a autenticação.
    /// </summary>
    /// <param name="id">Identificador do usuário autenticado.</param>
    public static void InterfaceLogin(int id)
    {
        InformacaoCliente informacaoCliente = new InformacaoCliente();
        Cliente cliente = new Cliente();
        Estudo estudo = new Estudo();
        double tempoEstudo = InformacaoCliente.MostrarTempoTotalEstudo(id);
        double metasPendentes = InformacaoCliente.ContarMetasPendentes(id);
        double metasConcluidas = InformacaoCliente.ContarMetasConcluidas(id);
        double totalMetas = InformacaoCliente.ContarTodasMetas(id);
        bool sair = false;
        string opcao = "";
        string Nome = informacaoCliente.ObterNomeCliente(id);

        while (!sair)
        {

            LimparTelaGeral();
            Titulo("PAINEL DO ESTUDANTE");
            AnsiConsole.MarkupLine($"\nBem-vindo(a) {Nome}!!\n");

            var painelEstudante = new Panel(@$"Seus minutos estudados: {tempoEstudo}
Metas criadas: {totalMetas}
Metas pendentes: {metasPendentes}
Metas concluídas: {metasConcluidas}")
.Border(BoxBorder.Rounded)
.BorderColor(Color.FromHex("#d02c2c"))
.Header("[#EF0606]Suas informações[/]", Justify.Center);

            AnsiConsole.Write(painelEstudante);

            System.Console.WriteLine(Textos.MensagemMotivacional_Kant);
            System.Console.WriteLine(Textos.MensagemMotivacional_Conhecimento);


            opcao = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
        .HighlightStyle(new Style(foreground: Color.FromHex("#EF0606")))
        .AddChoices("Criar nova meta", "Abrir menu para mostrar as metas", "Iniciar um estudo livre", "Sair")
        .HighlightStyle(new Style(
        foreground: Color.FromHex("#EF0606"), decoration: Decoration.Bold))
         );

            switch (opcao)
            {
                case "Criar nova meta":
                    estudo.CadastrarMeta(id);

                    // Atualiza os indicadores exibidos no painel após o cadastro de uma nova meta.
                    totalMetas = InformacaoCliente.ContarTodasMetas(id);
                    metasPendentes = InformacaoCliente.ContarMetasPendentes(id);
                    break;
                case "Abrir menu para mostrar as metas":
                    Interface.MostrarOpcoesMetas(id);
                    metasPendentes = InformacaoCliente.ContarMetasPendentes(id);
                    metasConcluidas = InformacaoCliente.ContarMetasConcluidas(id);
                    break;
                case "Iniciar um estudo livre":
                    double minutos = Cronometro.ContarTempo();

                    // Atualiza o total de minutos exibido ao usuário após a sessão de estudo.
                    Cronometro.AtualizarTempoTotalCliente(id, minutos);
                    tempoEstudo = InformacaoCliente.MostrarTempoTotalEstudo(id);

                    break;
                case "Sair":
                    sair = true;
                    break;

            }
        }
    }


    /// <summary>
    /// Exibe o menu de personalização de uma meta de estudo.
    /// </summary>
    /// <param name="id_estudo">Identificador da meta selecionada.</param>
    public static void PersonalizarMetas(int id_estudo)
    {
        string opcao = "";
        bool sair = false;
        while (!sair)
        {

            LimparTelaGeral();
            Titulo("PERSONALIZE SUAS METAS");

            opcao = AnsiConsole.Prompt(
new SelectionPrompt<string>()
        .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
.HighlightStyle(new Style(foreground: Color.FromHex("#EF0606")))
.AddChoices("Atualizar título", "Atualizar descrição", "Atualizar tempo de meta", "Apagar meta", "Sair")
);

            switch (opcao)
            {
                case "Atualizar título":
                    GerenciaMetas.AtualizarTitulo(id_estudo);
                    break;
                case "Atualizar descrição":
                    GerenciaMetas.AtualizarDescricao(id_estudo);
                    break;
                case "Atualizar tempo de meta":
                    GerenciaMetas.AtualizarMeta(id_estudo);
                    break;
                case "Apagar meta":
                    GerenciaMetas.ApagarMeta(id_estudo);
                    break;
                case "Sair":
                    sair = true;
                    break;
            }
        }
    }


    /// <summary>
    /// Exibe as opções de visualização e pesquisa das metas do usuário.
    /// </summary>
    /// <param name="id">Identificador do usuário.</param>
    public static void MostrarOpcoesMetas(int id)
    {
        string opcao = "";
        bool sair = false;
        while (!sair)
        {
            LimparTelaGeral();
            Titulo("ATHENA - Filtrar metas");

            opcao = AnsiConsole.Prompt(
new SelectionPrompt<string>()
        .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
.HighlightStyle(new Style(foreground: Color.FromHex("#EF0606")))
.AddChoices("Ver todas as metas", "Mostrar apenas as metas concluídas", "Mostrar apenas as metas pendentes", "Pesquisar meta pelo título", "Sair")
);

            switch (opcao)
            {
                case "Ver todas as metas":
                    Estudo.MostrarMetas(id);
                    break;
                case "Mostrar apenas as metas concluídas":
                    Estudo.MostrarMetasConcluidas(id);
                    break;
                case "Mostrar apenas as metas pendentes":
                    Estudo.MostrarMetasPendentes(id);
                    break;
                case "Pesquisar meta pelo título":
                    System.Console.WriteLine("Pesquise: ");
                    string pesquisa = Console.ReadLine()!;
                    Estudo.PesquisarMeta(pesquisa, id);
                    break;
                case "Sair":
                    sair = true;
                    break;

            }

        }
    }
}