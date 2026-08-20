namespace Init_db;

using Spectre.Console;
using Microsoft.Data.SqlClient;

/// <summary>
/// Responsável pela consulta, filtragem e ordenação das metas de estudo do cliente.
/// </summary>
class ListarEstudo
{
    /// <summary>
    /// Pesquisa metas pelo título ou descrição com base no termo fornecido.
    /// </summary>
    /// <param name="pesquisa">O termo de busca a ser pesquisado.</param>
    /// <param name="id_cliente">O identificador único do cliente.</param>
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
                        int idEscolhido = estudo.EscolherEstudo(id_cliente, filtro: Estudo.TipoFiltroEstudo.Pesquisa, termoBusca: pesquisa);
                        if (idEscolhido != -1)
                        {
                            GerenciaMetas.IniciarEstudo(id_cliente, idEscolhido);
                        }
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
    /// Exibe todas as metas cadastradas pelo usuário, permitindo gerenciamento ou vinculação a categorias.
    /// </summary>
    /// <param name="id">O identificador único do cliente.</param>
    /// <param name="adicionarCategoria">Indica se a consulta é para vincular a meta a uma categoria.</param>
    /// <param name="id_categoria">O identificador opcional da categoria para vinculação.</param>
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
                                int idEscolhido = estudo.EscolherEstudo(id, filtro: Estudo.TipoFiltroEstudo.Todas);
                                Categoria.VincularCategoria(idCategoria, idEscolhido);

                            }
                            else if (!adicionarCategoria)
                            {
                                if (Mensagens.IniciarEstudo() == "Sim")
                                {
                                    Estudo estudo = new Estudo();
                                    int idEscolhido = estudo.EscolherEstudo(id, filtro: Estudo.TipoFiltroEstudo.Todas);
                                    GerenciaMetas.IniciarEstudo(id, idEscolhido);
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
    /// Exibe apenas as metas pendentes do cliente.
    /// </summary>
    /// <param name="id">O identificador único do cliente.</param>
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
                                int idEscolhido = estudo.EscolherEstudo(id, filtro: Estudo.TipoFiltroEstudo.Pendentes);
                                if (idEscolhido != -1)
                                {
                                    GerenciaMetas.IniciarEstudo(id, idEscolhido);
                                }
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
    /// Exibe apenas as metas concluídas do cliente.
    /// </summary>
    /// <param name="id">O identificador único do cliente.</param>
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
                                int idEscolhido = estudo.EscolherEstudo(id, filtro: Estudo.TipoFiltroEstudo.Concluidas);
                                 if (idEscolhido != -1)
                                {
                                    GerenciaMetas.IniciarEstudo(id, idEscolhido);
                                }
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
    /// Exibe as metas cadastradas ordenadas de forma decrescente pela data de criação (ID).
    /// </summary>
    /// <param name="id">O identificador único do cliente.</param>
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
                                int idEscolhido = estudo.EscolherEstudo(id, filtro: Estudo.TipoFiltroEstudo.UltimasCriadas);
                                 if (idEscolhido != -1)
                                {
                                    GerenciaMetas.IniciarEstudo(id, idEscolhido);
                                }
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
    /// Exibe as metas ordenadas de forma decrescente pelo tempo total estudado.
    /// </summary>
    /// <param name="id">O identificador único do cliente.</param>
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
                                int idEscolhido = estudo.EscolherEstudo(id, filtro: Estudo.TipoFiltroEstudo.PorTempoEstudado);
                                 if (idEscolhido != -1)
                                {
                                    GerenciaMetas.IniciarEstudo(id, idEscolhido);
                                }
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
    /// Exibe as metas ordenadas alfabeticamente pelo título.
    /// </summary>
    /// <param name="id">O identificador único do cliente.</param>
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
                                int idEscolhido = estudo.EscolherEstudo(id, filtro: Estudo.TipoFiltroEstudo.PorTitulo);
                                 if (idEscolhido != -1)
                                {
                                    GerenciaMetas.IniciarEstudo(id, idEscolhido);
                                }
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
}