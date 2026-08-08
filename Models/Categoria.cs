namespace Init_db;

using System.Formats.Asn1;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Spectre.Console;

public class Categoria
{
    public static int MostrarCategorias(int id_cliente)
    {


        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "SELECT * FROM Categoria WHERE id_cliente = @id_cliente";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SUAS CATEGORIAS CRIADAS");
                    AnsiConsole.MarkupLine($"\n{Textos.SelecionarCategoria}");
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
                        string titulo = reader["nome"].ToString()!;

                        estudos.Add(id, titulo);
                        menu.AddChoice(titulo);
                    }

                    string resposta = AnsiConsole.Prompt(menu);

                    if (resposta == "Sair")
                        return -1;

                    int idEscolhido = estudos
                        .First(x => x.Value == resposta)
                        .Key;

                    // Estudo.IniciarEstudo(id_cliente, idEscolhido);

                    return idEscolhido;
                }
            }
        }
    }
    public static int CriarCategoria(int id_cliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "INSERT INTO Categoria (nome, id_cliente) VALUES (@nome, @id_cliente)";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {

                // Remove parâmetros da tentativa anterior antes de reutilizar o mesmo comando SQL.
                cmd.Parameters.Clear();
                Interface.LimparTelaGeral();
                Interface.Titulo("CRIE UMA NOVA CATEGORIA");
                AnsiConsole.MarkupLine($"\n{Textos.Categoria}\n");

                AnsiConsole.MarkupLine("[#D3CCC7]─────────────────────────────────[/]\n");

                var nome = AnsiConsole.Ask<string>("\nCrie um título para a sua nova categoria: ");

                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);

                cmd.ExecuteScalar();
                Mensagens.CriarCategoria();


            }
            return -1;
        }
    }
    public static void VincularCategoria(int id_categoria, int id_estudo)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = @"
UPDATE Estudo
SET id_categoria = @id_categoria
WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {

                // Remove parâmetros da tentativa anterior antes de reutilizar o mesmo comando SQL.
                cmd.Parameters.Clear();
                Interface.LimparTelaGeral();


                cmd.Parameters.AddWithValue("@id_categoria", id_categoria);
                cmd.Parameters.AddWithValue("@id", id_estudo);

                cmd.ExecuteNonQuery();

            }
        }
    }
    public static string NomeCategoria(int idEstudo)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = @"
SELECT C.nome
FROM Estudo E
LEFT JOIN Categoria C
    ON E.id_categoria = C.id
WHERE E.id = @idEstudo";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idEstudo", idEstudo);

                object? resultado = cmd.ExecuteScalar();

                if (resultado == null || resultado == DBNull.Value)
                {
                    return "Sem categoria";
                }

                return resultado.ToString()!;
            }
        }
    }
    public static void EscolherMetaCategoria(int id_categoria, int id_cliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            bool MetaEncontrada = false;
            conn.Open();

            string sql = @"
SELECT *
FROM Estudo
WHERE id_categoria = @id_categoria";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_categoria", id_categoria);
                using (var Reader = cmd.ExecuteReader())

                {
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SEUS PLANOS DE ESTUDO");
                    try
                    {
                        var tabela = GerenciaMetas.MostrarMetas(Reader, out MetaEncontrada);

                        AnsiConsole.Write(tabela);
                        if (MetaEncontrada)
                        {

                            if (Mensagens.IniciarEstudo() == "Sim")
                            {
                                Estudo estudo = new Estudo();
                                estudo.EscolherEstudo(id_cliente);
                            }
                            else
                            {
                                Mensagens.Sair();
                            }
                        }
                        else
                        {
                            Mensagens.Erro_SemInformacoes();
                        }
                    }
                    catch
                    {
                        Mensagens.Erro_SemInformacoes();
                    }
                }
            }
        }
    }
    public static bool ApagarCategoria(int id_categoria)
{
    using (SqlConnection conn = new SqlConnection(Banco.Conexao))
    {
        Interface.LimparTelaGeral();
        conn.Open();

        Interface.Titulo("APAGUE SUA CATEGORIA");

        string resposta = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Deseja apagar a categoria? As metas vinculadas ficarão sem categoria.")
            .AddChoices("Deletar categoria", "Cancelar")
            .HighlightStyle(new Style(
                foreground: Color.FromHex($"{Cores.Opcoes}"),
                decoration: Decoration.Bold)));

        if (resposta == "Deletar categoria")
        {

            try
            {
              
                string sqlUpdate = "UPDATE Estudo SET id_categoria = NULL WHERE id_categoria = @id";
                using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn))
                {
                    cmdUpdate.Parameters.AddWithValue("@id", id_categoria);
                    cmdUpdate.ExecuteNonQuery();
                }

           
                string sqlDelete = "DELETE FROM Categoria WHERE id = @id";
                using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, conn))
                {
                    cmdDelete.Parameters.AddWithValue("@id", id_categoria);

                    int linhasAfetadas = cmdDelete.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_FinalizarApagarMeta("apagada");
                        return true;
                    }

                    Mensagens.Erro_PlanoNaoEncontrado(id_categoria);
                    return false;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                Console.ReadKey();
                return false;
            }
        }
        }

        Mensagens.Erro_Cancelada();
        return true;
    }
    public static bool RemoverMeta(int id_estudo)
{
    using (SqlConnection conn = new SqlConnection(Banco.Conexao))
    {
        Interface.LimparTelaGeral();
        conn.Open();

        Interface.Titulo("REMOVER META");

        string resposta = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Deseja remover a meta da categoria?")
            .AddChoices("Remover categoria", "Cancelar")
            .HighlightStyle(new Style(
                foreground: Color.FromHex($"{Cores.Opcoes}"),
                decoration: Decoration.Bold)));

        if (resposta == "Remover categoria")
        {

            try
            {
              
                string sqlUpdate = "UPDATE Estudo  SET id_categoria = NULL WHERE id = @id";
                using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn))
                {
                    cmdUpdate.Parameters.AddWithValue("@id", id_estudo);
                    cmdUpdate.ExecuteNonQuery();
                    Mensagens.RemoverMeta();
                    return false;
                }
    
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                Console.ReadKey();
                return false;
            }
        }
        }

        Mensagens.Erro_Cancelada();
        return true;
    }
     public static int AtualizarTitulo(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            string resposta = "";
            do
            {
                conn.Open();
                Interface.LimparTelaGeral();
                Interface.Titulo("ATUALIZE A SUA CATEGORIA");

                var titulo = AnsiConsole.Ask<string>("Digite o novo título da sua categoria: ");

                string sql = "UPDATE Categoria SET nome = @nome WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@nome", titulo);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.AtualizarNomeCatetoria();
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }

                }
            } while (resposta == "s");

            return -1;
        }
    }


}


