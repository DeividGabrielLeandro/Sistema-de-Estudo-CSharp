using System.Reflection.Metadata.Ecma335;

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
                string resposta = "";
                do
                {
                    Interface.LimparTelaGeral();
                    Interface.EscreverCentralizado("ATHENA -  Crie seu plano de estudo");

                    string titulo = Validacao.EntradaObrigatoria("Defina um título para sua meta: ", ref resposta);
                    if (titulo == null) continue;

                    string descricao = Validacao.EntradaObrigatoria("Defina uma descrição para sua meta: ", ref resposta);

                    if (descricao == null) continue;

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

                } while (resposta == "s");
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
            Interface.EscreverCentralizado("ATHENA - Pesquise sua meta de estudo");
            try
            {

                while (Reader.Read())

                {
                    MetaEncontrada = true;
                    bool concluido = Convert.ToBoolean(Reader["concluido"]);

                    System.Console.WriteLine($"Id: {Reader["id"]}");
                    System.Console.WriteLine($"Titulo: {Reader["titulo"]}");
                    System.Console.WriteLine($"Descrição: {Reader["descricao"]}");
                    System.Console.WriteLine($"Meta em minútos: {Reader["meta_minutos"]} min");
                    System.Console.WriteLine($"Minutos estudados: {Reader["minutos_estudados"]}");
                    System.Console.WriteLine($"Criado em: {Reader["data_criacao"]}");

                    if (!concluido)
                    {
                        System.Console.WriteLine("Tarefa ainda não concluída\n");
                    }
                    else
                    {
                        System.Console.WriteLine("Tarefa concluída\n");
                    }

                    System.Console.WriteLine("===================================================================\n");
                }
                if (MetaEncontrada == true)
                {
                    System.Console.WriteLine("Iniciar algum plano de estudo? (s/n)");
                    string resposta = Console.ReadLine()!;
                    if (resposta == "s")
                    {
                        Estudo estudo = new Estudo();
                        estudo.EscolherEstudo(id_cliente);
                    }
                    else
                    {
                        Mensagens.Sair();
                    }
                }
                else if (MetaEncontrada == false)
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
                    Interface.EscreverCentralizado("ATHENA -  Seus planos de estudo ");

                    while (Reader.Read())
                    {
                        bool concluido = Convert.ToBoolean(Reader["concluido"]);

                        System.Console.WriteLine($"Id: {Reader["id"]}");
                        System.Console.WriteLine($"Titulo: {Reader["titulo"]}");
                        System.Console.WriteLine($"Descrição: {Reader["descricao"]}");
                        System.Console.WriteLine($"Meta em minútos: {Reader["meta_minutos"]} min");
                        System.Console.WriteLine($"Minutos estudados: {Reader["minutos_estudados"]}");
                        System.Console.WriteLine($"Criado em: {Reader["data_criacao"]}");

                        if (!concluido)
                        {
                            System.Console.WriteLine("Tarefa ainda não concluída\n");
                        }
                        else
                        {
                            System.Console.WriteLine("Tarefa concluída\n");
                        }

                        System.Console.WriteLine("===================================================================\n");
                    }
                    System.Console.WriteLine("Iniciar algum plano de estudo? (s/n)");
                    string resposta = Console.ReadLine()!;
                    if (resposta == "s")
                    {
                        Estudo estudo = new Estudo();
                        estudo.EscolherEstudo(id);
                    }
                    else
                    {
                        Mensagens.Sair();
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
                    Interface.EscreverCentralizado("ATHENA -  Seus planos de estudo ");

                    while (Reader.Read())
                    {
                        bool concluido = Convert.ToBoolean(Reader["concluido"]);

                        System.Console.WriteLine($"Id: {Reader["id"]}");
                        System.Console.WriteLine($"Titulo: {Reader["titulo"]}");
                        System.Console.WriteLine($"Descrição: {Reader["descricao"]}");
                        System.Console.WriteLine($"Meta em minútos: {Reader["meta_minutos"]} min");
                        System.Console.WriteLine($"Minutos estudados: {Reader["minutos_estudados"]}");
                        System.Console.WriteLine($"Criado em: {Reader["data_criacao"]}");

                        if (!concluido)
                        {
                            System.Console.WriteLine("Tarefa ainda não concluída\n");
                        }
                        else
                        {
                            System.Console.WriteLine("Tarefa concluída\n");
                        }

                        System.Console.WriteLine("===================================================================\n");
                    }
                    System.Console.WriteLine("Iniciar algum plano de estudo? (s/n)");
                    string resposta = Console.ReadLine()!;
                    if (resposta == "s")
                    {
                        Estudo estudo = new Estudo();
                        estudo.EscolherEstudo(id);
                    }
                    else
                    {
                        Mensagens.Sair();
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
                    Interface.EscreverCentralizado("ATHENA -  Seus planos de estudo ");

                    while (Reader.Read())
                    {
                        bool concluido = Convert.ToBoolean(Reader["concluido"]);

                        System.Console.WriteLine($"Id: {Reader["id"]}");
                        System.Console.WriteLine($"Titulo: {Reader["titulo"]}");
                        System.Console.WriteLine($"Descrição: {Reader["descricao"]}");
                        System.Console.WriteLine($"Meta em minútos: {Reader["meta_minutos"]} min");
                        System.Console.WriteLine($"Minutos estudados: {Reader["minutos_estudados"]}");
                        System.Console.WriteLine($"Criado em: {Reader["data_criacao"]}");

                        if (!concluido)
                        {
                            System.Console.WriteLine("Tarefa ainda não concluída\n");
                        }
                        else
                        {
                            System.Console.WriteLine("Tarefa concluída\n");
                        }

                        System.Console.WriteLine("===================================================================\n");
                    }
                    System.Console.WriteLine("Iniciar algum plano de estudo? (s/n)");
                    string resposta = Console.ReadLine()!;
                    if (resposta == "s")
                    {
                        Estudo estudo = new Estudo();
                        estudo.EscolherEstudo(id);
                    }
                    else
                    {
                        Mensagens.Sair();
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
            string resposta = "";
            int? id = null;
            do
            {
                System.Console.WriteLine("\n==================================");
                Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("=== Escolha plano de estudo ===");
                Console.ResetColor();
                System.Console.WriteLine("==================================\n");

                id = Validacao.IntVazio("Digite o id do plano de estudo escolhido: ", ref resposta);

                if (id == null) continue;

                resposta = "sucesso";

            } while (resposta == "s");
            if (resposta != "sucesso" || id == null) return -1;
            string sql = "SELECT * FROM Estudo WHERE id = @id AND id_cliente = @id_cliente";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id.Value);
                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Estudo.IniciarEstudo(id_cliente, id.Value);
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id.Value);
                        return -1;
                    }
                }
            }
        }
        return -1;
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



                        Interface.LimparTelaGeral();
                        Interface.EscreverCentralizado("ATHENA -  Seu plano de estudo ");


                        if (Reader.Read())

                        {
                            bool concluido = Convert.ToBoolean(Reader["concluido"]);

                            System.Console.WriteLine($"Titulo: {Reader["titulo"]}");
                            System.Console.WriteLine("==================================\n");
                            System.Console.WriteLine($"Descrição: {Reader["descricao"]}");
                            System.Console.WriteLine($"Sua meta: {Reader["meta_minutos"]} min\n");
                            System.Console.WriteLine("==================================\n");
                            System.Console.WriteLine($"{Reader["minutos_estudados"]} minuto(s) estudado(s)\n");

                            if (!concluido)
                            {
                                System.Console.WriteLine("Tarefa em andamento");
                            }
                            else
                            {
                                System.Console.WriteLine("Tarefa concluída");
                            }

                            System.Console.WriteLine("==================================");
                            System.Console.WriteLine("             OPÇÔES               ");
                            System.Console.WriteLine("==================================\n");
                            System.Console.WriteLine("[1] - Começar a contar o tempo");
                            System.Console.WriteLine("[2] - Abrir as opções");
                            System.Console.WriteLine("[3] - marcar como finalizada");
                            System.Console.WriteLine("[4] - Sair");
                            System.Console.WriteLine("Digite a opção escolhida: ");
                            if (int.TryParse(Console.ReadLine(), out int opcao))
                            {
                                switch (opcao)
                                {
                                    case 1:
                                        double minutos = Cronometro.ContarTempo();
                                        Cronometro.SalvarTempo(id_estudo, minutos);
                                        Cronometro.AtualizarTempoTotalCliente(id_cliente, minutos);
                                        break;
                                    case 2:
                                        Interface.PersonalizarMetas(id_estudo);
                                        break;
                                    case 3:
                                        Estudo.MarcarFinalizada(id_estudo);
                                        break;
                                    case 4:
                                        Sair = true;
                                        break;

                                }
                            }
                            else
                            {
                                Mensagens.Erro_NumeroInvalido();
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


    /// <summary>
    /// Atualiza o título de uma meta.
    /// </summary>
    public static int AtualizarTitulo(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            string resposta = "";
            do
            {
                conn.Open();
                Interface.LimparTelaGeral();
                Interface.EscreverCentralizado("ATHENA -  Atualize a sua meta ");

                string titulo = Validacao.EntradaObrigatoria("Defina um título para sua meta: ", ref resposta);
                if (titulo == null) continue;

                string sql = "UPDATE Estudo SET titulo = @titulo WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@titulo", titulo);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_AtualizarMeta("Titulo");
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }

                }
            } while (resposta == "s");

            return -1;
        }
    }


    /// <summary>
    /// Atualiza a descrição de uma meta.
    /// </summary>
    public static int AtualizarDescricao(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            string resposta = "";
            do
            {
                conn.Open();
                Interface.EscreverCentralizado("ATHENA -  Atualize a sua meta ");
                Interface.LimparTelaGeral();

                string descricao = Validacao.EntradaObrigatoria("Defina uma descrição para sua meta: ", ref resposta);
                if (descricao == null) continue;

                string sql = "UPDATE Estudo SET descricao = @descricao WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@descricao", descricao);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_AtualizarMeta("Descrição");
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }


                }
            } while (resposta == "s");
        }

        return -1;
    }


    /// <summary>
    /// Atualiza a meta de minutos de estudo.
    /// </summary>
    public static int AtualizarMeta(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {

            conn.Open();
            string resposta = "";
            do
            {
                Interface.EscreverCentralizado("ATHENA -  Atualize sua meta ");
                Interface.LimparTelaGeral();

                string meta_minutos = Validacao.EntradaObrigatoria("Digite a meta em minutos: ", ref resposta);
                if (meta_minutos == null) continue;

                string sql = "UPDATE Estudo SET meta_minutos = @meta_minutos WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@meta_minutos", meta_minutos);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_AtualizarMeta("Meta");
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }

                }
            } while (resposta == "s");
        }

        return -1;
    }


    /// <summary>
    /// Marca uma meta como concluída.
    /// </summary>
    public static void MarcarFinalizada(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {

            conn.Open();
            Interface.EscreverCentralizado("ATHENA -  Marcar como finalizada ");
            Interface.LimparTelaGeral();

            System.Console.WriteLine("Deseja marcar a meta como finalizada? (s/n)");
            string concluido = Console.ReadLine()!;
            if (concluido == "s")
            {

                string sql = "UPDATE Estudo SET concluido = 1 WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@id", id);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_FinalizarApagarMeta("finalizada");
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }
                }

            }
            else
            {
                {
                    Mensagens.Erro_Cancelada();
                }
            }
        }
    }


    /// <summary>
    /// Remove uma meta do banco de dados após confirmação do usuário.
    /// </summary>
    public static void ApagarMeta(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            Interface.LimparTelaGeral();

            conn.Open();
            Interface.EscreverCentralizado("ATHENA -  Apague a sua meta ");

            System.Console.WriteLine("Apagar essa meta? (s/n)");
            string concluido = Console.ReadLine()!;
            if (concluido == "s")
            {

                string sql = "DELETE FROM Estudo WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@id", id);

                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.Sucesso_FinalizarApagarMeta("apagada");
                    }
                    else
                    {
                        Mensagens.Erro_PlanoNaoEncontrado(id);
                    }
                }

            }
            else
            {
                {
                    Mensagens.Erro_Cancelada();
                }
            }
        }
    }
}


