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
    /// <summary>
    /// Enumeração com os tipos de filtros e ordenações aplicáveis na busca de metas.
    /// </summary>
    public enum TipoFiltroEstudo
    {
        /// <summary>Busca todas as metas cadastradas sem restrição de status.</summary>
        Todas,
        /// <summary>Filtra apenas metas pendentes.</summary>
        Pendentes,
        /// <summary>Filtra apenas metas marcadas como concluídas.</summary>
        Concluidas,
        /// <summary>Ordena as metas das mais recentes para as mais antigas.</summary>
        UltimasCriadas,
        /// <summary>Ordena as metas em ordem alfabética de título.</summary>
        PorTitulo,
        /// <summary>Ordena as metas pelo tempo total estudado em minutos.</summary>
        PorTempoEstudado,
        /// <summary>Aplica busca por palavra-chave no título ou descrição.</summary>
        Pesquisa
    }

    /// <summary>
    /// Cadastra uma nova meta de estudo vinculada ao usuário informado.
    /// </summary>
    /// <param name="id_gerado">O identificador do cliente proprietário da meta.</param>
    /// <returns>Retorna -1 após a conclusão da inclusão.</returns>
    public int CadastrarMeta(int id_gerado)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            // Instrução SQL para incluir um novo plano de estudos com meta em minutos.
            string sql = "INSERT INTO Estudo(titulo, descricao, meta_minutos, id_cliente) VALUES (@titulo, @descricao, @meta_minutos, @id_cliente)";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                Interface.LimparTelaGeral();
                Interface.Titulo("CRIE SEU PLANO DE ESTUDO");

                AnsiConsole.MarkupLine($"\n{Textos.MensagemMeta}\n");
                AnsiConsole.MarkupLine("[#D3CCC7]─────────────────────────────────[/]\n");

                // Solicita os dados da nova meta ao usuário.
                var titulo = AnsiConsole.Ask<string>("\nDefina um título para sua meta: ");
                var descricao = AnsiConsole.Ask<string>("\nDefina uma descrição para sua meta: ");

                Console.WriteLine("Defina uma meta em minutos para focar na tarefa: (campo não obrigatório)");
                string entrada = Console.ReadLine()!.Trim();

                // Converte a meta de minutos caso tenha sido informada.
                int meta_minutos = 0;
                if (!string.IsNullOrWhiteSpace(entrada))
                {
                    int.TryParse(entrada, out meta_minutos);
                }

                // Associa todos os parâmetros ao comando SQL.
                cmd.Parameters.AddWithValue("@titulo", titulo);
                cmd.Parameters.AddWithValue("@descricao", descricao);
                cmd.Parameters.AddWithValue("@meta_minutos", meta_minutos);
                cmd.Parameters.AddWithValue("@id_cliente", id_gerado);

                // Executa a gravação no banco de dados.
                cmd.ExecuteNonQuery();

                Mensagens.Sucesso_MetaCadastrada();
                return -1;
            }
        }
    }

    /// <summary>
    /// Altera a prioridade de uma meta de estudo específica.
    /// </summary>
    /// <param name="id">O identificador da meta que terá sua prioridade definida.</param>
    public static void DefinirPrioridade(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            string SalvarPrioridade = "";
            conn.Open();
            Interface.LimparTelaGeral();
            Interface.Titulo("DEFINA A PRIORIDADE DA META");
            string sql = "";

            // Exibe as opções de prioridade via seleção gráfica no console.
            SalvarPrioridade = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Escolha o nível de prioridade da sua meta")
            .AddChoices("Sem prioridade", "Prioridade baixa", "Prioridade média", "Prioridade alta", "Sair")
            .HighlightStyle(new Style(
            foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold))
            .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
            );

            // Cancela a operação se a escolha for Sair.
            if (SalvarPrioridade == "Sair")
            {
                return;
            }
            sql = "UPDATE Estudo SET prioridade = @prioridade WHERE id = @id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@prioridade", SalvarPrioridade);

                // Aplica a alteração de prioridade.
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
    /// <param name="id_cliente">O identificador do cliente proprietário.</param>
    /// <param name="id_categoria">Filtro opcional por identificador de categoria.</param>
    /// <param name="filtro">O tipo de filtro ou ordenação aplicado à busca de metas.</param>
    /// <param name="termoBusca">Termo para pesquisa textual em título ou descrição.</param>
    /// <returns>O identificador da meta selecionada ou -1 se cancelado/não encontrado.</returns>
    public int EscolherEstudo(
        int id_cliente,
        int? id_categoria = null,
        TipoFiltroEstudo filtro = TipoFiltroEstudo.Todas,
        string? termoBusca = null)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            // Montagem dinâmica da instrução SQL com base nas opções e filtros selecionados.
            string sql = "SELECT id, titulo FROM Estudo WHERE id_cliente = @id_cliente";

            // Aplica o filtro de categoria se fornecido.
            if (id_categoria.HasValue)
            {
                sql += " AND id_categoria = @id_categoria";
            }

            // Aplica os filtros de status e busca textual.
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

            // Concatena a ordenação dos dados conforme o parâmetro do enum.
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

                    // Adiciona as metas lidas ao menu de seleção e ao dicionário.
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string titulo = reader["titulo"].ToString()!;

                        // Formatação corrigida para evitar conflitos com tags de markup do Spectre.Console
                        string itemMenu = $"{id} - {Markup.Escape(titulo)}";

                        estudos.Add(id, itemMenu);
                        menu.AddChoice(itemMenu);
                    }

                    // Se não houver registros retornado pela query.
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

                    // Recupera o ID da meta escolhida a partir da string correspondente no menu.
                    int idEscolhido = estudos
                        .First(x => x.Value == resposta)
                        .Key;

                    return idEscolhido;
                }
            }
        }
    }

    /// <summary>
    /// Exibe opções pré-definidas ou personalizadas para o estabelecimento de uma data limite para a meta.
    /// </summary>
    /// <param name="id_estudo">O identificador da meta que receberá o limite de data.</param>
    /// <returns>A data limite selecionada ou <c>null</c> se cancelado.</returns>
    public static DateTime? Escolher(int id_estudo)
    {
        Interface.LimparTelaGeral();
        Interface.Titulo("DEFINA UMA META LIMITE");

        // Menu com opções prontas de prazos ou digitação manual.
        string opcao = AnsiConsole.Prompt(
                                       new SelectionPrompt<string>()
                                       .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
                                       .HighlightStyle(new Style(
                                           foreground: Color.FromHex($"{Cores.Opcoes}"),
                                           decoration: Decoration.Bold
                                       ))
                                       .AddChoices("Hoje", "Amanhã", "Daqui a 7 dias (1 semana)", "Daqui a 15 dias", "Daqui a 30 dias (1 mês)", "Digitar data específica (dd/mm/yyyy)", "Sair"));
        
        // Mapeia a opção selecionada para um objeto DateTime correspondente.
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

    /// <summary>
    /// Solicita ao usuário a entrada manual de uma data e aplica regras de validação.
    /// </summary>
    /// <returns>A data válida fornecida pelo usuário ou <c>null</c> caso haja cancelamento.</returns>
    private static DateTime? ObterDataManual()
    {
        Interface.LimparTelaGeral();
        Interface.Titulo("DIGITE UMA DATA ESPECÍFICA");

        // Validação da entrada no formato brasileiro dd/MM/yyyy e prevenção de datas passadas.
        var dataString = AnsiConsole.Prompt(
            new TextPrompt<string>("[#D3CCC7]Digite a data limite ([yellow]dd/mm/yyyy[/]):[/] ")
                .Validate(input =>
                {
                    // Tenta converter o texto para o formato de data aceito.
                    if (!DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime dataDigitada))
                    {
                        return ValidationResult.Error("[red]Formato inválido! Use o padrão dd/mm/yyyy (ex: 15/09/2026)[/]");
                    }

                    // Bloqueia definição de datas anteriores à atual.
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

    /// <summary>
    /// Atualiza o campo de data limite de uma determinada meta no banco de dados.
    /// </summary>
    /// <param name="id_estudo">O identificador da meta que terá a data alterada.</param>
    /// <param name="data">A nova data a ser atribuída ou <c>null</c> para limpar o prazo.</param>
    public static void AtualizarData(int id_estudo, DateTime? data)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            // Atualiza a coluna data_limite com o valor enviado ou com o valor nulo do SQL.
            string sql = "UPDATE Estudo SET data_limite = @data_limite WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                {
                    cmd.Parameters.AddWithValue("@id", id_estudo);
                    cmd.Parameters.AddWithValue(
                        "@data_limite",
                        data.HasValue ? (object)data : DBNull.Value
                    );

                    // Executa a alteração.
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