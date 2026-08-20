using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace Init_db;

/// <summary>
/// Gerencia a criação, exibição e consulta de informações relativas às sessões de estudo.
/// </summary>
public class Sessao_Estudo
{
    /// <summary>
    /// Cadastra uma nova sessão de estudo com status inicial "Em andamento" vinculada a uma meta.
    /// </summary>
    /// <param name="id_cliente">O identificador do cliente proprietário da sessão.</param>
    /// <param name="id_estudo">O identificador da meta de estudo associada.</param>
    /// <returns>Retorna -1 após a conclusão da criação da sessão.</returns>
    public static int CriarSessao(int id_cliente, int id_estudo)
    {
        // Inicializa a conexão com o banco de dados SQL Server.
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            // Consulta SQL para inserir uma nova sessão de estudo.
            string sql = "INSERT INTO sessao_estudo (titulo,id_cliente,id_meta,status) VALUES (@titulo,@id_cliente,@id_meta,@status)";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // Limpa e prepara a interface visual do console.
                Interface.LimparTelaGeral();
                Interface.Titulo("CRIE SUA SESSÃO DE ESTUDO");

                AnsiConsole.MarkupLine("[#D3CCC7]─────────────────────────────────[/]\n");

                // Coleta o título da sessão através da interação com o usuário.
                var titulo = AnsiConsole.Ask<string>("\nInforme o título para sua sessão: ");

                // Associa os parâmetros necessários à instrução SQL.
                cmd.Parameters.AddWithValue("@titulo", titulo);
                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
                cmd.Parameters.AddWithValue("@id_meta", id_estudo);
                cmd.Parameters.AddWithValue("@status", "Em andamento");

                // Executa a inclusão no banco de dados.
                cmd.ExecuteNonQuery();

                return -1;
            }
        }
    }

    /// <summary>
    /// Exibe um menu interativo com as sessões em andamento vinculadas a uma meta e retorna a selecionada.
    /// </summary>
    /// <param name="id_estudo">O identificador da meta de estudo pesquisada.</param>
    /// <returns>O identificador da sessão escolhida ou -1 caso o usuário decida sair.</returns>
    public static int MostrarSessao(int id_estudo)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            // Busca as sessões com status "Em andamento" associadas ao ID da meta.
            string sql = "SELECT * FROM sessao_estudo WHERE id_meta = @id_meta AND status = @status";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_meta", id_estudo);
                cmd.Parameters.AddWithValue("@status", "Em andamento");

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Prepara o terminal para exibir a lista de opções.
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SUAS SESSÕES CRIADAS");

                    // Dicionário para relacionar o ID da sessão com o seu respectivo título.
                    var estudos = new Dictionary<int, string>();

                    // Criação do menu seletivo no terminal via Spectre.Console.
                    var menu = new SelectionPrompt<string>()
                        .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
                        .HighlightStyle(new Style(
                            foreground: Color.FromHex($"{Cores.Opcoes}"),
                            decoration: Decoration.Bold));

                    // Opção padrão para permitir cancelamento/saída.
                    menu.AddChoice("Sair");

                    // Percorre os registros retornados populando o dicionário e o menu.
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string titulo = reader["titulo"].ToString()!;

                        estudos.Add(id, titulo);
                        menu.AddChoice(titulo);
                    }

                    // Exibe o menu na tela e captura a resposta do usuário.
                    string resposta = AnsiConsole.Prompt(menu);

                    // Retorna -1 se a escolha for sair.
                    if (resposta == "Sair")
                        return -1;

                    // Busca a chave (ID) associada ao título selecionado.
                    int idEscolhido = estudos
                        .First(x => x.Value == resposta)
                        .Key;

                    return idEscolhido;
                }
            }
        }
    }

    /// <summary>
    /// Recupera o nome da categoria vinculada à meta de uma sessão de estudo específica.
    /// </summary>
    /// <param name="id_sessao">O identificador da sessão pesquisada.</param>
    /// <returns>O nome da categoria vinculada ou "Sem sessão" caso nenhum registro seja encontrado.</returns>
    public static string NomeSessao(int id_sessao)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            // Consulta que junta Sessão, Estudo (Meta) e Categoria para extrair o nome da categoria.
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

                // Executa a consulta e recupera o valor resultante da primeira coluna.
                object? resultado = cmd.ExecuteScalar();

                // Valida se o retorno é nulo ou DBNull do SQL Server.
                if (resultado == null || resultado == DBNull.Value)
                {
                    return "Sem sessão";
                }

                return resultado.ToString()!;
            }
        }
    }
}