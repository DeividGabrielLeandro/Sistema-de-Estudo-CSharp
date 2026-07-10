namespace Init_db;

using Microsoft.Data.SqlClient;

using System;
using System.Diagnostics;
using System.Threading;

public class Cronometro
{
    public static double ContarTempo()
    {

       Console.Clear();
        System.Console.WriteLine("\n===================================================================");
        Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine("                  === CRONOMETRO DE ESTUDO ===        ");
        Console.ResetColor();
        System.Console.WriteLine("===================================================================\n");

        System.Console.WriteLine(" \"A educação é a arma mais poderosa que você pode usar para mudar o mundo.\" - Nelson Mandela\n");

        System.Console.WriteLine("\n===================================================================\n");


        Console.WriteLine("Pressione qualquer tecla para parar o contador...");

        Stopwatch cronometro = new Stopwatch();
        cronometro.Start();

        while (Console.KeyAvailable)
        {
            Console.ReadKey(true);
        }

        while (!Console.KeyAvailable)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            string tempoDecorrido = cronometro.Elapsed.ToString(@"hh\:mm\:ss");
            Console.Write($"\rTempo:        {tempoDecorrido}");
            Console.ResetColor();


            Thread.Sleep(1000);
        }
        Console.ReadKey(true);
        cronometro.Stop();

        while (Console.KeyAvailable)
        {
            Console.ReadKey(true);
        }

        double minutos = cronometro.Elapsed.TotalMinutes;
        System.Console.WriteLine(minutos);

        Console.WriteLine(";\nContador parado.");
        return minutos;
    }

    public static void SalvarTempo(int id_estudo, double minutos)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "UPDATE Estudo SET minutos_estudados = minutos_estudados + @minutos WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {

                cmd.Parameters.AddWithValue("@minutos", minutos);
                cmd.Parameters.AddWithValue("@id", id_estudo);
                cmd.ExecuteNonQuery();

            }
            System.Console.WriteLine("Tempo salvo");

        }
    }
    public static void AtualizarTempoTotalCliente(int id_cliente, double minutos)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql =@"
            UPDATE Cliente
            SET TotalMinutosEstudados = TotalMinutosEstudados + @minutos
            WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {

                cmd.Parameters.AddWithValue("@minutos", minutos);
                cmd.Parameters.AddWithValue("@id", id_cliente);
                cmd.ExecuteNonQuery();

            }
            System.Console.WriteLine("Tempo salvo");

        }
    }
}