namespace Init_db;

using Microsoft.Data.SqlClient;

/// <summary>
/// Prover consultas para recuperação de dados estatísticos e cadastrais do cliente.
/// </summary>
public class InformacaoCliente
{

    /// <summary>
    /// Obtém o nome do usuário a partir do identificador informado.
    /// </summary>
    /// <param name="id">Identificador do usuário.</param>
    /// <returns>Nome completo do usuário ou "Usuário" caso não encontre.</returns>

    public string ObterNomeCliente(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = "SELECT nome_completo FROM Cliente WHERE id = @id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteScalar()?.ToString() ?? "Usuário";
            }
        }
    }


    /// <summary>
    /// Retorna o tempo total de estudo acumulado pelo usuário.
    /// </summary>
    /// <param name="idCliente">Identificador do usuário.</param>
    /// <returns>Total de minutos estudados.</returns>
    public static double MostrarTempoTotalEstudo(int idCliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = @"SELECT TotalMinutosEstudados
                       FROM Cliente
                       WHERE id = @id ";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", idCliente);
                object resultado = cmd.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    return Convert.ToDouble(resultado);
                }

                return Convert.ToDouble(resultado);
            }
        }
    }


    /// <summary>
    /// Retorna a quantidade de metas pendentes do usuário.
    /// </summary>
    /// <param name="idCliente">Identificador do usuário.</param>
    /// <returns>Quantidade total de metas com status em andamento.</returns>
    public static double ContarMetasPendentes(int idCliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = "SELECT COUNT(*) FROM Estudo WHERE id_cliente = @id_cliente AND concluido = 0";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_cliente", idCliente);
                object resultado = cmd.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    return Convert.ToDouble(resultado);
                }

                return 0;
            }
        }
    }


    /// <summary>
    /// Retorna a quantidade de metas concluídas do usuário.
    /// </summary>
    /// <param name="idCliente">Identificador do usuário.</param>
    /// <returns>Quantidade total de metas finalizadas.</returns>
    public static double ContarMetasConcluidas(int idCliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = "SELECT COUNT(*) FROM Estudo WHERE id_cliente = @id_cliente AND concluido = 1";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_cliente", idCliente);
                object resultado = cmd.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    return Convert.ToDouble(resultado);
                }

                return 0;
            }
        }
    }


    /// <summary>
    /// Retorna a quantidade total de metas cadastradas pelo usuário.
    /// </summary>
    /// <param name="idCliente">Identificador do usuário.</param>
    /// <returns>Quantidade total absoluta de metas criadas.</returns>
    public static double ContarTodasMetas(int idCliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = "SELECT COUNT(*) FROM Estudo WHERE id_cliente = @id_cliente";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_cliente", idCliente);
                object resultado = cmd.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    return Convert.ToDouble(resultado);
                }

                return 0;
            }
        }
    }

}