namespace Init_db;

using Spectre.Console;
using Microsoft.Data.SqlClient;

/// <summary>
/// Responsável pelo gerenciamento, exibição e manipulação das categorias e suas metas associadas.
/// </summary>
public class GerenciaCategorias
{
    /// <summary>
    /// Inicia a interface interativa de gerenciamento da categoria selecionada.
    /// </summary>
    /// <param name="id_categoria">O identificador único da categoria.</param>
    /// <param name="id_cliente">O identificador único do cliente.</param>
    public static void IniciarCategoria(int id_categoria, int id_cliente)
    {
        bool Sair = false;
        Estudo estudo = new Estudo();
        Estudo estudo1 = new Estudo();
        while (!Sair)
        {
            using (SqlConnection conn = new SqlConnection(Banco.Conexao))
            {
                conn.Open();
                string sql = @"
SELECT *
FROM Categoria WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@id", id_categoria);
                    using (var Reader = cmd.ExecuteReader())
                    {

                        string opcao = "";
                        Interface.LimparTelaGeral();
                        Interface.Titulo("SUA CATEGORIA");

                        AnsiConsole.Write($"\n{Textos.SobreCategoria}\n\n");

                        if (Reader.Read())

                        {
                            string nomeCategoria = Reader["nome"].ToString()!;
                            AnsiConsole.Write(
                            GerenciaCategorias.InformacoesCategoria(nomeCategoria, id_categoria));

                            opcao = AnsiConsole.Prompt(
                                        new SelectionPrompt<string>()
                                        .Title("\n[#D3CCC7]─────────────────────────────────[/]\n[#D3CCC7]             OPÇÕES[/]\n[#D3CCC7]─────────────────────────────────[/]")
                                        .HighlightStyle(new Style(foreground: Color.FromHex($"{Cores.Opcoes}")))
                                        .AddChoices("Escolher uma meta", "Adicionar meta à categoria", "Remover meta", "Alterar nome da categoria", "Excluir categoria", "Sair")
                                        .HighlightStyle(new Style(
                                         foreground: Color.FromHex($"{Cores.Opcoes}"), decoration: Decoration.Bold
                            )));

                            switch (opcao)
                            {
                                case "Escolher uma meta":
                                    int idEscolhido_adicionar = estudo.EscolherEstudo(id_cliente, id_categoria: id_categoria, filtro: Estudo.TipoFiltroEstudo.Todas);
                                    if (idEscolhido_adicionar != -1)
                                    {
                                        GerenciaMetas.IniciarEstudo(id_cliente, idEscolhido_adicionar);
                                    }
                                    break;

                                case "Adicionar meta à categoria":
                                    ListarEstudo.MostrarMetas(id_cliente, true, id_categoria);
                                    break;

                                case "Remover meta":
                                    int idEscolhido_remover = estudo.EscolherEstudo(id_cliente, id_categoria: id_categoria, filtro: Estudo.TipoFiltroEstudo.Todas);
                                    if (idEscolhido_remover != -1)
                                    {
                                        Categoria.RemoverMeta(idEscolhido_remover);
                                    }
                                    break;

                                case "Alterar nome da categoria":
                                    Categoria.AtualizarTitulo(id_categoria);
                                    break;

                                case "Excluir categoria":
                                    if (Categoria.ApagarCategoria(id_categoria))
                                    {
                                        Sair = true;
                                    }
                                    break;

                                case "Sair":
                                    Sair = true;
                                    break;

                            }
                        }
                        else
                        {

                        }
                    }
                }

            }
        }
    }

    /// <summary>
    /// Consulta as estatísticas da categoria no banco de dados e gera um painel formatado para exibição no console.
    /// </summary>
    /// <param name="NomeCategoria">O nome da categoria.</param>
    /// <param name="idCategoria">O identificador da categoria.</param>
    /// <returns>Um painel do Spectre.Console contendo as métricas de metas da categoria ou um painel de erro.</returns>
    public static Panel InformacoesCategoria(string NomeCategoria, int idCategoria)
    {
        using (SqlConnection conn = new SqlConnection(Banco.Conexao))
        {
            conn.Open();

            string sql = @"
    SELECT
    COUNT(*) AS TotalMetas,
    ISNULL(SUM(CASE WHEN concluido = 0 THEN 1 ELSE 0 END), 0) AS Pendentes,
    ISNULL(SUM(CASE WHEN concluido = 1 THEN 1 ELSE 0 END), 0) AS Concluidas,
    ISNULL(SUM(minutos_estudados), 0) AS TempoTotal
FROM Estudo
WHERE id_categoria = @idCategoria";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@idCategoria", idCategoria);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {

                        int totalMetas = Convert.ToInt32(reader["TotalMetas"]);
                        int pendentes = Convert.ToInt32(reader["Pendentes"]);
                        int concluidas = Convert.ToInt32(reader["Concluidas"]);
                        int tempo = Convert.ToInt32(reader["TempoTotal"]);

                        int tamanhoTitulo = $" Categoria: {NomeCategoria} ".Length;
                        string espacamento = new string(' ', Math.Max(30, tamanhoTitulo));

                        string textoPainel =
                $"[bold]Metas criadas: [/] {totalMetas}\n" +
                $"[bold]Pendentes: [/] {pendentes}\n" +
                $"[bold]Concluídas: {concluidas}[/] \n" +
                $"[bold]Tempo total: {tempo}[/] \n" +
                espacamento;

                        var painelEstudante = new Panel(textoPainel)

                        .Border(BoxBorder.Rounded)
                        .BorderColor(Color.FromHex($"{Cores.Opcoes}"))
.Header($"[#EF0606] Categoria: {NomeCategoria} [/]", Justify.Left);

                        return painelEstudante;
                    }
                }
            }
        }
        return new Panel("[red]Não foi possível carregar as informações da categoria.[/]");


    }
}