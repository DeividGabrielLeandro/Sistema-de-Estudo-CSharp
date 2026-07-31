using System.Reflection.Metadata.Ecma335;

namespace Init_db;

using Spectre.Console;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;

public class GerenciaMetas
{

    /// <summary>
    /// Atualiza o título de uma meta.
    /// </summary>
    public static int AtualizarTitulo(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            string resposta = "";
            do
            {
                conn.Open();
                Interface.LimparTelaGeral();
                Interface.Titulo("ATUALIZE A SUA META");

                var titulo = AnsiConsole.Ask<string>("Digite o novo título da sua meta: ");

                string sql = "UPDATE Estudo SET titulo = @titulo WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@titulo", titulo);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_AtualizarMeta("Titulo");
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


    /// <summary>
    /// Atualiza a descrição de uma meta.
    /// </summary>
    public static int AtualizarDescricao(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            string resposta = "";
            do
            {
                conn.Open();
                Interface.LimparTelaGeral();
                Interface.Titulo("ATUALIZE A SUA META");

                var descricao = AnsiConsole.Ask<string>("Digite a nova descrição da sua meta: ");
                string sql = "UPDATE Estudo SET descricao = @descricao WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@descricao", descricao);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_AtualizarMeta("Descrição");
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }


                }
            } while (resposta == "s");
        }

        return -1;
    }


    /// <summary>
    /// Atualiza a meta de minutos de estudo.
    /// </summary>
    public static int AtualizarMeta(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {

            conn.Open();
            string resposta = "";
            do
            {
                Interface.LimparTelaGeral();
                Interface.Titulo("ATUALIZE A SUA META");
                var meta_minutos = AnsiConsole.Ask<int>("Defina uma nova meta em minútos para a sua meta: ");

                string sql = "UPDATE Estudo SET meta_minutos = @meta_minutos WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@meta_minutos", meta_minutos);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_AtualizarMeta("Meta");
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }

                }
            } while (resposta == "s");
        }

        return -1;
    }


    /// <summary>
    /// Marca uma meta como concluída.
    /// </summary>
    public static void MarcarFinalizada(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            string resposta = "";
            conn.Open();
            Interface.LimparTelaGeral();
            Interface.Titulo("ATUALIZE A SUA META");


            resposta = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Deseja marcar a meta como concluída?")
            .AddChoices("Concluir meta", "Cancelar")
            .HighlightStyle(new Style(
            foreground: Color.FromHex("#EF0606"), decoration: Decoration.Bold))
            .HighlightStyle(new Style(foreground: Color.FromHex("#EF0606")))
            );
            if (resposta == "Concluir meta")
            {

                string sql = "UPDATE Estudo SET concluido = 1 WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@id", id);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_FinalizarApagarMeta("finalizada");
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }
                }

            }
            else
            {
                {
                    Mensagens.Erro_Cancelada();
                }
            }
        }
    }


    /// <summary>
    /// Remove uma meta do banco de dados após confirmação do usuário.
    /// </summary>
    public static void ApagarMeta(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            Interface.LimparTelaGeral();
            string resposta = "";
            conn.Open();
            Interface.Titulo("ATUALIZE A SUA META");
            resposta = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Deseja apagar a meta? ")
                        .AddChoices("Apagar meta", "Cancelar")
                        .HighlightStyle(new Style(
                        foreground: Color.FromHex("#EF0606"), decoration: Decoration.Bold))
                        .HighlightStyle(new Style(foreground: Color.FromHex("#EF0606")))
                        );
            
            if (resposta == "Apagar meta")
            {

                string sql = "DELETE FROM Estudo WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@id", id);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_FinalizarApagarMeta("apagada");
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }
                }

            }
            else
            {
                {
                    Mensagens.Erro_Cancelada();
                }
            }
        }
    }




    public static Table MostrarMetas(SqlDataReader Reader, out bool MetaEncontrada)
    {
        MetaEncontrada = false;
        var tabela = new Table();
        tabela.AddColumn("Id");
        tabela.AddColumn("Titulo");
        tabela.AddColumn("Descrição");
        tabela.AddColumn("Meta em minútos");
        tabela.AddColumn("Minutos estudados");
        tabela.AddColumn("Criado em");
        tabela.AddColumn("Concluído");

        while (Reader.Read())

        {
            MetaEncontrada = true;
            MetaEncontrada = true;
            bool concluido = Convert.ToBoolean(Reader["concluido"]);
            string statusConcluido = concluido ? "Sim" : "Não";

            string descricao = Reader["descricao"].ToString()!;
            if (descricao.Length > 30)
            {
                descricao = descricao.Substring(0, 27) + "...";
            }

            tabela.AddRow(
                $"{Reader["id"]}",
                $"{Reader["titulo"]}",
                descricao,
                $"{Reader["meta_minutos"]}",
                $"{Reader["minutos_estudados"]}",
                $"{Reader["data_criacao"]}",
                statusConcluido
            );


        }
        return tabela;
    }

    public static Panel InformacoesMetas(SqlDataReader Reader)
    {

        string titulo = Reader["titulo"].ToString()!;
        string descricao = Reader["descricao"].ToString()!;
        string metaMinutos = Reader["meta_minutos"].ToString()!;
        string minutosEstudados = Reader["minutos_estudados"].ToString()!;
        bool concluido = Convert.ToBoolean(Reader["concluido"]);

        string status = concluido ? "[green]Concluída[/]" : "[yellow]Em andamento[/]";

        string textoPainel =
                $"\n[bold]Descrição:[/] {descricao}\n\n" +
                $"[bold]Meta:[/] {metaMinutos} minutos\n" +
                $"[bold]Estudado:[/] {minutosEstudados} minutos\n" +
                $"[bold]Status:[/] {status}";

        var painelEstudante = new Panel(textoPainel)

.Border(BoxBorder.Rounded)
.BorderColor(Color.FromHex("#d02c2c"))
.Header($"[#EF0606]{Reader["titulo"]}[/]", Justify.Left);

        return painelEstudante;
    }
}