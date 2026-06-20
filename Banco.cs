using Microsoft.Data.SqlClient;

namespace Init_db;

public static class Banco
{
    public static string Conexao = 
        @"Server=.\SQLEXPRESS;Database=StudyTrackerDB;Trusted_Connection=True;TrustServerCertificate=True;";
}
