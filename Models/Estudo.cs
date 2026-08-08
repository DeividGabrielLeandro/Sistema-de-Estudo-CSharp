using System;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// Pesquisa metas pelo título ou descrição.
    /// </summary>
    public static void PesquisarMeta(string pesquisa, int id_cliente)
    {
        bool MetaEncontrada = false;
        string sql = @"SELECT * FROM Estudo WHERE id_cliente = @id_cliente AND (titulo LIKE @TermoBusca OR descricao LIKE @TermoBusca)";

        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@TermoBusca", "%" + pesquisa + "%");
            cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
            conn.Open();

            using SqlDataReader Reader = cmd.ExecuteReader();
            Interface.LimparTelaGeral();
            Interface.Titulo("PESQUISE A SUA META CRIADA");

            try
            {
                var tabela = GerenciaMetas.MostrarMetas(Reader, out MetaEncontrada);
                AnsiConsole.Write(tabela);

                if (MetaEncontrada)
                {
                    if (Mensagens.IniciarEstudo() == "Sim")
                    {
                        Estudo estudo = new Estudo();
                        estudo.EscolherEstudo(id_cliente, filtro: TipoFiltroEstudo.Pesquisa, termoBusca: pesquisa);
                    }
                    else
                    {
                        Mensagens.Sair();
                    }
                }
                else
                {
                    Mensagens.Erro_SemInformacoes();
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
                Console.ReadKey();
            }
        }
    }

    /// <summary>
    /// Exibe todas as metas cadastradas pelo usuário.
    /// </summary>
    public static void MostrarMetas(int id, bool adicionarCategoria, int? id_categoria = null)
    {
        int idCategoria = id_categoria ?? 0;
        bool MetaEncontrada = true;
        Estudo estudo1 = new Estudo();

        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "SELECT * FROM Estudo WHERE id_cliente = @id";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var Reader = cmd.ExecuteReader())
                {
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SEUS PLANOS DE ESTUDO");

                    try
                    {
                        var tabela = GerenciaMetas.MostrarMetas(Reader, out MetaEncontrada);
                        AnsiConsole.Write(tabela);

                        if (MetaEncontrada)
                        {
                            if (adicionarCategoria)
                            {
                                Estudo estudo = new Estudo();
                                int idEscolhido = estudo.EscolherEstudo(id, filtro: TipoFiltroEstudo.Todas);
                                Categoria.VincularCategoria(idCategoria, idEscolhido);

                            }
                            else if (!adicionarCategoria)
                            {
                                if (Mensagens.IniciarEstudo() == "Sim")
                                {
                                    Estudo estudo = new Estudo();
                                    int idEscolhido = estudo.EscolherEstudo(id, filtro: TipoFiltroEstudo.Todas);
                                    Estudo.IniciarEstudo(id, idEscolhido);
                                }
                                else
                                {
                                    Mensagens.Sair();
                                }
                            }
                        }
                        else
                        {
                            Mensagens.Erro_SemInformacoes();
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.WriteException(ex);
                        Console.ReadKey();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Exibe apenas as metas pendentes.
    /// </summary>
    public static void MostrarMetasPendentes(int id)
    {
        bool MetaEncontrada = true;
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "SELECT * FROM Estudo WHERE id_cliente = @id AND (concluido = 0 OR concluido IS NULL)";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var Reader = cmd.ExecuteReader())
                {
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SUAS METAS PENDENTES");

                    try
                    {
                        var tabela = GerenciaMetas.MostrarMetas(Reader, out MetaEncontrada);
                        AnsiConsole.Write(tabela);

                        if (MetaEncontrada)
                        {
                            if (Mensagens.IniciarEstudo() == "Sim")
                            {
                                Estudo estudo = new Estudo();
                                estudo.EscolherEstudo(id, filtro: TipoFiltroEstudo.Pendentes);
                            }
                            else
                            {
                                Mensagens.Sair();
                            }
                        }
                        else
                        {
                            Mensagens.Erro_SemInformacoes();
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.WriteException(ex);
                        Console.ReadKey();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Exibe apenas as metas concluídas.
    /// </summary>
    public static void MostrarMetasConcluidas(int id)
    {
        bool MetaEncontrada = true;
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "SELECT * FROM Estudo WHERE id_cliente = @id AND concluido = 1";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var Reader = cmd.ExecuteReader())
                {
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SUAS METAS CONCLUÍDAS");

                    try
                    {
                        var tabela = GerenciaMetas.MostrarMetas(Reader, out MetaEncontrada);
                        AnsiConsole.Write(tabela);

                        if (MetaEncontrada)
                        {
                            if (Mensagens.IniciarEstudo() == "Sim")
                            {
                                Estudo estudo = new Estudo();
                                estudo.EscolherEstudo(id, filtro: TipoFiltroEstudo.Concluidas);
                            }
                            else
                            {
                                Mensagens.Sair();
                            }
                        }
                        else
                        {
                            Mensagens.Erro_SemInformacoes();
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.WriteException(ex);
                        Console.ReadKey();
                    }
                }
            }
        }
    }

    public static void MostrarUltimasCriadas(int id)
    {
        bool MetaEncontrada = true;
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "SELECT * FROM Estudo WHERE id_cliente = @id ORDER BY id DESC";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var Reader = cmd.ExecuteReader())
                {
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SUAS ÚLTIMAS METAS CRIADAS");

                    try
                    {
                        var tabela = GerenciaMetas.MostrarMetas(Reader, out MetaEncontrada);
                        AnsiConsole.Write(tabela);

                        if (MetaEncontrada)
                        {
                            if (Mensagens.IniciarEstudo() == "Sim")
                            {
                                Estudo estudo = new Estudo();
                                estudo.EscolherEstudo(id, filtro: TipoFiltroEstudo.UltimasCriadas);
                            }
                            else
                            {
                                Mensagens.Sair();
                            }
                        }
                        else
                        {
                            Mensagens.Erro_SemInformacoes();
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.WriteException(ex);
                        Console.ReadKey();
                    }
                }
            }
        }
    }

    public static void OrdenarPorTempoEstudado(int id)
    {
        bool MetaEncontrada = true;
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "SELECT * FROM Estudo WHERE id_cliente = @id ORDER BY minutos_estudados DESC";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var Reader = cmd.ExecuteReader())
                {
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SUAS METAS POR TEMPO ESTUDADO");

                    try
                    {
                        var tabela = GerenciaMetas.MostrarMetas(Reader, out MetaEncontrada);
                        AnsiConsole.Write(tabela);

                        if (MetaEncontrada)
                        {
                            if (Mensagens.IniciarEstudo() == "Sim")
                            {
                                Estudo estudo = new Estudo();
                                estudo.EscolherEstudo(id, filtro: TipoFiltroEstudo.PorTempoEstudado);
                            }
                            else
                            {
                                Mensagens.Sair();
                            }
                        }
                        else
                        {
                            Mensagens.Erro_SemInformacoes();
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.WriteException(ex);
                        Console.ReadKey();
                    }
                }
            }
        }
    }

    public static void OrdenarPorTitulo(int id)
    {
        bool MetaEncontrada = true;
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "SELECT * FROM Estudo WHERE id_cliente = @id ORDER BY titulo ASC";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                using (var Reader = cmd.ExecuteReader())
                {
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SUAS METAS ORDENADAS POR TÍTULO");

                    try
                    {
                        var tabela = GerenciaMetas.MostrarMetas(Reader, out MetaEncontrada);
                        AnsiConsole.Write(tabela);

                        if (MetaEncontrada)
                        {
                            if (Mensagens.IniciarEstudo() == "Sim")
                            {
                                Estudo estudo = new Estudo();
                                estudo.EscolherEstudo(id, filtro: TipoFiltroEstudo.PorTitulo);
                            }
                            else
                            {
                                Mensagens.Sair();
                            }
                        }
                        else
                        {
                            Mensagens.Erro_SemInformacoes();
                        }
                    }
                    catch (Exception ex)
                    {
                        AnsiConsole.WriteException(ex);
                        Console.ReadKey();
                    }
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

    /// <summary>
    /// Exibe os detalhes da meta selecionada e permite iniciar,
    /// editar ou concluir a sessão de estudo.
    /// </summary>
    public static void IniciarEstudo(int id_cliente, int id_estudo)
    {
        bool Sair = false;
        while (!Sair)
        {
            using (SqlConnection conn = new SqlConnection(Banco.Conexao))
            {
                conn.Open();
                string sql = "SELECT * FROM Estudo WHERE id = @id AND id_cliente = @id_cliente";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id_estudo);
                    cmd.Parameters.AddWithValue("@id_cliente", id_cliente);

                    using (var Reader = cmd.ExecuteReader())
                    {
                        string opcao = "";
                        Interface.LimparTelaGeral();
                        Interface.Titulo("SEU PLANO");

                        AnsiConsole.Write($"\n{Textos.MensagemMotivacional_Seneca}\n\n");

                        if (Reader.Read())
                        {
                            AnsiConsole.Write(GerenciaMetas.InformacoesMetas(Reader));

                            opcao = AnsiConsole.Prompt(
                                new SelectionPrompt<string>()
                                .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
                                .HighlightStyle(new Style(
                                    foreground: Color.FromHex($"{Cores.Opcoes}"),
                                    decoration: Decoration.Bold
                                ))
                                .AddChoices("Começar a contar o tempo", "Abrir as opções", "Marcar como finalizada", "Definir prioridade", "Vincular à uma categoria criada", "Sair"));

                            switch (opcao)
                            {
                                case "Começar a contar o tempo":
                                    double minutos = Cronometro.ContarTempo();
                                    Cronometro.SalvarTempo(id_estudo, minutos);
                                    Cronometro.AtualizarTempoTotalCliente(id_cliente, minutos);
                                    break;
                                case "Abrir as opções":
                                    Interface.PersonalizarMetas(id_estudo);
                                    break;
                                case "Marcar como finalizada":
                                    GerenciaMetas.MarcarFinalizada(id_estudo);
                                    break;
                                case "Definir prioridade":
                                    GerenciaMetas.DefinirPrioridade(id_estudo);
                                    break;
                                case "Vincular à uma categoria criada":
                                    int id = Categoria.MostrarCategorias(id_cliente);
                                    Categoria.VincularCategoria(id, id_estudo);
                                    break;
                                case "Sair":
                                    Sair = true;
                                    break;
                            }
                        }
                        else
                        {
                            Mensagens.Erro_PlanoNaoEncontrado(id_estudo);
                            Sair = true;
                        }
                    }
                }
            }
        }
    }
}