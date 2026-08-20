namespace Init_db;

using Spectre.Console;
using Microsoft.Data.SqlClient;

/// <summary>
/// Prover métodos para atualização e exclusão de dados de uma meta no banco de dados.
/// </summary>
public class AtualizarEstudo
{
    // <summary>
    // Atualiza o título de uma meta de estudo específica.
    // </summary>
    // <param name="id">O identificador da meta.</param>
    // <returns>Retorna -1 após a execução da operação.</returns>
    public static int AtualizarTitulo(int id)
    {
        string resposta = "s";
        do
        {
            using (SqlConnection conn = new SqlConnection(Banco.Conexao))
            {
                conn.Open();

                Interface.LimparTelaGeral();
                Interface.Titulo("ATUALIZE A SUA META");

                // Solicita o novo título ao usuário
                var titulo = AnsiConsole.Ask<string>("Digite o novo título da sua meta: ");

                string sql = "UPDATE Estudo SET titulo = @titulo WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@titulo", titulo);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    // Notifica o usuário sobre o resultado da operação
                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_AtualizarMeta("Titulo");
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }
                }
            }

        } while (resposta == "s");

        return -1;
    }

    /// <summary>
    /// Atualiza a descrição de uma meta de estudo específica.
    /// </summary>
    /// <param name="id">O identificador da meta.</param>
    /// <returns>Retorna -1 após a execução da operação.</returns>
    public static int AtualizarDescricao(int id)
    {
        string resposta = "s";
        do
        {
            using (SqlConnection conn = new SqlConnection(Banco.Conexao))
            {
                conn.Open();

                Interface.LimparTelaGeral();
                Interface.Titulo("ATUALIZE A SUA META");

                // Coleta a nova descrição
                var descricao = AnsiConsole.Ask<string>("Digite a nova descrição da sua meta: ");
                string sql = "UPDATE Estudo SET descricao = @descricao WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@descricao", descricao);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    // Notifica o resultado
                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_AtualizarMeta("Descrição");
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }
                }
            }

        } while (resposta == "s");

        return -1;
    }

    /// <summary>
    /// Atualiza a meta de tempo em minutos para a meta selecionada.
    /// </summary>
    /// <param name="id">O identificador da meta.</param>
    /// <returns>Retorna -1 após a execução da operação.</returns>
    public static int AtualizarMeta(int id)
    {
        string resposta = "s";
        do
        {
            using (SqlConnection conn = new SqlConnection(Banco.Conexao))
            {
                conn.Open();

                Interface.LimparTelaGeral();
                Interface.Titulo("ATUALIZE A SUA META");

                // Solicita o tempo desejado em minutos
                var meta_minutos = AnsiConsole.Ask<int>("Defina uma nova meta em minutos para a sua meta: ");

                string sql = "UPDATE Estudo SET meta_minutos = @meta_minutos WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@meta_minutos", meta_minutos);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    // Notifica o resultado
                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_AtualizarMeta("Meta");
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }
                }
            }

        } while (resposta == "s");

        return -1;
    }

    /// <summary>
    /// Marca uma meta como concluída e grava a data atual de conclusão.
    /// </summary>
    /// <param name="id">O identificador da meta.</param>
    public static void MarcarFinalizada(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            Interface.LimparTelaGeral();
            Interface.Titulo("ATUALIZE A SUA META");

            // Pergunta de confirmação de conclusão
            string resposta = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Deseja marcar a meta como concluída?")
                    .AddChoices("Concluir meta", "Cancelar")
                    .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold))
            );

            if (resposta == "Concluir meta")
            {
                string sql = "UPDATE Estudo SET concluido = 1, data_conclusao = @data_conclusao WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@data_conclusao", DateTime.Now);

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
                Mensagens.Erro_Cancelada();
            }
        }
    }

    /// <summary>
    /// Remove permanentemente uma meta do banco de dados após confirmação.
    /// </summary>
    /// <param name="id">O identificador da meta.</param>
    /// <returns>True se a meta foi apagada com sucesso; caso contrário, False.</returns>
    public static bool ApagarMeta(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            Interface.LimparTelaGeral();
            Interface.Titulo("ATUALIZE A SUA META");

            // Exibe o prompt de confirmação de exclusão
            string resposta = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Deseja apagar a meta?")
                    .AddChoices("Apagar meta", "Cancelar")
                    .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold)));

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
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                        return false;
                    }
                }
            }
            else
            {
                Mensagens.Erro_Cancelada();
                return false;
            }
        }
    }
}