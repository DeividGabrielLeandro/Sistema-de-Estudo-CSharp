using System.Data;
using System.Data.Common;
using System.Runtime.ConstrainedExecution;

namespace Init_db;

public class Interface
{
    /// <summary>
    /// Move o cursor para o início do terminal e limpa completamente a tela,
    /// incluindo o histórico visível do console.
    /// </summary>
    public static void LimparTelaGeral()
    {
        Console.SetCursorPosition(0, 0);
        Console.Write("\x1b[3J");
        Console.Clear();
    }


    /// <summary>
    /// Exibe um título centralizado entre linhas de separação,
    /// padronizando o cabeçalho das telas do sistema.
    /// </summary>
    /// <param name="titulo">Texto que será exibido como título.</param>
    public static void EscreverCentralizado(string titulo)
    {
        string linha = new string('=', 75);
        string texto = $"=== {titulo} ===";

        int espacos = Math.Max(0, (linha.Length - texto.Length) / 2);

        Console.WriteLine(linha);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(new string(' ', espacos) + texto);
        Console.ResetColor();

        Console.WriteLine(linha);
        Console.WriteLine();
    }


    /// <summary>
    /// Exibe informações sobre o projeto ATHENA.
    /// </summary>
    public static void SobreAthena()
    {
        LimparTelaGeral();
        EscreverCentralizado("ATHENA - Sobre o Projeto");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(Textos.Sobre);
        Console.ResetColor();

        Mensagens.Sair();
    }


    /// <summary>
    /// Exibe o menu principal do sistema e direciona o usuário
    /// para as funcionalidades disponíveis.
    /// </summary>
    public static void MenuPrincipal()
    {
        Cliente cliente = new Cliente();
        bool clienteLogado = false;
        int id = -1;
        int opcao = 0;
        while (!clienteLogado)
        {
            bool entradaValida = false;

            do
            {
                LimparTelaGeral();

                Interface.EscreverCentralizado("ATHENA - Sistema de gerenciamento de estudo");
                System.Console.WriteLine("Bem-vindo ao Athena!\n");
                System.Console.WriteLine(Textos.MensagemInicial);

                System.Console.WriteLine("==================================");
                System.Console.WriteLine("             OPÇÔES               ");
                System.Console.WriteLine("==================================\n");
                System.Console.WriteLine("[1] - Sobre o projeto");
                System.Console.WriteLine("[2] - Criar cadastro");
                System.Console.WriteLine("[3] - Fazer cadastro");
                System.Console.WriteLine("[4] - Sair");
                System.Console.WriteLine("Digite a opção escolhida: ");

                if (int.TryParse(Console.ReadLine(), out opcao) && opcao >= 1 && opcao <= 4)
                {
                    entradaValida = true;
                }
                else
                {
                    Mensagens.Erro_NumeroInvalido();
                    continue;
                }
                break;
            } while (!entradaValida);

            switch (opcao)
            {
                case 1:
                    Interface.SobreAthena();
                    break;
                case 2:
                    id = cliente.CadastrarCliente();
                    if (id != -1)
                    {
                        Interface.InterfaceLogin(id);
                    }
                    break;
                case 3:

                    id = cliente.FazerLogin();

                    if (id != -1)
                    {

                        Interface.InterfaceLogin(id);
                    }
                    break;
                case 4:
                    Environment.Exit(0);
                    System.Console.WriteLine("Obrigado por usar!!!");
                    break;
            }
        }
    }


    /// <summary>
    /// Exibe o painel principal do estudante, permitindo o acesso
    /// às funcionalidades disponíveis após a autenticação.
    /// </summary>
    /// <param name="id">Identificador do usuário autenticado.</param>
    public static void InterfaceLogin(int id)
    {
        Cliente cliente = new Cliente();
        Estudo estudo = new Estudo();
        double tempoEstudo = Cliente.MostrarTempoTotalEstudo(id);
        double metasPendentes = Cliente.ContarMetasPendentes(id);
        double metasConcluidas = Cliente.ContarMetasConcluidas(id);
        double totalMetas = Cliente.ContarTodasMetas(id);
        bool sair = false;
        int opcao = 0;
        string Nome = cliente.ObterNomeCliente(id);

        while (!sair)
        {
            bool entradaValida = false;

            do
            {
                LimparTelaGeral();
                EscreverCentralizado("ATHENA -  Painel do estudante");

                System.Console.WriteLine($"\nBem-vindo(a) {Nome}!!");

                System.Console.WriteLine("==================================");
                System.Console.WriteLine($"Seus minutos estudados: {tempoEstudo}");
                System.Console.WriteLine($"Metas criadas: {totalMetas}");
                System.Console.WriteLine($"Metas pendentes: {metasPendentes}");
                System.Console.WriteLine($"Metas concluídas: {metasConcluidas}");
                System.Console.WriteLine("==================================");

                System.Console.WriteLine(Textos.MensagemMotivacional_Kant);
                System.Console.WriteLine(Textos.MensagemMotivacional_Conhecimento);


                System.Console.WriteLine("==================================");
                System.Console.WriteLine("             OPÇÔES               ");
                System.Console.WriteLine("==================================\n");
                System.Console.WriteLine("[1] - Criar nova meta");
                System.Console.WriteLine("[2] - Abrir menu para mostrar as metas");
                System.Console.WriteLine("[3] - Iniciar um estudo livre");
                System.Console.WriteLine("[4] - Sair");
                System.Console.WriteLine("Digite a opção escolhida: ");

                if (int.TryParse(Console.ReadLine(), out opcao) && opcao >= 1 && opcao <= 4)
                {
                    entradaValida = true;
                }
                else
                {
                    Mensagens.Erro_NumeroInvalido();
                    continue;
                }
                break;
            } while (!entradaValida);


            switch (opcao)
            {
                case 1:
                    estudo.CadastrarMeta(id);

                    // Atualiza os indicadores exibidos no painel após o cadastro de uma nova meta.
                    totalMetas = Cliente.ContarTodasMetas(id);
                    metasPendentes = Cliente.ContarMetasPendentes(id);
                    break;
                case 2:
                    Interface.MostrarOpcoesMetas(id);
                    metasPendentes = Cliente.ContarMetasPendentes(id);
                    metasConcluidas = Cliente.ContarMetasConcluidas(id);
                    break;
                case 3:
                    double minutos = Cronometro.ContarTempo();

                    // Atualiza o total de minutos exibido ao usuário após a sessão de estudo.
                    Cronometro.AtualizarTempoTotalCliente(id, minutos);
                    tempoEstudo = Cliente.MostrarTempoTotalEstudo(id);
                    break;
                case 4:
                    sair = true;
                    break;

            }
        }
    }


    /// <summary>
    /// Exibe o menu de personalização de uma meta de estudo.
    /// </summary>
    /// <param name="id_estudo">Identificador da meta selecionada.</param>
    public static void PersonalizarMetas(int id_estudo)
    {
        int opcao = 0;
        bool entradaValida = false;
        bool sair = false;
        while (!sair)
        {
            do
            {
                LimparTelaGeral();
                EscreverCentralizado("ATHENA -  Personalize suas metas de estudo");

                System.Console.WriteLine("==================================");
                System.Console.WriteLine("             OPÇÔES               ");
                System.Console.WriteLine("==================================\n");
                System.Console.WriteLine("[1] - Atualizar título");
                System.Console.WriteLine("[2] - Atualizar descrição");
                System.Console.WriteLine("[3] - Atualizar tempo de meta");
                System.Console.WriteLine("[4] - Apagar meta");
                System.Console.WriteLine("[5] - Sair");
                System.Console.WriteLine("Digite a opção escolhida: ");
                if (int.TryParse(Console.ReadLine(), out opcao) && opcao >= 1 && opcao <= 5)
                {
                    entradaValida = true;
                }
                else
                {
                    Mensagens.Erro_NumeroInvalido();
                    continue;
                }
                break;
            } while (!entradaValida);

            switch (opcao)
            {
                case 1:
                    Estudo.AtualizarTitulo(id_estudo);
                    break;
                case 2:
                    Estudo.AtualizarDescricao(id_estudo);
                    break;
                case 3:
                    Estudo.AtualizarMeta(id_estudo);
                    break;
                case 4:
                    Estudo.ApagarMeta(id_estudo);
                    break;
                case 5:
                    sair = true;
                    break;
            }
        }
    }


    /// <summary>
    /// Exibe as opções de visualização e pesquisa das metas do usuário.
    /// </summary>
    /// <param name="id">Identificador do usuário.</param>
    public static void MostrarOpcoesMetas(int id)
    {
        int opcao = 0;
        bool entradaValida = false;
        bool sair = false;
        while (!sair)
        {
            do
            {
                LimparTelaGeral();
                EscreverCentralizado("ATHENA - Filtrar metas");

                System.Console.WriteLine("==================================");
                System.Console.WriteLine("             OPÇÔES               ");
                System.Console.WriteLine("==================================\n");
                System.Console.WriteLine("[1] - Ver todas as metas");
                System.Console.WriteLine("[2] - Mostrar apenas as metas concluídas");
                System.Console.WriteLine("[3] - Mostrar apenas as metas pendentes");
                System.Console.WriteLine("[4] - Pesquisar meta pelo título");
                System.Console.WriteLine("[5] - Sair");
                System.Console.WriteLine("Digite a opção escolhida: ");
                if (int.TryParse(Console.ReadLine(), out opcao) && opcao >= 1 && opcao <= 5)
                {
                    entradaValida = true;
                }
                else
                {
                    Mensagens.Erro_NumeroInvalido();
                    continue;
                }
                break;
            } while (!entradaValida);

            switch (opcao)
            {
                case 1:
                    Estudo.MostrarMetas(id);
                    break;
                case 2:
                    Estudo.MostrarMetasConcluidas(id);
                    break;
                case 3:
                    Estudo.MostrarMetasPendentes(id);
                    break;
                case 4:
                    System.Console.WriteLine("Pesquise: ");
                    string pesquisa = Console.ReadLine()!;
                    Estudo.PesquisarMeta(pesquisa, id);
                    break;
                case 5:
                    sair = true;
                    break;

            }

        }
    }
}