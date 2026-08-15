using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace Init_db;

public class Sessao_Estudo
{
    public static int CriarSessao(int id_cliente, int id_estudo)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "INSERT INTO sessao_estudo (titulo,id_cliente,id_meta,status) VALUES (@titulo,@id_cliente,@id_meta,@status)";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                Interface.LimparTelaGeral();
                Interface.Titulo("CRIE SUA SESSÃO DE ESTUDO");

                // AnsiConsole.MarkupLine($"\n{Textos.MensagemMeta}\n");
                AnsiConsole.MarkupLine("[#D3CCC7]─────────────────────────────────[/]\n");

                var titulo = AnsiConsole.Ask<string>("\nInforme o título para sua sessão: ");

                cmd.Parameters.AddWithValue("@titulo", titulo);
                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
                cmd.Parameters.AddWithValue("@id_meta", id_estudo);
                cmd.Parameters.AddWithValue("@status", "Em andamento");

                cmd.ExecuteNonQuery();

                return -1;
            }
        }
    }
    public static int MostrarSessao(int id_estudo)
    {

        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "SELECT * FROM sessao_estudo WHERE id_meta = @id_meta AND status = @status";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_meta", id_estudo);
                cmd.Parameters.AddWithValue("@status", "Em andamento");

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SUAS SESSÕES CRIADAS");
                    // AnsiConsole.MarkupLine($"\n{Textos.SelecionarCategoria}");
                    var estudos = new Dictionary<int, string>();

                    var menu = new SelectionPrompt<string>()
                        .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
                        .HighlightStyle(new Style(
                            foreground: Color.FromHex($"{Cores.Opcoes}"),
                            decoration: Decoration.Bold));

                    menu.AddChoice("Sair");

                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string titulo = reader["titulo"].ToString()!;

                        estudos.Add(id, titulo);
                        menu.AddChoice(titulo);
                    }

                    string resposta = AnsiConsole.Prompt(menu);

                    if (resposta == "Sair")
                        return -1;

                    int idEscolhido = estudos
                        .First(x => x.Value == resposta)
                        .Key;


                    return idEscolhido;
                }
            }
        }
    }
    public static string NomeSessao(int id_sessao)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = @"
SELECT C.nome
FROM sessao_estudo S
LEFT JOIN Estudo E 
    ON S.id_meta = E.id
LEFT JOIN Categoria C 
    ON E.id_categoria = C.id
WHERE S.id = @id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id_sessao);

                object? resultado = cmd.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    return "Sem sessão";
                }

                return resultado.ToString()!;
            }
        }
    }
}
