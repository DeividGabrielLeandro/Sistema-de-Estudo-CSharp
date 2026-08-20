namespace Init_db;

using Spectre.Console;
using Microsoft.Data.SqlClient;

/// <summary>
/// Controla o fluxo de visualização, interação, personalização e construção visual das metas.
/// </summary>
public class GerenciaMetas
{
    /// <summary>
    /// Exibe os detalhes de uma meta de estudo e apresenta o menu principal de ações disponíveis.
    /// </summary>
    /// <param name="id_cliente">O identificador do cliente.</param>
    /// <param name="id_estudo">O identificador da meta selecionada.</param>
    public static void IniciarEstudo(int id_cliente, int id_estudo)
    {
        bool Sair = false;
        while (!Sair)
        {
            using (SqlConnection conn = new SqlConnection(Banco.Conexao))
            {
                conn.Open();
                string sql = "SELECT * FROM Estudo WHERE id = @id AND id_cliente = @id_cliente";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id_estudo);
                    cmd.Parameters.AddWithValue("@id_cliente", id_cliente);

                    using (var Reader = cmd.ExecuteReader())
                    {
                        Interface.LimparTelaGeral();
                        Interface.Titulo("SEU PLANO");

                        AnsiConsole.Write($"\n{Textos.MensagemMotivacional_Seneca}\n\n");

                        if (Reader.Read())
                        {
                            // Renderiza o painel de detalhes da meta
                            AnsiConsole.Write(GerenciaMetas.InformacoesMetas(Reader));

                            string opcao = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
                                .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold))
                                .AddChoices("Começar a contar o tempo", "Abrir sessão de estudo", "Escolher sessão de estudo", "Ver histórico de sessões de estudo", "Abrir as opções", "Marcar como finalizada", "Definir prioridade", "Definir data limite", "Vincular à uma categoria criada", "Sair"));

                            switch (opcao)
                            {
                                case "Começar a contar o tempo":
                                    ResultadoSessao sessao = Cronometro.ContarTempo();
                                    Cronometro.SalvarTempo(id_estudo, sessao.MinutosLiquidos);
                                    Cronometro.AtualizarTempoTotalCliente(id_cliente, sessao.MinutosLiquidos);
                                    RegistroFoco.SalvarFoco(id_cliente, sessao.MinutosLiquidos, "META");
                                    break;

                                case "Abrir sessão de estudo":
                                    Sessao_Estudo.CriarSessao(id_cliente, id_estudo);
                                    break;

                                case "Escolher sessão de estudo":
                                    int id_gerado = Sessao_Estudo.MostrarSessao(id_estudo);
                                    if (id_gerado != -1)
                                    {
                                        GerenciaSessao_Estudo.Interface_Sessao(id_estudo, id_gerado, id_cliente);
                                    }
                                    break;

                                case "Ver histórico de sessões de estudo":
                                    AnsiConsole.Write(GerenciaSessao_Estudo.HistoricoSessoes(id_estudo));
                                    Mensagens.Sair();
                                    break;

                                case "Abrir as opções":
                                    PersonalizarMetas(id_estudo);
                                    break;

                                case "Marcar como finalizada":
                                    AtualizarEstudo.MarcarFinalizada(id_estudo);
                                    break;

                                case "Definir data limite":
                                    DateTime? Data = Estudo.Escolher(id_estudo);
                                    Estudo.AtualizarData(id_estudo, Data);
                                    break;

                                case "Definir prioridade":
                                    Estudo.DefinirPrioridade(id_estudo);
                                    break;

                                case "Vincular à uma categoria criada":
                                    int id = Categoria.MostrarCategorias(id_cliente);
                                    if (id != -1)
                                    {
                                        Categoria.VincularCategoria(id, id_estudo);
                                    }
                                    break;

                                case "Sair":
                                    Sair = true;
                                    break;
                            }
                        }
                        else
                        {
                            Mensagens.Erro_PlanoNaoEncontrado(id_estudo);
                            Sair = true;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Exibe o menu interativo com opções de edição das propriedades da meta.
    /// </summary>
    /// <param name="id_estudo">O identificador da meta.</param>
    public static void PersonalizarMetas(int id_estudo)
    {
        bool sair = false;
        while (!sair)
        {
            Interface.LimparTelaGeral();
            Interface.Titulo("PERSONALIZE SUAS METAS");

            string opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
                    .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
                    .AddChoices("Atualizar título", "Atualizar descrição", "Atualizar tempo de meta", "Apagar meta", "Sair")
            );

            switch (opcao)
            {
                case "Atualizar título":
                    AtualizarEstudo.AtualizarTitulo(id_estudo);
                    break;
                case "Atualizar descrição":
                    AtualizarEstudo.AtualizarDescricao(id_estudo);
                    break;
                case "Atualizar tempo de meta":
                    AtualizarEstudo.AtualizarMeta(id_estudo);
                    break;
                case "Apagar meta":
                    bool apagou = AtualizarEstudo.ApagarMeta(id_estudo);
                    if (apagou)
                        return;
                    break;
                case "Sair":
                    sair = true;
                    break;
            }
        }
    }

    /// <summary>
    /// Constrói uma tabela formatada contendo todas as metas lidas do leitor de dados SQL.
    /// </summary>
    /// <param name="Reader">O leitor com o resultado da consulta SQL contendo os registros de metas.</param>
    /// <param name="MetaEncontrada">Parâmetro de saída que indica true caso haja registros lidos.</param>
    /// <returns>Uma tabela do Spectre.Console estilizada contendo as metas.</returns>
    public static Table MostrarMetas(SqlDataReader Reader, out bool MetaEncontrada)
    {
        MetaEncontrada = false;

        var tabela = new Table().Border(TableBorder.Rounded);
        tabela.BorderColor(Color.FromHex(Cores.Opcoes));
        tabela.AddColumn("Id");
        tabela.AddColumn("Titulo");
        tabela.AddColumn("Descrição");
        tabela.AddColumn("Meta em minútos");
        tabela.AddColumn("Minutos estudados");
        tabela.AddColumn("Criado em");
        tabela.AddColumn("Data limite");
        tabela.AddColumn("Concluído");
        tabela.AddColumn("Prioridade");
        tabela.AddColumn("Categoria");

        tabela.Columns[0].Centered(); // Id
        tabela.Columns[3].Centered(); // Meta
        tabela.Columns[4].Centered(); // Minutos estudados
        tabela.Columns[5].Centered(); // Criado em
        tabela.Columns[6].Centered(); // Data limite
        tabela.Columns[7].Centered(); // Concluído
        tabela.Columns[8].Centered(); // Prioridade
        tabela.Columns[9].Centered(); // Categoria

        while (Reader.Read())
        {
            MetaEncontrada = true;
            string data_limite;

            bool concluido = Convert.ToBoolean(Reader["concluido"]);
            string prioridade = Convert.ToString(Reader["prioridade"].ToString()!);
            string statusConcluido = concluido ? "[green]Concluída[/]" : "[yellow]Em andamento[/]";

            // Formatação do status de prazo de entrega
            if (Reader["data_limite"] == DBNull.Value)
            {
                data_limite = "Sem data limite";
            }
            else
            {
                DateTime data = Convert.ToDateTime(Reader["data_limite"]);
                if (data.Date < DateTime.Today)
                {
                    data_limite = $"{data:dd/MM/yyyy} - Atrasada";
                }
                else
                {
                    data_limite = data.ToString("dd/MM/yyyy");
                }
            }

            // Trunca strings longas para se adequarem à largura do terminal
            string descricao = Reader["descricao"].ToString()!;
            if (descricao.Length > 30)
            {
                descricao = descricao.Substring(0, 15) + "...";
            }

            string titulo = Reader["titulo"].ToString()!;
            if (titulo.Length > 18)
            {
                titulo = titulo.Substring(0, 15) + "...";
            }

            // Aplica marcações de cor no campo de prioridade
            if (prioridade == "Prioridade alta")
                prioridade = "[red]Prioridade alta[/]";
            else if (prioridade == "Prioridade média")
                prioridade = "[yellow]Prioridade média[/]";
            else if (prioridade == "Prioridade baixa")
                prioridade = "[green]Prioridade baixa[/]";
            else
                prioridade = "[grey]Sem prioridade[/]";

            string categoria = Categoria.NomeCategoria(Convert.ToInt32(Reader["id"]));

            tabela.AddRow(
                $"{Reader["id"]}",
                $"{Reader["titulo"]}",
                descricao,
                $"{Reader["meta_minutos"]}",
                $"{Reader["minutos_estudados"]}",
                $"{Reader["data_criacao"]}",
                data_limite,
                statusConcluido,
                prioridade,
                categoria
            );
            tabela.AddEmptyRow();
        }

        return tabela;
    }

    /// <summary>
    /// Busca no banco de dados e monta uma tabela com todas as metas já concluídas do cliente.
    /// </summary>
    /// <param name="id_cliente">O identificador do cliente.</param>
    /// <returns>Uma tabela formatada contendo o histórico de metas finalizadas.</returns>
    public static Table HistoricoMetasConcluídas(int id_cliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "SELECT * FROM Estudo WHERE id_cliente = @id AND concluido = 1";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id_cliente);
                using (var Reader = cmd.ExecuteReader())
                {
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SUAS METAS CONCLUÍDAS");

                    var tabela = new Table().Border(TableBorder.Rounded);
                    tabela.AddColumn("Id");
                    tabela.AddColumn("Titulo");
                    tabela.AddColumn("Descrição");
                    tabela.AddColumn("Meta em minútos");
                    tabela.AddColumn("Minutos estudados");
                    tabela.AddColumn("Criado em");
                    tabela.AddColumn("Concluído em");

                    tabela.Columns[0].Centered(); // Id
                    tabela.Columns[3].Centered(); // Meta em minutos
                    tabela.Columns[4].Centered(); // Minutos estudados
                    tabela.Columns[5].Centered(); // Criado em
                    tabela.Columns[6].Centered(); // Concluído em

                    while (Reader.Read())
                    {
                        string descricao = Reader["descricao"].ToString()!;
                        if (descricao.Length > 30)
                        {
                            descricao = descricao.Substring(0, 15) + "...";
                        }

                        string titulo = Reader["titulo"].ToString()!;
                        if (titulo.Length > 18)
                        {
                            titulo = titulo.Substring(0, 15) + "...";
                        }

                        string dataCriacao = Reader["data_criacao"] != DBNull.Value
                             ? Convert.ToDateTime(Reader["data_criacao"]).ToString("dd/MM/yyyy")
                             : "-";

                        string dataConclusao = Reader["data_conclusao"] != DBNull.Value
                            ? Convert.ToDateTime(Reader["data_conclusao"]).ToString("dd/MM/yyyy")
                            : "-";

                        tabela.AddRow(
                            $"{Reader["id"]}",
                            titulo,
                            descricao,
                            $"{Reader["meta_minutos"]}",
                            $"{Reader["minutos_estudados"]}",
                            dataCriacao,
                            dataConclusao
                        );
                        tabela.AddEmptyRow();
                    }

                    return tabela;
                }
            }
        }
    }

    /// <summary>
    /// Constrói um painel do Spectre.Console para exibição detalhada de uma meta específica.
    /// </summary>
    /// <param name="Reader">O leitor SQL posicionado na meta a ser exibida.</param>
    /// <returns>Um objeto de painel formatado contendo as informações da meta.</returns>
    public static Panel InformacoesMetas(SqlDataReader Reader)
    {
        string data_limite;

        if (Reader["data_limite"] == DBNull.Value)
        {
            data_limite = "Sem data limite";
        }
        else
        {
            DateTime data = Convert.ToDateTime(Reader["data_limite"]);
            if (data.Date < DateTime.Today)
            {
                data_limite = $"{data:dd/MM/yyyy} - Atrasada";
            }
            else
            {
                data_limite = data.ToString("dd/MM/yyyy");
            }
        }

        string titulo = Reader["titulo"].ToString()!;
        string descricao = Reader["descricao"].ToString()!;
        string metaMinutos = Reader["meta_minutos"].ToString()!;
        string minutosEstudados = Reader["minutos_estudados"].ToString()!;
        string prioridade = Convert.ToString(Reader["prioridade"].ToString()!);
        bool concluido = Convert.ToBoolean(Reader["concluido"]);

        string status = concluido ? "[green]Concluída[/]" : "[yellow]Em andamento[/]";

        if (prioridade == "Prioridade alta")
            prioridade = "[red]Prioridade alta[/]";
        else if (prioridade == "Prioridade média")
            prioridade = "[yellow]Prioridade média[/]";
        else if (prioridade == "Prioridade baixa")
            prioridade = "[green]Prioridade baixa[/]";
        else
            prioridade = "[grey]Sem prioridade[/]";

        string categoria = Categoria.NomeCategoria(Convert.ToInt32(Reader["id"]));

        string textoPainel =
            $"\n[bold]Descrição:[/] {descricao}\n\n" +
            $"[bold]Meta:[/] {metaMinutos} minutos\n" +
            $"[bold]Data limite: {data_limite}[/]\n" +
            $"[bold]Estudado:[/] {minutosEstudados} minutos\n" +
            $"[bold]Status:[/] {status}\n" +
            $"[bold]{prioridade}[/] \n" +
            $"[bold]Categoria: {categoria}[/] \n";

        var painelEstudante = new Panel(textoPainel)
            .Border(BoxBorder.Rounded)
            .BorderColor(Color.FromHex($"{Cores.Opcoes}"))
            .Header($"[{Cores.TextosDestaque}]{Reader["titulo"]}[/]", Justify.Left);

        return painelEstudante;
    }
}