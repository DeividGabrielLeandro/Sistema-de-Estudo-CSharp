namespace Init_db;

using System.Formats.Asn1;
using Microsoft.Data.SqlClient;
using Spectre.Console;

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
                    Interface.Titulo("CRIE SEU CADASTRO");

                    AnsiConsole.MarkupLine($"\n{Textos.MensagemCadastro}\n");
                    AnsiConsole.MarkupLine("[#D3CCC7]─────────────────────────────────[/]\n");

                    var nome_completo = AnsiConsole.Ask<string>("\nDigite seu nome: ");
                    var email = AnsiConsole.Ask<string>("\nDigite seu email: ");
                    var usuario = AnsiConsole.Ask<string>("\nCrie um usuário: ");
                    var senha = AnsiConsole.Ask<string>("\nCrie uma senha: ");

                    cmd.Parameters.AddWithValue("@nome_completo", nome_completo);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@senha", Hash.CriaHash(senha));
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
                            continue;
                        }

                        throw;

                    }

                } while (Mensagens.TentarNovamente(resposta) == "Tentar novamente");

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
            conn.Open();
            string resposta = "";
            bool usuarioEncontrado = false;
            string senhaHashSalva = "";
            do
            {
                Interface.LimparTelaGeral();
                Interface.Titulo("FAÇA O SEU CADASTRO");

                AnsiConsole.MarkupLine($"\n{Textos.MensagemLogin}\n");
                AnsiConsole.MarkupLine("[#D3CCC7]─────────────────────────────────[/]\n");


                var usuarioLogin = AnsiConsole.Ask<string>("\nDigite seu usuário: ");
                var senhaLogin = AnsiConsole.Prompt(
                                        new TextPrompt<string>("\nDigite sua senha: ")
                                            .Secret()
                                    );

                string sql = @"SELECT id, senha FROM Cliente WHERE usuario = @usuario";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@usuario", usuarioLogin);
                    using (SqlDataReader leitor = cmd.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            int idDoBanco = leitor.GetInt32(0);
                            senhaHashSalva = leitor.GetString(1);
                            usuarioEncontrado = true;
                        }
                    }
                    object resultado = cmd.ExecuteScalar();


                    if (usuarioEncontrado && Hash.VerificaHash(senhaLogin, senhaHashSalva))
                    {
                        Mensagens.Sucesso_LoginSucesso();
                        return (int)resultado;
                    }
                    else
                    {
                        Mensagens.Erro_LoginErro();
                    }
                }


            } while (Mensagens.TentarNovamente(resposta) == "Tentar novamente");
        }
        return -1;
    }
}

