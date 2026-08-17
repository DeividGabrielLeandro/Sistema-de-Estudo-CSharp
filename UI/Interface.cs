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

        AnsiConsole.MarkupLine($"[{Cores.Titulo}]{Textos.TituloAthena}[/]\n\n");

        // Linha de baixo
        AnsiConsole.Write(new Spectre.Console.Rule($"[{Cores.Divisao}]{conteudo.ToUpper()}[/]")
        {
            Style = Style.Parse($"{Cores.Divisao}")
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
    public static void Atualizacoes()
    {
        LimparTelaGeral();
        Titulo("ATUALIZAÇÕES FUTURAS");
        AnsiConsole.MarkupLine(Textos.Atualizacoes);
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
            AnsiConsole.MarkupLine($"[{Cores.TextosDestaque}]Bem-vindo ao Athena![/]\n");
            System.Console.WriteLine(Textos.MensagemInicial);

            opcao = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
            .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
            .AddChoices("Sobre o projeto", "Criar cadastro", "Faça seu login", "Sair")
            .HighlightStyle(new Style(
             foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold
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
                case "Faça seu login":

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
        double TempoFocoHoje = RegistroFoco.TempoFocoHoje(id);
        double TempoFocoSemana = RegistroFoco.TempoFocoSemana(id);
        double TempoFocoMes = RegistroFoco.TempoFocoMes(id);
        bool sair = false;
        string opcao = "";
        string Nome = informacaoCliente.ObterNomeCliente(id);

        while (!sair)
        {

            LimparTelaGeral();
            Titulo("PAINEL DO ESTUDANTE");
            AnsiConsole.MarkupLine($"\n[{Cores.TextosDestaque}]Bem-vindo(a) {Nome}!![/]\n\n");

            var painelEstudante = new Panel(@$"Seus minutos estudados: {tempoEstudo}
Metas criadas: {totalMetas}
Metas pendentes: {metasPendentes}
Metas concluídas: {metasConcluidas}
Tempo de foco hoje: {TempoFocoHoje}
Tempo de foco esta semana: {TempoFocoSemana}
Tempo de foco este mês: {TempoFocoMes}")
.Border(BoxBorder.Rounded)
.BorderColor(Color.FromHex($"{Cores.Opcoes}"))
.Header($"[{Cores.TextosDestaque}]Suas informações[/]", Justify.Center);

            AnsiConsole.Write(painelEstudante);

            System.Console.WriteLine(Textos.MensagemMotivacional_Kant);
            System.Console.WriteLine(Textos.MensagemMotivacional_Conhecimento);


            opcao = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
        .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
        .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
        .AddChoices("Criar nova meta", "Gerenciar metas", "Criar novas categorias", "Escolher categoria", "Iniciar um estudo livre", "Ver historico de metas concluídas", "Visualizar futuras atualizações", "Sair")
        .HighlightStyle(new Style(
        foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold))
         );

            switch (opcao)
            {
                case "Criar nova meta":
                    estudo.CadastrarMeta(id);

                    // Atualiza os indicadores exibidos no painel após o cadastro de uma nova meta.
                    totalMetas = InformacaoCliente.ContarTodasMetas(id);
                    metasPendentes = InformacaoCliente.ContarMetasPendentes(id);
                    break;
                case "Gerenciar metas":
                    Interface.MostrarOpcoesMetas(id);
                    metasPendentes = InformacaoCliente.ContarMetasPendentes(id);
                    metasConcluidas = InformacaoCliente.ContarMetasConcluidas(id);
                    break;
                case "Criar novas categorias":
                    Categoria.CriarCategoria(id);
                    break;
                case "Escolher categoria":
                    int id_categoria = Categoria.MostrarCategorias(id);
                    if (id_categoria != -1)
                    {
                        GerenciaCategorias.IniciarCategoria(id_categoria, id);
                    }

                    break;
                case "Iniciar um estudo livre":
                    ResultadoSessao sessao = Cronometro.ContarTempo();
                    RegistroFoco.SalvarFoco(id, sessao.MinutosLiquidos, "ESTUDO_LIVRE");
                    Cronometro.AtualizarTempoTotalCliente(id, sessao.MinutosLiquidos);
                    break;
                case "Ver historico de metas concluídas":
                    AnsiConsole.Write(GerenciaMetas.HistoricoMetasConcluídas(id));
                    Mensagens.Sair();
                    break;
                case "Visualizar futuras atualizações":
                    Atualizacoes();
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
            Titulo("VISUALIZAR METAS");

            opcao = AnsiConsole.Prompt(
new SelectionPrompt<string>()
        .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
.HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
.AddChoices(
"Ver todas as metas", "Pesquisar", "Filtros", "Ordenar", "Sair")
);

            switch (opcao)
            {
                case "Ver todas as metas":
                    ListarEstudo.MostrarMetas(id, false);
                    break;
                case "Pesquisar":
                    System.Console.WriteLine("Pesquise: ");
                    string pesquisa = Console.ReadLine()!;
                    ListarEstudo.PesquisarMeta(pesquisa, id);

                    break;
                case "Filtros":
                    Interface.FiltrosMetas(id, "Filtros");
                    break;
                case "Ordenar":
                    Interface.FiltrosMetas(id, "Ordenação");
                    break;
                case "Sair":
                    sair = true;
                    break;

            }

        }
    }

    public static void FiltrosMetas(int id, string escolha)
    {
        string opcao = "";
        bool sair = false;
        while (!sair)
        {
            if (escolha == "Filtros")
            {
                LimparTelaGeral();
                Titulo("FILTROS");

                opcao = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
            .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
    .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
    .AddChoices(
     "Mostrar ultimas metas criadas", "Mostrar metas concluídas",
    "Mostrar metas pendentes", "Sair")
    );

                switch (opcao)
                {
                    case "Mostrar metas concluídas":
                        ListarEstudo.MostrarMetasConcluidas(id);
                        break;
                    case "Mostrar metas pendentes":
                        ListarEstudo.MostrarMetasPendentes(id);
                        break;
                    case "Mostrar ultimas metas criadas":
                        ListarEstudo.MostrarUltimasCriadas(id);
                        break;
                    case "Sair":
                        sair = true;
                        break;

                }

            }
            else if (escolha == "Ordenação")
            {
                LimparTelaGeral();
                Titulo("ORDENAR");

                opcao = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
            .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
    .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
    .AddChoices(
     "Ordenar por título", "Ordenar por tempo estudado", "Sair")
    );

                switch (opcao)
                {
                    case "Ordenar por título":
                        ListarEstudo.OrdenarPorTitulo(id);
                        break;
                    case "Ordenar por tempo estudado":
                        ListarEstudo.OrdenarPorTempoEstudado(id);
                        break;
                    case "Sair":
                        sair = true;
                        break;

                }
            }

        }
    }
}
