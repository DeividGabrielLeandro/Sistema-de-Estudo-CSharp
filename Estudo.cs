namespace Init_db;

using System;
using System.Diagnostics;
using System.Threading;

using Microsoft.Data.SqlClient;

public class Estudo
{
    public static void CadastrarMeta(int id_gerado)
    {
        Estudo estudo = new Estudo();
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "INSERT INTO Estudo(titulo,descricao,meta_minutos,id_cliente) " + "VALUES (@titulo,@descricao,@meta_minutos,@id_cliente)";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
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
        }

    }
    public static void MostrarMetas()
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "SELECT * FROM Estudo ";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (var Reader = cmd.ExecuteReader())
            {
                Console.Clear();
                System.Console.WriteLine("=========================================================");
                System.Console.WriteLine("                  SEUS PLANOS DE ESTUDO");
                System.Console.WriteLine("=========================================================\n");
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

                    System.Console.WriteLine("------------------------------------------------\n");
                }

            }
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
                    System.Console.WriteLine("=========================================================");
                    System.Console.WriteLine("                  SEU PLANO DE ESTUDO");
                    System.Console.WriteLine("=========================================================\n");

                    if (Reader.Read())
                    {

                        System.Console.WriteLine($"Titulo: {Reader["titulo"]}");
                        System.Console.WriteLine("------------------------------------------------\n");
                        System.Console.WriteLine($"Descrição: {Reader["descricao"]}");
                        System.Console.WriteLine($"Sua meta: {Reader["meta_minutos"]} min\n");
                        System.Console.WriteLine("------------------------------------------------\n");
                        System.Console.WriteLine($"{Reader["minutos_estudados"]} minutos estudado(s)\n");
                        System.Console.WriteLine("Opções:");
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
