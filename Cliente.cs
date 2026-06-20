namespace Init_db;

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


                System.Console.WriteLine("Digite seu nome completo: ");
                string nome_completo = Console.ReadLine()!;

                System.Console.WriteLine("Digite seu nome email: ");
                string email = Console.ReadLine()!;

                System.Console.WriteLine("Crie seu usuário: ");
                string usuario = Console.ReadLine()!;

                System.Console.WriteLine("Digite sua senha: ");
                string senha = Console.ReadLine()!;

                cmd.Parameters.AddWithValue("@nome_completo", nome_completo);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@usuario", usuario);
                cmd.Parameters.AddWithValue("@senha", senha);

                Console.WriteLine("CLIENTE SALVO!!!!");

                int id_gerado = (int)cmd.ExecuteScalar();
                return id_gerado;

            }
        }

    }
}