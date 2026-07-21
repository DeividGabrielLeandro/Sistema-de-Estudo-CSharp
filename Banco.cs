using Microsoft.Data.SqlClient;

namespace Init_db;

/// <summary>
/// Centraliza a string de conexão utilizada para acessar
/// o banco de dados da aplicação.
/// </summary>
public static class Banco
{

    /// <summary>
    /// String de conexão com o banco de dados SQL Server.
    /// </summary>
    public static readonly string Conexao =
        @"Server=.\SQLEXPRESS;Database=StudyTrackerDB;Trusted_Connection=True;TrustServerCertificate=True;";
}
