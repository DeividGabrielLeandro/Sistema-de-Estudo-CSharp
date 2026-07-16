namespace Init_db;

using System.Data.Common;
using System.Runtime.ConstrainedExecution;
using Microsoft.Data.SqlClient;


public class Cliente
{
    public int CadastrarCliente()
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "INSERT INTO Cliente(nome_completo,email,usuario,senha) " + "OUTPUT INSERTED.id" + " VALUES (@nome_completo,@email,@usuario,@senha)";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                string resposta = "";
                do
                {

                    Console.Clear();
                    System.Console.WriteLine("===================================================================");
                    Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.WriteLine("                     === CRIE O SEU CADASTRO===                    ");
                    Console.ResetColor();
                    System.Console.WriteLine("===================================================================\n");

                    System.Console.WriteLine("Digite seu nome completo: ");
                    string nome_completo = Console.ReadLine()!;
                    System.Console.WriteLine();

                    System.Console.WriteLine("Digite seu nome email: ");
                    string email = Console.ReadLine()!;
                    System.Console.WriteLine();

                    System.Console.WriteLine("Crie seu usuário: ");
                    string usuario = Console.ReadLine()!;
                    System.Console.WriteLine();

                    System.Console.WriteLine("Digite sua senha: ");
                    string senha = Console.ReadLine()!;
                    System.Console.WriteLine();
                    cmd.Parameters.AddWithValue("@nome_completo", nome_completo);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@senha", senha);
                    try
                    {
                        int id_gerado = (int)cmd.ExecuteScalar();
                        System.Console.WriteLine();
                        Console.WriteLine("CLIENTE CADASTRADO COM SUCESSO!");
                        return id_gerado;
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            System.Console.WriteLine("Não foi possível concluir o cadastro. Verifique os dados informados ou utilize outras informações.\n");
                            Console.ResetColor();
                            System.Console.WriteLine("Tentar novamente? (s/n)");
                            resposta = Console.ReadLine()!;
                            return -1;
                        }

                        throw;

                    }

                } while (resposta == "s");
            }
        }
    }
    public int FazerLogin()
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))

        {
            Console.Clear();
            conn.Open();
            string resposta = "";
            Console.Clear();
            do
            {
                System.Console.WriteLine("===================================================================");
                Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("                    === FAÇA O SEU CADASTRO ===                   ");
                Console.ResetColor();
                System.Console.WriteLine("===================================================================\n");
                System.Console.WriteLine("Digite o seu usuário: ");
                string Login = Console.ReadLine()!;
                System.Console.WriteLine();

                System.Console.WriteLine("Digite a sua senha: ");
                string Senha = Console.ReadLine()!;
                System.Console.WriteLine();


                string sql = @" SELECT id FROM Cliente WHERE usuario = @usuario AND senha = @senha";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", Login);
                    cmd.Parameters.AddWithValue("@senha", Senha);

                    object resultado = cmd.ExecuteScalar();

                    if (resultado != null)
                    {
                        Console.WriteLine("✅ Login realizado com sucesso!");

                        return (int)resultado;
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Usuário ou senha inválidos!");
                    Console.ResetColor();

                    System.Console.WriteLine("Tentar novamente?(s/n)");
                    resposta = Console.ReadLine()!;
                }
            } while (resposta == "s");
        }
        return -1;
    }

    public string ObterNomeCliente(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = "SELECT nome_completo FROM Cliente WHERE id = @id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);

                return cmd.ExecuteScalar()?.ToString() ?? "Usuário";
            }
        }
    }
    public static double MostrarTempoTotalEstudo(int idCliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = @"SELECT TotalMinutosEstudados
                       FROM Cliente
                       WHERE id = @id ";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id", idCliente);
                object resultado = cmd.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    return Convert.ToDouble(resultado);
                }

                return Convert.ToDouble(resultado);
            }
        }
    }

    public static double MostrarMetasPendentes(int idCliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = "SELECT COUNT(*) FROM Estudo WHERE id_cliente = @id_cliente AND concluido = 0";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_cliente", idCliente);
                object resultado = cmd.ExecuteScalar();

                if (resultado != null && resultado != DBNull.Value)
                {
                    return Convert.ToDouble(resultado);
                }

                return 0;
            }
        }
    }


        public static double MostrarMetasConcluidas(int idCliente)
        {
            using (SqlConnection conn = new SqlConnection(Banco.Conexao))
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM Estudo WHERE id_cliente = @id_cliente AND concluido = 1";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_cliente", idCliente);
                    object resultado = cmd.ExecuteScalar();

                    if (resultado != null && resultado != DBNull.Value)
                    {
                        return Convert.ToDouble(resultado);
                    }

                    return 0;
                }
            }
        }
        public static double MostrarTodasMetas(int idCliente)
        {
            using (SqlConnection conn = new SqlConnection(Banco.Conexao))
            {
                conn.Open();

                string sql = "SELECT COUNT(*) FROM Estudo WHERE id_cliente = @id_cliente";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id_cliente", idCliente);
                    object resultado = cmd.ExecuteScalar();

                    if (resultado != null && resultado != DBNull.Value)
                    {
                        return Convert.ToDouble(resultado);
                    }

                    return 0;
                }
            }
        }

}