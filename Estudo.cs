using System.Reflection.Metadata.Ecma335;

namespace Init_db;

using System;
using System.Diagnostics;
using System.Threading;

using Microsoft.Data.SqlClient;

public class Estudo
{
    public int CadastrarMeta(int id_gerado)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "INSERT INTO Estudo(titulo,descricao,meta_minutos,id_cliente) " + "VALUES (@titulo,@descricao,@meta_minutos,@id_cliente)";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                Console.Clear();
                System.Console.WriteLine("===================================================================");
                Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("            === ATHENA - Crie seu plano de estudo ===        ");
                Console.ResetColor();
                System.Console.WriteLine("===================================================================\n");

                System.Console.WriteLine("Informe o título que você vai dar para a tarefa: ");
                string titulo = Console.ReadLine()!;

                System.Console.WriteLine("Agora crie uma descrição: ");
                string descricao = Console.ReadLine()!;

                System.Console.WriteLine("Defina uma meta em minútos para focar na tarefa: (campo não obrigatório)");
                int? meta_minutos = int.Parse(Console.ReadLine()!);

                cmd.Parameters.AddWithValue("@titulo", titulo);
                cmd.Parameters.AddWithValue("@descricao", descricao);
                cmd.Parameters.AddWithValue("@meta_minutos", meta_minutos);
                cmd.Parameters.AddWithValue("@id_cliente", id_gerado);

                cmd.ExecuteNonQuery();

            }
            System.Console.WriteLine("Meta definida!!");
            System.Console.WriteLine("Aperte qualquer tecla para sair");
            Console.ReadKey();

            return -1;
        }

    }
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

                    Console.Clear();
                    System.Console.WriteLine("===================================================================");
                    Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine("            === ATHENA - Seus planos de estudo===        ");
                    Console.ResetColor();
                    System.Console.WriteLine("===================================================================\n");
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
                        System.Console.WriteLine("Aperte qualquer tecla para sair! ");
                        Console.ReadKey();

                    }

                }
            }
        }
    }
    public int EscolherEstudo(int id_cliente)
    {

        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            System.Console.WriteLine("\n==================================");
            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("=== Escolha plano de estudo ===");
            Console.ResetColor();
            System.Console.WriteLine("==================================\n");

            Console.WriteLine("Digite o id do plano de estudo escolhido: ");
            int id = int.Parse(Console.ReadLine()!);

            string sql = "SELECT * FROM Estudo WHERE id = @id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                Estudo.IniciarEstudo(id_cliente, id);
            }
            return -1;
        }
    }
    public static void IniciarEstudo(int id_cliente, int id_estudo)
    {

        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "SELECT * FROM Estudo WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {

                cmd.Parameters.AddWithValue("@id", id_estudo);
                using (var Reader = cmd.ExecuteReader())
                {
                    Console.Clear();
                    System.Console.WriteLine("\n===================================================================");
                    Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine("            === ATHENA - Seu plano de estudo ===        ");
                    Console.ResetColor();
                    System.Console.WriteLine("\n===================================================================\n");

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
                        System.Console.WriteLine("[2] - atualiza titulo");
                        System.Console.WriteLine("[3] - atualiza descricao");
                        System.Console.WriteLine("[4] - atualiz meta");
                        System.Console.WriteLine("[5] - marcar como finalizada");
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
                                    Estudo.AtualizarTitulo(id_estudo);
                                    break;
                                case 3:
                                    Estudo.AtualizarDescricao(id_estudo);
                                    break;
                                case 4:
                                    Estudo.AtualizarMeta(id_estudo);
                                    break;
                                case 5:
                                    Estudo.MarcarFinalizada(id_estudo);
                                    break;
                                case 6:
                                    Estudo.ApagarMeta(id_estudo);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Digite um número válido!");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Nenhum plano de estudo encontrado com o ID {id_estudo}.");
                        Console.ReadKey();
                    }
                }


            }
        }
    }
    public static int AtualizarTitulo(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {

            conn.Open();
            System.Console.WriteLine("digite o novo tutulo");
            string titulo = Console.ReadLine()!;
            string sql = "UPDATE Estudo SET titulo = @titulo WHERE id = @id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {



                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@titulo", titulo);

                int linhasAfetadas = cmd.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    Console.WriteLine("Título atualizado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Nenhum estudo encontrado com esse ID.");
                }

            }
        }

        return -1;
    }

    public static int AtualizarDescricao(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {

            conn.Open();
            System.Console.WriteLine("digite a descrucai");
            string descricao = Console.ReadLine()!;
            string sql = "UPDATE Estudo SET descricao = @descricao WHERE id = @id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {



                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@descricao", descricao);

                int linhasAfetadas = cmd.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    Console.WriteLine("Descrição atualizado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Nenhum estudo encontrado com esse ID.");
                }

            }
        }

        return -1;
    }

    public static int AtualizarMeta(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {

            conn.Open();
            System.Console.WriteLine("digite a meta em minutos");
            int meta_minutos = int.Parse(Console.ReadLine()!);
            string sql = "UPDATE Estudo SET meta_minutos = @meta_minutos WHERE id = @id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@meta_minutos", meta_minutos);

                int linhasAfetadas = cmd.ExecuteNonQuery();

                if (linhasAfetadas > 0)
                {
                    Console.WriteLine("Descrição atualizado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Nenhum estudo encontrado com esse ID.");
                }

            }
        }

        return -1;
    }
    public static void MarcarFinalizada(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {

            conn.Open();
            System.Console.WriteLine("Marcar a meta como finalizada? (s/n)");
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
                        Console.WriteLine("Meta marcada como concluída com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Nenhum estudo encontrado com esse ID.");
                    }
                }

            }
            else
            {
                {
                    Console.WriteLine("Operação cancelada.");
                }
            }
        }
    }

    public static void ApagarMeta (int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {

            conn.Open();
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
                        Console.WriteLine("Deletado com sucesso");
                    }
                    else
                    {
                        Console.WriteLine("Nenhum estudo encontrado com esse ID.");
                    }
                }

            }
            else
            {
                {
                    Console.WriteLine("Operação cancelada.");
                }
            }
        }
    }
}


