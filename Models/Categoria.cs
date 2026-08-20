namespace Init_db;

using System.Formats.Asn1;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Spectre.Console;

/// <summary>
/// Gerencia as operações de CRUD e vinculação associadas às categorias de estudo.
/// </summary>
public class Categoria
{
    /// <summary>
    /// Exibe um menu interativo com as categorias cadastradas do cliente e retorna o ID da categoria selecionada.
    /// </summary>
    /// <param name="id_cliente">O identificador do cliente proprietário das categorias.</param>
    /// <returns>O identificador da categoria selecionada ou -1 se o usuário escolher sair.</returns>
    public static int MostrarCategorias(int id_cliente)
    {
        // Inicializa a conexão com o banco de dados SQL Server.
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            // Consulta para buscar todas as categorias pertencentes ao cliente informado.
            string sql = "SELECT * FROM Categoria WHERE id_cliente = @id_cliente";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Prepara o terminal para exibir as opções da interface.
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SUAS CATEGORIAS CRIADAS");
                    AnsiConsole.MarkupLine($"\n{Textos.SelecionarCategoria}");
                    
                    // Dicionário para mapear os IDs das categorias com seus respectivos nomes.
                    var estudos = new Dictionary<int, string>();

                    // Criação da caixa de seleção no terminal via Spectre.Console.
                    var menu = new SelectionPrompt<string>()
                        .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
                        .HighlightStyle(new Style(
                            foreground: Color.FromHex($"{Cores.Opcoes}"),
                            decoration: Decoration.Bold));

                    // Adiciona opção padrão de saída no menu.
                    menu.AddChoice("Sair");

                    // Itera sobre o leitor do banco preenchendo o dicionário e as opções do menu.
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["id"]);
                        string titulo = reader["nome"].ToString()!;

                        estudos.Add(id, titulo);
                        menu.AddChoice(titulo);
                    }

                    // Exibe o menu na tela e aguarda a interação do usuário.
                    string resposta = AnsiConsole.Prompt(menu);

                    // Retorna -1 se a escolha for cancelar/sair.
                    if (resposta == "Sair")
                        return -1;

                    // Procura o ID equivalente ao nome da categoria selecionada.
                    int idEscolhido = estudos
                        .First(x => x.Value == resposta)
                        .Key;

                    return idEscolhido;
                }
            }
        }
    }

    /// <summary>
    /// Solicita o nome de uma nova categoria ao usuário e insere o registro no banco de dados.
    /// </summary>
    /// <param name="id_cliente">O identificador do cliente para o qual a categoria será atribuída.</param>
    /// <returns>Retorna -1 após finalizar o fluxo da operação.</returns>
    public static int CriarCategoria(int id_cliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            // Instrução para inserir o nome da nova categoria associada ao cliente.
            string sql = "INSERT INTO Categoria (nome, id_cliente) VALUES (@nome, @id_cliente)";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // Limpa os parâmetros do comando SQL antes da execução.
                cmd.Parameters.Clear();
                Interface.LimparTelaGeral();
                Interface.Titulo("CRIE UMA NOVA CATEGORIA");
                AnsiConsole.MarkupLine($"\n{Textos.Categoria}\n");

                AnsiConsole.MarkupLine("[#D3CCC7]─────────────────────────────────[/]\n");

                // Solicita o título da nova categoria via input interativo no terminal.
                var nome = AnsiConsole.Ask<string>("\nCrie um título para a sua nova categoria: ");

                // Adiciona os valores informados aos parâmetros do comando SQL.
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@id_cliente", id_cliente);

                // Executa a instrução no banco de dados.
                cmd.ExecuteScalar();
                Mensagens.CriarCategoria();
            }
            return -1;
        }
    }

    /// <summary>
    /// Associa uma categoria existente a uma meta de estudo específica.
    /// </summary>
    /// <param name="id_categoria">O identificador da categoria que será vinculada.</param>
    /// <param name="id_estudo">O identificador da meta de estudo que receberá o vínculo.</param>
    public static void VincularCategoria(int id_categoria, int id_estudo)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();
            // Comando para atualizar a chave estrangeira da categoria na tabela de Estudo.
            string sql = @"
UPDATE Estudo
SET id_categoria = @id_categoria
WHERE id = @id";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                // Limpa parâmetros antigos e prepara a interface do usuário.
                cmd.Parameters.Clear();
                Interface.LimparTelaGeral();

                // Associa as variáveis aos parâmetros da consulta SQL.
                cmd.Parameters.AddWithValue("@id_categoria", id_categoria);
                cmd.Parameters.AddWithValue("@id", id_estudo);

                // Executa a atualização na tabela.
                cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Recupera o nome da categoria associada a uma meta de estudo específica.
    /// </summary>
    /// <param name="idEstudo">O identificador da meta de estudo pesquisada.</param>
    /// <returns>O nome da categoria associada ou "Sem categoria" caso não haja vínculo.</returns>
    public static string NomeCategoria(int idEstudo)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            // Consulta que junta a tabela de Estudo com a Categoria para extrair o nome da categoria.
            string sql = @"
SELECT C.nome
FROM Estudo E
LEFT JOIN Categoria C
    ON E.id_categoria = C.id
WHERE E.id = @idEstudo";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idEstudo", idEstudo);

                // Executa o comando e captura a primeira coluna do primeiro resultado.
                object? resultado = cmd.ExecuteScalar();

                // Valida se o retorno é nulo ou DBNull antes de converter.
                if (resultado == null || resultado == DBNull.Value)
                {
                    return "Sem categoria";
                }

                return resultado.ToString()!;
            }
        }
    }

    /// <summary>
    /// Exibe as metas associadas a uma categoria e permite ao usuário optar por iniciar um estudo.
    /// </summary>
    /// <param name="id_categoria">O identificador da categoria cujas metas serão buscadas.</param>
    /// <param name="id_cliente">O identificador do cliente atual.</param>
    public static void EscolherMetaCategoria(int id_categoria, int id_cliente)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            // Controla a validação de existência de registros na verificação de exibição.
            bool MetaEncontrada = false;
            conn.Open();

            // Consulta todas as metas de estudo associadas ao ID da categoria informada.
            string sql = @"
SELECT *
FROM Estudo
WHERE id_categoria = @id_categoria";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@id_categoria", id_categoria);
                using (var Reader = cmd.ExecuteReader())
                {
                    Interface.LimparTelaGeral();
                    Interface.Titulo("SEUS PLANOS DE ESTUDO");
                    try
                    {
                        // Renderiza e retorna uma tabela visual com as metas obtidas do leitor.
                        var tabela = GerenciaMetas.MostrarMetas(Reader, out MetaEncontrada);

                        AnsiConsole.Write(tabela);
                        // Caso existam metas, solicita se o usuário deseja iniciar o fluxo de estudos.
                        if (MetaEncontrada)
                        {
                            if (Mensagens.IniciarEstudo() == "Sim")
                            {
                                Estudo estudo = new Estudo();
                                estudo.EscolherEstudo(id_cliente);
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
                    catch
                    {
                        // Exibe mensagem amigável caso haja erro de processamento das informações.
                        Mensagens.Erro_SemInformacoes();
                    }
                }
            }
        }
    }

    /// <summary>
    /// Remove uma categoria do banco de dados e desvincula as metas associadas a ela.
    /// </summary>
    /// <param name="id_categoria">O identificador da categoria que será excluída.</param>
    /// <returns><c>true</c> se o processo for concluído ou cancelado sem erros; <c>false</c> se falhar.</returns>
    public static bool ApagarCategoria(int id_categoria)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            Interface.LimparTelaGeral();
            conn.Open();

            Interface.Titulo("APAGUE SUA CATEGORIA");

            // Exibe o prompt de confirmação antes de remover do banco.
            string resposta = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Deseja apagar a categoria? As metas vinculadas ficarão sem categoria.")
                .AddChoices("Deletar categoria", "Cancelar")
                .HighlightStyle(new Style(
                    foreground: Color.FromHex($"{Cores.Opcoes}"),
                    decoration: Decoration.Bold)));

            if (resposta == "Deletar categoria")
            {
                try
                {
                    // Define o campo id_categoria como NULL em todas as metas vinculadas antes da exclusão.
                    string sqlUpdate = "UPDATE Estudo SET id_categoria = NULL WHERE id_categoria = @id";
                    using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@id", id_categoria);
                        cmdUpdate.ExecuteNonQuery();
                    }

                    // Executa o comando de exclusão do registro da categoria.
                    string sqlDelete = "DELETE FROM Categoria WHERE id = @id";
                    using (SqlCommand cmdDelete = new SqlCommand(sqlDelete, conn))
                    {
                        cmdDelete.Parameters.AddWithValue("@id", id_categoria);

                        int linhasAfetadas = cmdDelete.ExecuteNonQuery();

                        // Verifica se a exclusão foi bem-sucedida.
                        if (linhasAfetadas > 0)
                        {
                            Mensagens.Sucesso_FinalizarApagarMeta("apagada");
                            return true;
                        }

                        Mensagens.Erro_PlanoNaoEncontrado(id_categoria);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // Exibe tratamento formatado de exceção caso falhe no banco.
                    AnsiConsole.WriteException(ex);
                    Console.ReadKey();
                    return false;
                }
            }
        }

        // Retorna cancelado se o usuário escolheu a opção "Cancelar".
        Mensagens.Erro_Cancelada();
        return true;
    }

    /// <summary>
    /// Remove o vínculo de categoria de uma meta de estudo específica.
    /// </summary>
    /// <param name="id_estudo">O identificador do estudo que terá sua categoria removida.</param>
    /// <returns><c>true</c> se a remoção for efetuada ou cancelada; <c>false</c> em caso de exceção.</returns>
    public static bool RemoverMeta(int id_estudo)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            Interface.LimparTelaGeral();
            conn.Open();

            Interface.Titulo("REMOVER META");

            // Confirmação para desvincular a meta da categoria atual.
            string resposta = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Deseja remover a meta da categoria?")
                .AddChoices("Remover categoria", "Cancelar")
                .HighlightStyle(new Style(
                    foreground: Color.FromHex($"{Cores.Opcoes}"),
                    decoration: Decoration.Bold)));

            if (resposta == "Remover categoria")
            {
                try
                {
                    // Define id_categoria como NULL na meta especificada.
                    string sqlUpdate = "UPDATE Estudo  SET id_categoria = NULL WHERE id = @id";
                    using (SqlCommand cmdUpdate = new SqlCommand(sqlUpdate, conn))
                    {
                        cmdUpdate.Parameters.AddWithValue("@id", id_estudo);
                        cmdUpdate.ExecuteNonQuery();
                        Mensagens.RemoverMeta();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    // Exibe a exceção detalhada em caso de falha.
                    AnsiConsole.WriteException(ex);
                    Console.ReadKey();
                    return false;
                }
            }
        }

        Mensagens.Erro_Cancelada();
        return true;
    }

    /// <summary>
    /// Atualiza o nome de uma categoria cadastrada no banco de dados.
    /// </summary>
    /// <param name="id">O identificador da categoria a ser modificada.</param>
    /// <returns>Retorna -1 ao encerrar o fluxo do método.</returns>
    public static int AtualizarTitulo(int id)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            string resposta = "";
            do
            {
                conn.Open();
                Interface.LimparTelaGeral();
                Interface.Titulo("ATUALIZE A SUA CATEGORIA");

                // Solicita a entrada com o novo nome para a categoria.
                var titulo = AnsiConsole.Ask<string>("Digite o novo título da sua categoria: ");

                // Query SQL responsável por modificar o nome da categoria com base no ID.
                string sql = "UPDATE Categoria SET nome = @nome WHERE id = @id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@nome", titulo);

                    // Executa a instrução e verifica a quantidade de linhas afetadas.
                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                    {
                        Mensagens.AtualizarNomeCatetoria();
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
}