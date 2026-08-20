namespace Init_db;

using BCrypt.Net;

/// <summary>
/// Prover métodos utilitários para criptografia e verificação de senhas utilizando a biblioteca BCrypt.
/// </summary>
public class Hash
{
    /// <summary>
    /// Gera um hash criptográfico seguro a partir de uma senha em texto plano.
    /// </summary>
    /// <param name="senha">A senha em texto plano a ser criptografada.</param>
    /// <returns>A string contendo o hash gerado com o fator de trabalho configurado.</returns>
    public static string CriaHash(string senha)
    {
        // Gera o hash da senha utilizando o algoritmo aprimorado do BCrypt com fator de custo (work factor) igual a 11.
        string senhaHash = BCrypt.EnhancedHashPassword(senha, 11);
        return senhaHash;
    }

    /// <summary>
    /// Verifica se uma senha em texto plano corresponde a um hash previamente salvo.
    /// </summary>
    /// <param name="senha">A senha em texto plano informada pelo usuário.</param>
    /// <param name="resultado">O objeto que contém o hash salvo no banco de dados.</param>
    /// <returns><c>true</c> se a senha for válida e corresponder ao hash; caso contrário, <c>false</c>.</returns>
    public static bool VerificaHash(string senha, object resultado)
    {
        // Converte o objeto recuperado (ex: retorno de consulta do banco) para string contendo o hash.
        string senhaHashSalva = resultado.ToString()!;

        // Compara a senha digitada com o hash armazenado para validar a autenticação.
        bool senhaValida = BCrypt.EnhancedVerify(senha, senhaHashSalva);

        return senhaValida;
    }
}