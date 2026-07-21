namespace Init_db;

/// <summary>
/// Centraliza os métodos de validação das entradas informadas pelo usuário.
/// </summary>
public class Validacao
{

    /// <summary>
    /// Solicita um texto obrigatório ao usuário.
    /// Caso o campo seja deixado em branco, exibe uma mensagem de erro
    /// e permite uma nova tentativa.
    /// </summary>
    /// <param name="mensagemExibida">Mensagem apresentada ao usuário.</param>
    /// <param name="respostaLoop">Controla se o usuário deseja tentar novamente.</param>
    /// <returns>Retorna o texto informado ou null caso a entrada seja inválida.</returns>
    public static string EntradaObrigatoria(string mensagemExibida, ref string respostaLoop)
    {
        System.Console.WriteLine(mensagemExibida);
        string entrada = Console.ReadLine()!;

        if (string.IsNullOrWhiteSpace(entrada))
        {
            Mensagens.Erro_CampoVazio();
            System.Console.WriteLine("------------------------------------------");
            System.Console.WriteLine("\nDeseja tentar novamente? (s/n)");
            respostaLoop = Console.ReadLine()!.ToLower();
            return null!;
        }

        return entrada;
    }


    /// <summary>
    /// Solicita um número inteiro ao usuário e valida a entrada.
    /// Caso o valor informado seja inválido, permite uma nova tentativa.
    /// </summary>
    /// <param name="mensagemExibida">Mensagem apresentada ao usuário.</param>
    /// <param name="respostaLoop">Controla se o usuário deseja tentar novamente.</param>
    /// <returns>Retorna o número informado ou null caso a entrada seja inválida.</returns>
    public static int? IntVazio(string mensagemExibida, ref string respostaLoop)
    {
        System.Console.WriteLine(mensagemExibida);
        string entradaTexto = Console.ReadLine()!;

        if (entradaTexto == null)
        {
            Mensagens.Erro_CampoVazio();
            System.Console.WriteLine("------------------------------------------");
            System.Console.WriteLine("\nDeseja tentar novamente? (s/n)");
            respostaLoop = Console.ReadLine()!.ToLower();
            return null!;
        }
        if (int.TryParse(entradaTexto, out int numeroConvertido))
        {
            return numeroConvertido;
        }
        else
        {
            Mensagens.Erro_NumeroInvalido();
            System.Console.WriteLine("------------------------------------------");
            System.Console.WriteLine("\nDeseja tentar novamente? (s/n)");
            respostaLoop = Console.ReadLine()!.ToLower();
            return null;
        }
    }
}