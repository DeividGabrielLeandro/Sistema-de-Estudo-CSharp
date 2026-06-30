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
                        estudo.EscolherEstudo();
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
    public int EscolherEstudo()
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
                Estudo.IniciarEstudo(id);
            }
            return -1;
        }
    }

    public static void IniciarEstudo(int id_estudo)
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

                        System.Console.WriteLine($"Titulo: {Reader["titulo"]}");
                        System.Console.WriteLine("==================================\n");
                        System.Console.WriteLine($"Descrição: {Reader["descricao"]}");
                        System.Console.WriteLine($"Sua meta: {Reader["meta_minutos"]} min\n");
                        System.Console.WriteLine("==================================\n");
                        System.Console.WriteLine($"{Reader["minutos_estudados"]} minuto(s) estudado(s)\n");
                        System.Console.WriteLine("==================================");
                        System.Console.WriteLine("             OPÇÔES               ");
                        System.Console.WriteLine("==================================\n"); 
                        System.Console.WriteLine("[1] - Começar a contar o tempo");
                        System.Console.WriteLine("[2] - Sair");
                        System.Console.WriteLine("Digite a opção escolhida: ");
                        if (int.TryParse(Console.ReadLine(), out int opcao))
                        {
                            switch (opcao)
                            {
                                case 1:
                                    double minutos = Cronometro.ContarTempo(id_estudo);
                                    Cronometro.SalvarTempo(id_estudo, minutos);
                                    break;
                                case 2:
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
}
