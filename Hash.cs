namespace Init_db;

using BCrypt.Net;

public class Hash
{
    public static string CriaHash(string senha)
    {
        string senhaHash = BCrypt.EnhancedHashPassword(senha, 11);
        return senhaHash;
    }
    public static bool VerificaHash(string senha, object resultado)
    {

        string senhaHashSalva = resultado.ToString()!;

        bool senhaValida = BCrypt.EnhancedVerify(senha, senhaHashSalva);

        return senhaValida;
    }
}