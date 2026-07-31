using System.Reflection.Metadata.Ecma335;
using Spectre.Console;
namespace Init_db;

using System;
using System.Diagnostics;
using System.Threading;

using Microsoft.Data.SqlClient;

/// <summary>
/// Gerencia o cadastro, consulta, atualização e acompanhamento
/// das metas de estudo do usuário.
/// </summary>
public class Estudo
{
    /// <summary>
    /// Cadastra uma nova meta de estudo vinculada ao usuário informado.
    /// </summary>
    /// <param name="id_gerado">Identificador do usuário.</param>
    /// <returns>Retorna -1 ao finalizar o processo.</returns>
    public int CadastrarMeta(int id_gerado)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "INSERT INTO Estudo(titulo,descricao,meta_minutos,id_cliente) " + "VALUES (@titulo,@descricao,@meta_minutos,@id_cliente)";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                
                    Interface.LimparTelaGeral();
                    Interface.Titulo("CRIE SEU PLANO DE ESTUDO");

                    AnsiConsole.MarkupLine($"\n{Textos.MensagemMeta}\n");
                    AnsiConsole.MarkupLine("[#D3CCC7]─────────────────────────────────[/]\n");

                    var titulo = AnsiConsole.Ask<string>("\nDefina um título para sua meta: ");
                    var descricao = AnsiConsole.Ask<string>("\nDefina uma descrição para sua meta: ");

                    System.Console.WriteLine("Defina uma meta em minútos para focar na tarefa: (campo não obrigatório)");
                    string entrada = Console.ReadLine()!.Trim();

                    int meta_minutos = 0;

                    // Permite que o usuário deixe a meta de minutos em branco.
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
    /// <param name="pesquisa">Texto utilizado na pesquisa.</param>
    /// <param name="id_cliente">Identificador do usuário.</param>
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
                        estudo.EscolherEstudo(id_cliente);
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
            catch
            {
                Mensagens.Erro_SemInformacoes();
            }


        }

    }


    /// <summary>
    /// Exibe todas as metas cadastradas pelo usuário.
    /// </summary>
    public static void MostrarMetas(int id)
    {
        bool MetaEncontrada = true;

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

                            if (Mensagens.IniciarEstudo() == "Sim")
                            {
                                Estudo estudo = new Estudo();
                                estudo.EscolherEstudo(id);
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
                    catch
                    {
                        Mensagens.Erro_SemInformacoes();
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
            string sql = "SELECT * FROM Estudo WHERE id_cliente = @id AND concluido = 0";
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
                                estudo.EscolherEstudo(id);
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
                    catch
                    {
                        Mensagens.Erro_SemInformacoes();
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
                                estudo.EscolherEstudo(id);
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
                    catch
                    {
                        Mensagens.Erro_SemInformacoes();
                    }

                }
            }
        }
    }


    /// <summary>
    /// Permite ao usuário selecionar uma meta e iniciar uma sessão de estudo.
    /// </summary>
    /// <param name="id_cliente">Identificador do usuário.</param>
    /// <returns>
    /// Retorna -1 quando nenhuma meta é selecionada ou o processo é cancelado.
    /// </returns>
    public int EscolherEstudo(int id_cliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = "SELECT id, titulo FROM Estudo WHERE id_cliente = @id_cliente";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    var estudos = new Dictionary<int, string>();

                    var menu = new SelectionPrompt<string>()
                        .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
                        .HighlightStyle(new Style(
                            foreground: Color.FromHex("#EF0606"),
                            decoration: Decoration.Bold));

                    menu.AddChoice("Sair");

                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string titulo = reader["titulo"].ToString()!;

                        estudos.Add(id, titulo);
                        menu.AddChoice(titulo);
                    }

                    string resposta = AnsiConsole.Prompt(menu);

                    if (resposta == "Sair")
                        return -1;

                    int idEscolhido = estudos
                        .First(x => x.Value == resposta)
                        .Key;

                    Estudo.IniciarEstudo(id_cliente, idEscolhido);

                    return idEscolhido;
                }
            }
        }
    }



    /// <summary>
    /// Exibe os detalhes da meta selecionada e permite iniciar,
    /// editar ou concluir a sessão de estudo.
    /// </summary>
    /// <param name="id_cliente">Identificador do usuário.</param>
    /// <param name="id_estudo">Identificador da meta.</param>
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
                                        .HighlightStyle(new Style(foreground: Color.FromHex("#EF0606")))
                                        .AddChoices("Começar a contar o tempo", "Abrir as opções", "Marcar como finalizada", "Sair")
                                        .HighlightStyle(new Style(
                                         foreground: Color.FromHex("#EF0606"), decoration: Decoration.Bold
                            )));

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
                                case "Sair":
                                    Sair = true;
                                    break;

                            }
                        }
                        else
                        {
                            Mensagens.Erro_PlanoNaoEncontrado(id_estudo);
                        }
                    }
                }

            }
        }
    }
}