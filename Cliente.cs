namespace Init_db;


using Microsoft.Data.SqlClient;

/// <summary>
/// Gerencia as operações relacionadas aos usuários do sistema,
/// como cadastro, autenticação e consulta de informações.
/// </summary>
public class Cliente
{
    
    /// <summary>
    /// Realiza o cadastro de um novo usuário no banco de dados.
    /// </summary>
    /// <returns>
    /// Retorna o identificador do usuário cadastrado ou -1 caso o cadastro não seja concluído.
    /// </returns>
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
                    // Remove parâmetros da tentativa anterior antes de reutilizar o mesmo comando SQL.
                    cmd.Parameters.Clear();
                    Interface.LimparTelaGeral();
                    Interface.EscreverCentralizado("ATHENA -  Crie seu cadastro ");

                    string nome_completo = Validacao.EntradaObrigatoria("Digite seu nome completo: ", ref resposta);
                    if (nome_completo == null) continue;
                    string email = Validacao.EntradaObrigatoria("Digite seu email: ", ref resposta);
                    if (email == null) continue;
                    string usuario = Validacao.EntradaObrigatoria("Crie seu usuário: ", ref resposta);
                    if (usuario == null) continue;
                    string senha = Validacao.EntradaObrigatoria("Crie sua senha: ", ref resposta);
                    if (senha == null) continue;

                    cmd.Parameters.AddWithValue("@nome_completo", nome_completo);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@senha", senha);
                    try
                    {
                        int id_gerado = (int)cmd.ExecuteScalar();
                        Mensagens.Sucesso_CadastroSucesso();
                        return id_gerado;
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 2627)
                        {
                            Mensagens.Erro_InformacoesInvalidas();
                            System.Console.WriteLine("Tentar novamente? (s/n)");
                            resposta = Console.ReadLine()!;
                            if (resposta != "s")
                            {
                                return -1;
                            }

                            continue;
                        }

                        throw;

                    }

                } while (resposta == "s");

            }
            return -1;
        }
    }


    /// <summary>
    /// Valida as credenciais informadas e realiza a autenticação do usuário.
    /// </summary>
    /// <returns>
    /// Retorna o identificador do usuário autenticado ou -1 caso o login não seja realizado.
    /// </returns>
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
                Interface.LimparTelaGeral();
                Interface.EscreverCentralizado("ATHENA -  Faça o seu cadastro ");

                string Login = Validacao.EntradaObrigatoria("Digite seu usuário: ", ref resposta);
                if (Login == null) continue;
                string Senha = Validacao.EntradaObrigatoria("Digite sua senha: ", ref resposta);
                if (Senha == null) continue;


                string sql = @" SELECT id FROM Cliente WHERE usuario = @usuario AND senha = @senha";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", Login);
                    cmd.Parameters.AddWithValue("@senha", Senha);

                    object resultado = cmd.ExecuteScalar();

                    if (resultado != null)
                    {
                        Mensagens.Sucesso_LoginSucesso();
                        return (int)resultado;
                    }
                    Mensagens.Erro_LoginErro();
                    System.Console.WriteLine("Tentar novamente?(s/n)");
                    resposta = Console.ReadLine()!;
                }
            } while (resposta == "s");
        }
        return -1;
    }


    /// <summary>
    /// Obtém o nome do usuário a partir do identificador informado.
    /// </summary>
    /// <param name="id">Identificador do usuário.</param>
    /// <returns>Nome completo do usuário.</returns>
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


    /// <summary>
    /// Retorna o tempo total de estudo acumulado pelo usuário.
    /// </summary>
    /// <param name="idCliente">Identificador do usuário.</param>
    /// <returns>Total de minutos estudados.</returns>
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


    /// <summary>
    /// Retorna a quantidade de metas pendentes do usuário.
    /// </summary>
    ///<param name="idCliente">Identificador do usuário.</param>
    public static double ContarMetasPendentes(int idCliente)
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


    /// <summary>
    /// Retorna a quantidade de metas concluídas do usuário.
    /// </summary>
    /// <param name="idCliente">Identificador do usuário.</param>
    public static double ContarMetasConcluidas(int idCliente)
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


    /// <summary>
    /// Retorna a quantidade total de metas cadastradas pelo usuário.
    /// </summary>
    /// <param name="idCliente">Identificador do usuário.</param>
    public static double ContarTodasMetas(int idCliente)
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