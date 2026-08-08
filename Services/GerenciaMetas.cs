using System.Reflection.Metadata.Ecma335;

namespace Init_db;

using Spectre.Console;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

public class GerenciaMetas
{

    /// <summary>
    /// Exibe os detalhes da meta selecionada e permite iniciar,
    /// editar ou concluir a sessão de estudo.
    /// </summary>
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
                        string opcao = "";
                        Interface.LimparTelaGeral();
                        Interface.Titulo("SEU PLANO");

                        AnsiConsole.Write($"\n{Textos.MensagemMotivacional_Seneca}\n\n");

                        if (Reader.Read())
                        {
                            AnsiConsole.Write(GerenciaMetas.InformacoesMetas(Reader));

                            opcao = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
                                .HighlightStyle(new Style(
                                    foreground: Color.FromHex($"{Cores.Opcoes}"),
                                    decoration: Decoration.Bold
                                ))
                                .AddChoices("Começar a contar o tempo", "Abrir as opções", "Marcar como finalizada", "Definir prioridade", "Definir data limite", "Vincular à uma categoria criada", "Sair"));

                            switch (opcao)
                            {
                                case "Começar a contar o tempo":
                                    double minutos = Cronometro.ContarTempo();
                                    Cronometro.SalvarTempo(id_estudo, minutos);
                                    Cronometro.AtualizarTempoTotalCliente(id_cliente, minutos);
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
    /// Exibe o menu de personalização de uma meta de estudo.
    /// </summary>
    /// <param name="id_estudo">Identificador da meta selecionada.</param>
    public static void PersonalizarMetas(int id_estudo)
    {
        string opcao = "";
        bool sair = false;
        while (!sair)
        {

            Interface.LimparTelaGeral();
            Interface.Titulo("PERSONALIZE SUAS METAS");

            opcao = AnsiConsole.Prompt(
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
                    {
                        return;
                    }
                    break;
                case "Sair":
                    sair = true;
                    break;
            }
        }
    }


    public static Table MostrarMetas(SqlDataReader Reader, out bool MetaEncontrada)
    {
        MetaEncontrada = false;

        var tabela = new Table()
.Border(TableBorder.Rounded);
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
            string data_limite;
            MetaEncontrada = true;

            bool concluido = Convert.ToBoolean(Reader["concluido"]);
            string prioridade = Convert.ToString(Reader["prioridade"].ToString()!);
            string statusConcluido = concluido ? "[green]Concluída[/]" : "[yellow]Em andamento[/]";


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


            if (prioridade == "Prioridade alta")
                prioridade = "[red]Prioridade alta[/]";
            else if (prioridade == "Prioridade média")
                prioridade = "[yellow]Prioridade média[/]";
            else if (prioridade == "Prioridade baixa")
                prioridade = "[green]Prioridade baixa[/]";
            else
                prioridade = "[grey]Sem prioridade[/]";

            string categoria = Categoria.NomeCategoria(
    Convert.ToInt32(Reader["id"])
);


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

        string categoria = Categoria.NomeCategoria(
Convert.ToInt32(Reader["id"])
);

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