using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Data.SqlClient;
using Spectre.Console;

namespace Init_db;

/// <summary>
/// Gerencia o cadastro, consulta, atualização e acompanhamento
/// das metas de estudo do usuário.
/// </summary>
public class Estudo
{

    public enum TipoFiltroEstudo
    {
        Todas,
        Pendentes,
        Concluidas,
        UltimasCriadas,
        PorTitulo,
        PorTempoEstudado,
        Pesquisa
    }


    /// <summary>
    /// Cadastra uma nova meta de estudo vinculada ao usuário informado.
    /// </summary>
    public int CadastrarMeta(int id_gerado)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "INSERT INTO Estudo(titulo, descricao, meta_minutos, id_cliente) VALUES (@titulo, @descricao, @meta_minutos, @id_cliente)";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                Interface.LimparTelaGeral();
                Interface.Titulo("CRIE SEU PLANO DE ESTUDO");

                AnsiConsole.MarkupLine($"\n{Textos.MensagemMeta}\n");
                AnsiConsole.MarkupLine("[#D3CCC7]─────────────────────────────────[/]\n");

                var titulo = AnsiConsole.Ask<string>("\nDefina um título para sua meta: ");
                var descricao = AnsiConsole.Ask<string>("\nDefina uma descrição para sua meta: ");

                Console.WriteLine("Defina uma meta em minutos para focar na tarefa: (campo não obrigatório)");
                string entrada = Console.ReadLine()!.Trim();

                int meta_minutos = 0;
                if (!string.IsNullOrWhiteSpace(entrada))
                {
                    int.TryParse(entrada, out meta_minutos);
                }

                cmd.Parameters.AddWithValue("@titulo", titulo);
                cmd.Parameters.AddWithValue("@descricao", descricao);
                cmd.Parameters.AddWithValue("@meta_minutos", meta_minutos);
                cmd.Parameters.AddWithValue("@id_cliente", id_gerado);

                cmd.ExecuteNonQuery();

                Mensagens.Sucesso_MetaCadastrada();
                return -1;
            }
        }
    }
    public static void DefinirPrioridade(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            string SalvarPrioridade = "";
            conn.Open();
            Interface.LimparTelaGeral();
            Interface.Titulo("DEFINA A PRIORIDADE DA META");
            string sql = "";

            SalvarPrioridade = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Escolha o nível de prioridade da sua meta")
            .AddChoices("Sem prioridade", "Prioridade baixa", "Prioridade média", "Prioridade alta", "Sair")
            .HighlightStyle(new Style(
            foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold))
            .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
            );

            if (SalvarPrioridade == "Sair")
            {
                return;
            }
            sql = "UPDATE Estudo SET prioridade = @prioridade WHERE id = @id";


            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@prioridade", SalvarPrioridade);

                int linhasAfetadas = cmd.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    Mensagens.Sucesso_FinalizarApagarMeta("prioridade definida");
                }
                else
                {
                    Mensagens.Erro_PlanoNaoEncontrado(id);
                }
            }
        }
    }



    /// <summary>
    /// Permite ao usuário selecionar uma meta e iniciar uma sessão de estudo.
    /// </summary>
    public int EscolherEstudo(
        int id_cliente,
        int? id_categoria = null,
        TipoFiltroEstudo filtro = TipoFiltroEstudo.Todas,
        string? termoBusca = null)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = "SELECT id, titulo FROM Estudo WHERE id_cliente = @id_cliente";

            if (id_categoria.HasValue)
            {
                sql += " AND id_categoria = @id_categoria";
            }

            switch (filtro)
            {
                case TipoFiltroEstudo.Pendentes:
                    sql += " AND (concluido = 0 OR concluido IS NULL)";
                    break;

                case TipoFiltroEstudo.Concluidas:
                    sql += " AND concluido = 1";
                    break;

                case TipoFiltroEstudo.Pesquisa:
                    if (!string.IsNullOrWhiteSpace(termoBusca))
                    {
                        sql += " AND (titulo LIKE @termo OR descricao LIKE @termo)";
                    }
                    break;
            }

            sql += filtro switch
            {
                TipoFiltroEstudo.UltimasCriadas => " ORDER BY id DESC",
                TipoFiltroEstudo.PorTitulo => " ORDER BY titulo ASC",
                TipoFiltroEstudo.PorTempoEstudado => " ORDER BY minutos_estudados DESC",
                _ => " ORDER BY id ASC"
            };

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);

                if (id_categoria.HasValue)
                {
                    cmd.Parameters.AddWithValue("@id_categoria", id_categoria.Value);
                }

                if (filtro == TipoFiltroEstudo.Pesquisa && !string.IsNullOrWhiteSpace(termoBusca))
                {
                    cmd.Parameters.AddWithValue("@termo", $"%{termoBusca}%");
                }

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var estudos = new Dictionary<int, string>();

                    var menu = new SelectionPrompt<string>()
                        .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             ESCOLHA UMA META[/]\n[#D3CCC7]─────────────────────────────────[/]")
                        .HighlightStyle(new Style(
                            foreground: Color.FromHex($"{Cores.Opcoes}"),
                            decoration: Decoration.Bold));

                    menu.AddChoice("Sair");

                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string titulo = reader["titulo"].ToString()!;

                        // Formatação corrigida para evitar conflitos com tags de markup do Spectre.Console
                        string itemMenu = $"{id} - {Markup.Escape(titulo)}";

                        estudos.Add(id, itemMenu);
                        menu.AddChoice(itemMenu);
                    }

                    if (estudos.Count == 0)
                    {
                        Mensagens.Erro_SemInformacoes();
                        return -1;
                    }

                    string resposta = AnsiConsole.Prompt(menu);

                    if (resposta == "Sair")
                    {
                        return -1;
                    }

                    int idEscolhido = estudos
                        .First(x => x.Value == resposta)
                        .Key;


                    return idEscolhido;
                }
            }
        }
    }


    public static DateTime? Escolher(int id_estudo)
    {
        Interface.LimparTelaGeral();
        Interface.Titulo("DEFINA UMA META LIMITE");

        string opcao = AnsiConsole.Prompt(
                                       new SelectionPrompt<string>()
                                       .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
                                       .HighlightStyle(new Style(
                                           foreground: Color.FromHex($"{Cores.Opcoes}"),
                                           decoration: Decoration.Bold
                                       ))
                                       .AddChoices("Hoje", "Amanhã", "Daqui a 7 dias (1 semana)", "Daqui a 15 dias", "Daqui a 30 dias (1 mês)", "Digitar data específica (dd/mm/yyyy)", "Sair"));
        return opcao switch
        {
            "Hoje" => DateTime.Today,
            "Amanhã" => DateTime.Today.AddDays(1),
            "Daqui a 7 dias (1 semana)" => DateTime.Today.AddDays(7),
            "Daqui a 15 dias" => DateTime.Today.AddDays(15),
            "Daqui a 30 dias (1 mês)" => DateTime.Today.AddDays(30),
            "Digitar data específica (dd/mm/yyyy)" => ObterDataManual(),
            _ => null
        };
    }
    private static DateTime? ObterDataManual()
    {
        Interface.LimparTelaGeral();
        Interface.Titulo("DIGITE UMA DATA ESPECÍFICA");

        var dataString = AnsiConsole.Prompt(
            new TextPrompt<string>("[#D3CCC7]Digite a data limite ([yellow]dd/mm/yyyy[/]):[/] ")
                .Validate(input =>
                {

                    if (!DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataDigitada))
                    {
                        return ValidationResult.Error("[red]Formato inválido! Use o padrão dd/mm/yyyy (ex: 15/09/2026)[/]");
                    }


                    if (dataDigitada.Date < DateTime.Today)
                    {
                        return ValidationResult.Error("[red]A data limite não pode ser anterior a hoje![/]");
                    }

                    return ValidationResult.Success();
                })
        );
        if (dataString == "0")
            return null;

        return DateTime.ParseExact(dataString, "dd/MM/yyyy", null);
    }
    public static void AtualizarData(int id_estudo, DateTime? data)
    {

        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "UPDATE Estudo SET data_limite = @data_limite WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                {
                    cmd.Parameters.AddWithValue("@id", id_estudo);
                    cmd.Parameters.AddWithValue(
                        "@data_limite",
                        data.HasValue ? (object)data : DBNull.Value
                    );

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    Interface.LimparTelaGeral();
                    if (linhasAfetadas > 0)
                    {
                        AnsiConsole.MarkupLine("[green]Data limite atualizada com sucesso![/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Não foi possível atualizar a data limite.[/]");
                    }

                    AnsiConsole.MarkupLine("\n[grey]Pressione qualquer tecla para continuar...[/]");
                    Console.ReadKey(true);

                }
            }
        }

    }
}

