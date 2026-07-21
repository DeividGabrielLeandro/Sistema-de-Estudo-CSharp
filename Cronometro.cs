namespace Init_db;

using Microsoft.Data.SqlClient;

using System;
using System.Diagnostics;
using System.Threading;

/// <summary>
/// Gerencia o cronômetro de estudo e o registro do tempo
/// dedicado às metas do usuário.
/// </summary>
public class Cronometro
{

    /// <summary>
    /// Inicia o cronômetro e retorna o tempo total de estudo em minutos.
    /// A contagem é encerrada quando o usuário pressiona uma tecla.
    /// </summary>
    /// <returns>Total de minutos estudados.</returns>
    public static double ContarTempo()
    {
        Interface.LimparTelaGeral();
        Interface.EscreverCentralizado("ATHENA - Cronometro ");


        System.Console.WriteLine(Textos.MensagemMotivacional_NelsonMandela);

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



    /// <summary>
    /// Salva o tempo estudado na meta informada.
    /// </summary>
    /// <param name="id_estudo">Identificador da meta.</param>
    /// <param name="minutos">Quantidade de minutos estudados.</param>
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


    /// <summary>
    /// Atualiza o tempo total de estudo acumulado pelo usuário.
    /// </summary>
    /// <param name="id_cliente">Identificador do usuário.</param>
    /// <param name="minutos">Quantidade de minutos a ser adicionada ao total.</param>
    public static void AtualizarTempoTotalCliente(int id_cliente, double minutos)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = @"
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