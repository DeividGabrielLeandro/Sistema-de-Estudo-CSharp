namespace Init_db;

using Microsoft.Data.SqlClient;
using Spectre.Console;
using System;
using System.Diagnostics;
using System.Threading;

/// <summary>
/// Modelo DTO que armazena as métricas de tempo calculadas ao fim de um ciclo no cronômetro.
/// </summary>
public class ResultadoSessao
{
    /// <summary>Data e hora de início da sessão de foco.</summary>
    public DateTime DataInicio { get; set; }

    /// <summary>Data e hora de término da sessão de foco.</summary>
    public DateTime DataFim { get; set; }

    /// <summary>Tempo total em que o usuário permaneceu focado.</summary>
    public TimeSpan TempoLiquido { get; set; }

    /// <summary>Tempo total absoluto decorrido (incluindo as pausas).</summary>
    public TimeSpan TempoBruto { get; set; }

    /// <summary>Tempo total consumido em pausas.</summary>
    public TimeSpan TempoPausa => TempoBruto - TempoLiquido;

    /// <summary>Total de minutos líquidos acumulados de foco.</summary>
    public double MinutosLiquidos => TempoLiquido.TotalMinutes;

    /// <summary>Total de minutos brutos decorridos na sessão.</summary>
    public double MinutosBrutos => TempoBruto.TotalMinutes;

    /// <summary>Total de minutos acumulados em pausa.</summary>
    public double MinutosPausa => TempoPausa.TotalMinutes;
}

/// <summary>
/// Gerencia a contagem de tempo em tempo real e o envio dos dados às tabelas correspondentes.
/// </summary>
public class Cronometro
{
    /// <summary>
    /// Executa o loop do cronômetro interativo com controle de pausa/continuação no console.
    /// </summary>
    /// <returns>Objeto <see cref="ResultadoSessao"/> contendo todos os dados e métricas calculados.</returns>
    public static ResultadoSessao ContarTempo()
    {
        Stopwatch tempoBruto = Stopwatch.StartNew();
        Stopwatch tempoLiquido = new Stopwatch();
        DateTime dataInicio = DateTime.Now;

        Interface.LimparTelaGeral();
        Interface.Titulo("CRONOMETRO");
        bool emExecucao = true;

        // Loop principal do cronômetro
        while (emExecucao)
        {
            tempoLiquido.Start();
            Console.Clear();
            Interface.LimparTelaGeral();
            Interface.Titulo("CRONOMETRO");
            AnsiConsole.MarkupLine($"[{Cores.TextosDestaque}]Pressione QUALQUER TECLA para pausar...[/]\n");

            // Exibe a animação do status com cronômetro em tempo real
            AnsiConsole.Status()
                .Spinner(Spinner.Known.TimeTravel)
                .SpinnerStyle(Style.Parse($"{Cores.TextosDestaque}"))
                .Start("Estudando...", ctx =>
                {
                    while (!Console.KeyAvailable)
                    {
                        ctx.Status($"[{Cores.TextosDestaque}]Tempo de foco: {tempoLiquido.Elapsed:hh\\:mm\\:ss}[/] | [grey]Tempo total: {tempoBruto.Elapsed:hh\\:mm\\:ss}[/]");
                        Thread.Sleep(200);
                    }

                    // Limpa a tecla pressionada da fila do console
                    Console.ReadKey(true);
                });

            // Pausa o tempo de foco quando o usuário interage
            tempoLiquido.Stop();
            Console.Clear();
            Interface.LimparTelaGeral();
            Interface.Titulo("CRONOMETRO");
            AnsiConsole.MarkupLine($"[{Cores.TextosDestaque}]CRONÔMETRO PAUSADO[/]");
            AnsiConsole.MarkupLine($"Foco acumulado: [bold]{tempoLiquido.Elapsed:hh\\:mm\\:ss}[/]\n");

            // Questiona se deve retomar ou encerra a sessão
            string opcao = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[#D3CCC7]O que deseja fazer agora?[/]")
                    .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold))
                    .AddChoices("Continuar estudando", "Finalizar sessão"));

            if (opcao == "Finalizar sessão")
            {
                emExecucao = false;
            }
        }

        tempoBruto.Stop();
        DateTime dataFim = DateTime.Now;

        // Exibe o resumo no console
        AnsiConsole.MarkupLine($"[green]Sessão Finalizada com Sucesso![/]\n");
        AnsiConsole.MarkupLine($"[bold]Tempo Líquido (Foco):[/] {tempoLiquido.Elapsed:hh\\:mm\\:ss}");
        AnsiConsole.MarkupLine($"[bold]Tempo Bruto (Total):[/] {tempoBruto.Elapsed:hh\\:mm\\:ss}");
        AnsiConsole.MarkupLine($"[bold]Tempo de Pausa:[/] {tempoBruto.Elapsed - tempoLiquido.Elapsed:hh\\:mm\\:ss}\n");

        return new ResultadoSessao
        {
            DataInicio = dataInicio,
            DataFim = dataFim,
            TempoLiquido = tempoLiquido.Elapsed,
            TempoBruto = tempoBruto.Elapsed,
        };
    }

    /// <summary>
    /// Adiciona os minutos de foco acumulados ao total de minutos estudados da meta.
    /// </summary>
    /// <param name="id_estudo">O identificador da meta.</param>
    /// <param name="minutos">Quantidade de minutos líquidos a adicionar.</param>
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

            Console.WriteLine("Tempo salvo");
        }
    }

    /// <summary>
    /// Adiciona a contagem de minutos de estudo ao histórico geral acumulado do cliente.
    /// </summary>
    /// <param name="id_cliente">O identificador do cliente.</param>
    /// <param name="minutos">Quantidade de minutos líquidos a adicionar.</param>
    public static void AtualizarTempoTotalCliente(int id_cliente, double minutos)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "UPDATE Cliente SET TotalMinutosEstudados = TotalMinutosEstudados + @minutos WHERE id = @id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@minutos", minutos);
                cmd.Parameters.AddWithValue("@id", id_cliente);
            }

            Console.WriteLine("Tempo salvo");
        }
    }

    /// <summary>
    /// Atualiza os dados detalhados de uma sessão de estudo específica já registrada.
    /// </summary>
    /// <param name="id_cliente">O identificador do cliente.</param>
    /// <param name="id_estudo">O identificador da meta associada.</param>
    /// <param name="id_sessao">O identificador da sessão de estudo a atualizar.</param>
    /// <param name="sessao">Instância de <see cref="ResultadoSessao"/> contendo os dados do cronômetro.</param>
    public static void SalvarTempoSessao(int id_cliente, int id_estudo, int id_sessao, ResultadoSessao sessao)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            // Pede ao usuário que faça o relato dos assuntos abordados no bloco
            var descricao = AnsiConsole.Ask<string>("Descreva o que foi estudado e realizado nessa sessão de estudo: ");

            string sql = "UPDATE sessao_estudo SET id_cliente = @id_cliente, id_meta = @id_meta, data_inicio = @data_inicio, data_fim = @data_fim, duracao_minutos = @duracao_minutos, tempo_estudado_minutos = @tempo_estudado, status = @status, descricao = @descricao, tempo_pausa_minutos = @tempo_pausa WHERE id = @id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@data_inicio", sessao.DataInicio);
                cmd.Parameters.AddWithValue("@data_fim", sessao.DataFim);
                cmd.Parameters.AddWithValue("@descricao", descricao);

                cmd.Parameters.AddWithValue("@duracao_minutos", Convert.ToInt32(sessao.MinutosBrutos));
                cmd.Parameters.AddWithValue("@tempo_estudado", Convert.ToInt32(sessao.MinutosLiquidos));
                cmd.Parameters.AddWithValue("@tempo_pausa", Convert.ToInt32(sessao.MinutosPausa));

                cmd.Parameters.AddWithValue("@status", "CONCLUIDO");

                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
                cmd.Parameters.AddWithValue("@id_meta", id_estudo);
                cmd.Parameters.AddWithValue("@id", id_sessao);

                cmd.ExecuteNonQuery();
            }

            Console.WriteLine("Tempo salvo");
        }
    }
}