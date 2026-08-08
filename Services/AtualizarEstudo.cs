namespace Init_db;
using Spectre.Console;
using Microsoft.Data.SqlClient;

public class AtualizarEstudo
{
    // <summary>
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
            foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold))
            .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
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
    public static bool ApagarMeta(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            Interface.LimparTelaGeral();
            conn.Open();

            Interface.Titulo("ATUALIZE A SUA META");

            string resposta = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Deseja apagar a meta?")
                .AddChoices("Apagar meta", "Cancelar")
                .HighlightStyle(new Style(
                    foreground: Color.FromHex($"{Cores.Opcoes}"),
                    decoration: Decoration.Bold)));

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
                        return true;
                    }

                    Mensagens.Erro_PlanoNaoEncontrado(id);
                    return false;
                }
            }

            Mensagens.Erro_Cancelada();
            return false;
        }
    }


}