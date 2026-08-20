using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace Init_db;

/// <summary>
/// Gerencia a exibição e execução de sessões individuais de estudo e o histórico associado.
/// </summary>
public class GerenciaSessao_Estudo
{
    /// <summary>
    /// Exibe a interface interativa de uma sessão de estudo específica e inicia a contagem de tempo.
    /// </summary>
    /// <param name="id_estudo">O identificador da meta associada.</param>
    /// <param name="id_sessao">O identificador único da sessão de estudo.</param>
    /// <param name="id_cliente">O identificador do cliente.</param>
    public static void Interface_Sessao(int id_estudo, int id_sessao, int id_cliente)
    {
        bool Sair = false;
        while (!Sair)
        {
            using (SqlConnection conn = new SqlConnection(Banco.Conexao))
            {
                conn.Open();
                string sql = "SELECT * FROM sessao_estudo WHERE id = @id AND id_meta = @id_meta";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id_sessao);
                    cmd.Parameters.AddWithValue("@id_meta", id_estudo);

                    using (var Reader = cmd.ExecuteReader())
                    {
                        string opcao = "";
                        Interface.LimparTelaGeral();
                        Interface.Titulo($"SUA SESSÃO DE ESTUDO - {Sessao_Estudo.NomeSessao(id_sessao)}");

                        AnsiConsole.Write($"{Textos.SobreSessao}\n");

                        if (Reader.Read())
                        {

                            opcao = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
                                .HighlightStyle(new Style(
                                    foreground: Color.FromHex($"{Cores.Opcoes}"),
                                    decoration: Decoration.Bold
                                ))
                                .AddChoices("Começar a contar o tempo", "Sair"));

                            switch (opcao)
                            {
                                case "Começar a contar o tempo":
                                    ResultadoSessao sessao = Cronometro.ContarTempo();
                                    Cronometro.SalvarTempoSessao(id_cliente, id_estudo, id_sessao, sessao);
                                    Cronometro.SalvarTempo(id_estudo, sessao.MinutosLiquidos);
                                    Cronometro.AtualizarTempoTotalCliente(id_cliente, sessao.MinutosLiquidos);
                                    RegistroFoco.SalvarFoco(id_cliente, sessao.MinutosLiquidos, "SESSAO");

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
    /// Busca no banco de dados e gera uma tabela com o histórico de sessões concluídas de uma meta.
    /// </summary>
    /// <param name="id_meta">O identificador da meta cujas sessões serão consultadas.</param>
    /// <returns>Uma tabela formatada do Spectre.Console contendo os registros de sessões finalizadas.</returns>
    public static Table HistoricoSessoes(int id_meta)
    {

        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "SELECT * FROM sessao_estudo WHERE id_meta = @id_meta AND status = @status";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_meta", id_meta);
                cmd.Parameters.AddWithValue("@status", "CONCLUIDO");
                using (var Reader = cmd.ExecuteReader())
                {
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SEU HISTÓRICO DE SESSÕES");

                    var tabela = new Table()

            .Border(TableBorder.Rounded);
                    tabela.AddColumn("Id");
                    tabela.AddColumn("Titulo");
                    tabela.AddColumn("Descrição");
                    tabela.AddColumn("Tempo total estudado(bruto)");
                    tabela.AddColumn("Tempo de foco");
                    tabela.AddColumn("Tempo em pausa");
                    tabela.AddColumn("Iniciado em");
                    tabela.AddColumn("Concluído em");

                    tabela.Columns[0].Centered(); // Id
                    tabela.Columns[2].Centered(); // Tempo total estudado(bruto)
                    tabela.Columns[3].Centered(); // Descrição
                    tabela.Columns[4].Centered(); // Tempo de foco
                    tabela.Columns[5].Centered(); // Tempo em pausa
                    tabela.Columns[6].Centered(); // Iniciado em
                    tabela.Columns[7].Centered(); // Concluído em


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

                        string data_inicio = Reader["data_inicio"] != DBNull.Value
                             ? Convert.ToDateTime(Reader["data_inicio"]).ToString("dd/MM/yyyy")
                             : "-";

                        string data_fim = Reader["data_fim"] != DBNull.Value
                            ? Convert.ToDateTime(Reader["data_fim"]).ToString("dd/MM/yyyy")
                            : "-";


                        tabela.AddRow(
                $"{Reader["id"]}",
                titulo,
                descricao,
                $"{Reader["duracao_minutos"]} minutos",
                $"{Reader["tempo_estudado_minutos"]} minutos",
                $"{Reader["tempo_pausa_minutos"]} minutos",
                data_inicio,
                data_fim

            );
                        tabela.AddEmptyRow();


                    }


                    return tabela;
                }

            }
        }
    }
}