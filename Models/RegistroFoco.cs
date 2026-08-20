using Microsoft.Data.SqlClient;

namespace Init_db;

/// <summary>
/// Prover métodos para registrar e consultar sessões e históricos de foco do cliente.
/// </summary>
public class RegistroFoco
{
    /// <summary>
    /// Salva um novo registro de foco no histórico e incrementa o total acumulado do cliente.
    /// </summary>
    /// <param name="idCliente">O identificador do cliente.</param>
    /// <param name="minutos">O tempo gasto em minutos durante a sessão de foco.</param>
    /// <param name="origem">A origem da atividade ("ESTUDO_LIVRE", "META" ou "SESSAO").</param>
    public static void SalvarFoco(int idCliente, double minutos, string origem)
    {
        // Arredonda o valor de minutos informados para um número inteiro.
        int tempoEmMinutos = (int)Math.Round(minutos);
        // Evita inserções de tempos inválidos ou zerados.
        if (tempoEmMinutos <= 0) return;

        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            // Comando SQL duplo: grava o registro do histórico e atualiza o saldo total do cliente.
            string sql = @"
                INSERT INTO registro_foco (id_cliente, tempo_foco, origem, data)
                VALUES (@id_cliente, @tempo_foco, @origem, CAST(GETDATE() AS DATE));

                UPDATE Cliente 
                SET TotalMinutosEstudados = ISNULL(TotalMinutosEstudados, 0) + @tempo_foco
                WHERE id = @id_cliente;";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_cliente", idCliente);
                cmd.Parameters.AddWithValue("@tempo_foco", tempoEmMinutos);
                cmd.Parameters.AddWithValue("@origem", origem); // "ESTUDO_LIVRE", "META" ou "SESSAO"

                cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Consulta o total de minutos acumulados em foco pelo cliente no dia de hoje.
    /// </summary>
    /// <param name="id_cliente">O identificador do cliente.</param>
    /// <returns>O somatório de minutos estudados hoje.</returns>
    public static int TempoFocoHoje(int id_cliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            // Soma os tempos de foco onde a data é estritamente a data atual.
            string sql = @"
            SELECT ISNULL(SUM(tempo_foco), 0)
            FROM registro_foco
            WHERE id_cliente = @id_cliente
              AND data = @data_hoje;";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
                cmd.Parameters.AddWithValue("@data_hoje", DateTime.Today); 

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }

    /// <summary>
    /// Consulta o total de minutos acumulados pelo cliente nos últimos 7 dias.
    /// </summary>
    /// <param name="id_cliente">O identificador do cliente.</param>
    /// <returns>O somatório dos minutos de foco acumulados na semana.</returns>
    public static int TempoFocoSemana(int id_cliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            // Seleciona e soma os registros compreendidos do intervalo de hoje até 6 dias atrás.
            string sql = @"
            SELECT ISNULL(SUM(tempo_foco), 0)
            FROM registro_foco
            WHERE id_cliente = @id_cliente
              AND data >= DATEADD(DAY, -6, @data_hoje)
              AND data <= @data_hoje;";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
                cmd.Parameters.AddWithValue("@data_hoje", DateTime.Today); 

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }

    /// <summary>
    /// Consulta o total de minutos acumulados pelo cliente dentro do mês corrente.
    /// </summary>
    /// <param name="id_cliente">O identificador do cliente.</param>
    /// <returns>O somatório do tempo de foco acumulado no mês atual.</returns>
    public static int TempoFocoMes(int id_cliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            // Soma os registros cujo mês e ano coincidem com o mês e ano atuais.
            string sql = @"
            SELECT ISNULL(SUM(tempo_foco), 0)
            FROM registro_foco
            WHERE id_cliente = @id_cliente
              AND MONTH(data) = MONTH(@data_hoje)
              AND YEAR(data) = YEAR(@data_hoje);";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
                cmd.Parameters.AddWithValue("@data_hoje", DateTime.Today);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}