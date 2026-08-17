namespace Init_db;

using Microsoft.Data.SqlClient;
using Spectre.Console;
using System;
using System.Diagnostics;
using System.Threading;

/// <summary>
/// Gerencia o cronômetro de estudo e o registro do tempo
/// dedicado às metas do usuário.
/// </summary>

public class ResultadoSessao
{
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public TimeSpan TempoLiquido { get; set; }
    public TimeSpan TempoBruto { get; set; }
    public TimeSpan TempoPausa => TempoBruto - TempoLiquido;

    public double MinutosLiquidos => TempoLiquido.TotalMinutes;
    public double MinutosBrutos => TempoBruto.TotalMinutes;
    public double MinutosPausa => TempoPausa.TotalMinutes;
}

public class Cronometro
{
    /// <summary>
    /// Gerencia a sessão de estudo calculando o tempo líquido (foco) 
    /// e o tempo bruto (total decorrido incluindo pausas).
    /// </summary>
    /// <returns>Tempo líquido estudado em minutos.</returns>
    public static ResultadoSessao ContarTempo()
    {
        Stopwatch tempoBruto = Stopwatch.StartNew();
        Stopwatch tempoLiquido = new Stopwatch();
        DateTime dataInicio = DateTime.Now;
        Interface.LimparTelaGeral();
        Interface.Titulo("CRONOMETRO");
        bool emExecucao = true;

        while (emExecucao)
        {
            tempoLiquido.Start();
            Console.Clear();
            Interface.LimparTelaGeral();
            Interface.Titulo("CRONOMETRO");
            AnsiConsole.MarkupLine($"[{Cores.TextosDestaque}]Pressione QUALQUER TECLA para pausar...[/]\n");

            AnsiConsole.Status()
                .Spinner(Spinner.Known.TimeTravel)
                .SpinnerStyle(Style.Parse($"{Cores.TextosDestaque}"))
                .Start("Estudando...", ctx =>
                {
                    while (!Console.KeyAvailable)
                    {
                        ctx.Status(
                            $"[{Cores.TextosDestaque}]Tempo de foco: {tempoLiquido.Elapsed:hh\\:mm\\:ss}[/] | " +
                            $"[grey]Tempo total): {tempoBruto.Elapsed:hh\\:mm\\:ss}[/]"
                        );
                        Thread.Sleep(200);
                    }

                    Console.ReadKey(true);
                });

            tempoLiquido.Stop();
            Console.Clear();
            Interface.LimparTelaGeral();
            Interface.Titulo("CRONOMETRO");
            AnsiConsole.MarkupLine($"[{Cores.TextosDestaque}]CRONÔMETRO PAUSADO[/]");


            AnsiConsole.MarkupLine($"Foco acumulado: [bold]{tempoLiquido.Elapsed:hh\\:mm\\:ss}[/]\n");

            string opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[#D3CCC7]O que deseja fazer agora?[/]")
                    .HighlightStyle(new Style(
                        foreground: Color.FromHex($"{Cores.Opcoes}"),
                        decoration: Decoration.Bold
                    ))
                    .AddChoices("Continuar estudando", "Finalizar sessão"));

            if (opcao == "Finalizar sessão")
            {
                emExecucao = false;
            }
        }

        tempoBruto.Stop();
        DateTime dataFim = DateTime.Now;
        double minutosLiquidos = tempoLiquido.Elapsed.TotalMinutes;
        double minutosBrutos = tempoBruto.Elapsed.TotalMinutes;
        double tempoPausa = minutosBrutos - minutosLiquidos;



        AnsiConsole.MarkupLine($"[green]Sessão Finalizada com Sucesso![/]\n");
        AnsiConsole.MarkupLine($"[bold]Tempo Líquido (Foco):[/] {tempoLiquido.Elapsed:hh\\:mm\\:ss} min)");
        AnsiConsole.MarkupLine($"[bold]Tempo Bruto (Total):[/] {tempoBruto.Elapsed:hh\\:mm\\:ss} min)");
        AnsiConsole.MarkupLine($"[bold]Tempo de Pausa:[/] {tempoBruto.Elapsed - tempoLiquido.Elapsed:hh\\:mm\\:ss}min \n");

        return new ResultadoSessao
        {
            DataInicio = dataInicio,
            DataFim = dataFim,
            TempoLiquido = tempoLiquido.Elapsed,
            TempoBruto = tempoBruto.Elapsed,
        };
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

            }
            System.Console.WriteLine("Tempo salvo");

        }

    }
    public static void SalvarTempoSessao(int id_cliente, int id_estudo, int id_sessao, ResultadoSessao sessao)
    {

        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = @"
            UPDATE sessao_estudo
            SET 
                id_cliente = @id_cliente,
                id_meta = @id_meta,
                data_inicio = @data_inicio,
                data_fim = @data_fim,
                duracao_minutos = @duracao_minutos,
                tempo_estudado_minutos = @tempo_estudado_minutos,
                status = @status,
                descricao = @descricao,
                tempo_pausa_minutos = @tempo_pausa_minutos
            WHERE id = @id";

            var descricao = AnsiConsole.Ask<string>("Descreva o que foi estudado e realizado nessa sessão de estudo: ");
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {

                cmd.Parameters.AddWithValue("data_inicio", sessao.DataInicio);
                cmd.Parameters.AddWithValue("data_fim", sessao.DataFim);
                cmd.Parameters.AddWithValue("descricao", descricao);

                cmd.Parameters.AddWithValue("@duracao_minutos", Convert.ToInt32(sessao.MinutosBrutos));
                cmd.Parameters.AddWithValue("@tempo_estudado_minutos", Convert.ToInt32(sessao.MinutosLiquidos));
                cmd.Parameters.AddWithValue("@tempo_pausa_minutos", Convert.ToInt32(sessao.MinutosPausa));

                cmd.Parameters.AddWithValue("@status", "CONCLUIDO");

                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
                cmd.Parameters.AddWithValue("@id_meta", id_estudo);
                cmd.Parameters.AddWithValue("@id", id_sessao);
                cmd.ExecuteNonQuery();

            }
            System.Console.WriteLine("Tempo salvo");

        }
    }


}
