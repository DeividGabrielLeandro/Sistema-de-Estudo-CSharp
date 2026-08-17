
using Microsoft.Data.SqlClient;


namespace Init_db;


public class RegistroFoco
{

    public static void SalvarFoco(int idCliente, double minutos, string origem)
    {
        int tempoEmMinutos = (int)Math.Round(minutos);
        if (tempoEmMinutos <= 0) return;

        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

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


    public static int TempoFocoHoje(int id_cliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

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
    public static int TempoFocoSemana(int id_cliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

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
    public static int TempoFocoMes(int id_cliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

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