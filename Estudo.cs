namespace Init_db;

using Microsoft.Data.SqlClient;

public class Estudo
{
    public static void CadastrarMeta(int id_gerado)
    {
        Estudo estudo = new Estudo();
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            string sql = "INSERT INTO Estudo(titulo,descricao,meta_minutos,id_cliente) "+"VALUES (@titulo,@descricao,@meta_minutos,@id_cliente)";
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
}