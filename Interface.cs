using System.Data;
using System.Data.Common;
using System.Runtime.ConstrainedExecution;

namespace Init_db;

public class Interface
{
    public static void Titulo(string Titulo)
    {
        Console.Clear();
        System.Console.WriteLine("===========================================================================");
        Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine($"            === {Titulo}===                             ");
        Console.ResetColor();
        System.Console.WriteLine("===========================================================================\n");
    }
    public static void MenuPrincipal()
    {
        Cliente cliente = new Cliente();
        bool ClienteLogado = false;
        int id = -1;
        int opcao = 0;
        while (!ClienteLogado)
        {
            bool entradaValida = false;

            Interface.Titulo("ATHENA - Sistema de gerenciamento de estudo");
            System.Console.WriteLine("Bem-vindo ao Athena!\n");
            System.Console.WriteLine("Inspirado em Atena, a deusa grega da sabedoria, estratégia e conhecimento,\no Athenas foi criado para auxiliar estudantes na organização dos estudos,\nno acompanhamento do progresso e na construção de uma jornada de aprendizado \nmais eficiente e disciplinada.\n");

            System.Console.WriteLine("==================================");
            System.Console.WriteLine("             OPÇÔES               ");
            System.Console.WriteLine("==================================\n");
            do
            {
                System.Console.WriteLine("[1] - Criar cadastro");
                System.Console.WriteLine("[2] - Fazer cadastro");
                System.Console.WriteLine("[3] - Sair");
                System.Console.WriteLine("Digite a opção escolhida: ");

                if (int.TryParse(Console.ReadLine(), out opcao) && opcao >= 1 && opcao <= 3)
                {
                    entradaValida = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Digite um número válido!");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            } while (!entradaValida);

            switch (opcao)
            {
                case 1:
                    cliente.CadastrarCliente();
                    id = cliente.FazerLogin();

                    if (id != -1)
                    {
                        Interface.InterfaceLogin(id);
                    }
                    break;
                case 2:
                    id = cliente.FazerLogin();

                    if (id != -1)
                    {
                        Interface.InterfaceLogin(id);
                    }
                    break;
                case 3:
                    Environment.Exit(0);
                    System.Console.WriteLine("Obrigado por usar!!!");
                    break;
            }
        }
    }

    public static void InterfaceLogin(int id)
    {
        Cliente cliente = new Cliente();
        Estudo estudo = new Estudo();
        double TempoEstudo = Cliente.MostrarTempoTotalEstudo(id);
        double MetasPendentes = Cliente.MostrarMetasPendentes(id);
        double MetasConcluidas = Cliente.MostrarMetasConcluidas(id);
        double TotalMetas = Cliente.MostrarTodasMetas(id);
        bool entradaValida = false;
        bool Sair = false;
        int opcao = 0;
        string Nome = cliente.ObterNomeCliente(id);

        while (!Sair)
        {

            Console.Clear();
            System.Console.WriteLine("===================================================================");
            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("         === ATHENA - Painel do estudante ===        ");
            Console.ResetColor();
            System.Console.WriteLine("===================================================================\n\n");

            System.Console.WriteLine($"\nBem-vindo(a) {Nome}!!");

            System.Console.WriteLine("==================================");
            System.Console.WriteLine($"Seus minutos estudados: {TempoEstudo}");
            System.Console.WriteLine($"Metas criadas: {TotalMetas}");
            System.Console.WriteLine($"Metas pendentes: {MetasPendentes}");
            System.Console.WriteLine($"Metas concluídas: {MetasConcluidas}");
            System.Console.WriteLine("==================================");

            System.Console.WriteLine("\"O homem não é nada além daquilo que a educação faz dele\" - Imannuel Kant \n");
            System.Console.WriteLine("Continue sua jornada de conhecimento \ne transforme disciplina em resultados.\nCada minuto dedicado aos estudos é um passoa \nmais em direção aos seus objetivos.");


            System.Console.WriteLine("==================================");
            System.Console.WriteLine("             OPÇÔES               ");
            System.Console.WriteLine("==================================\n");
            do
            {
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Digite um número válido!");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            } while (!entradaValida);

            switch (opcao)
            {
                case 1:
                    estudo.CadastrarMeta(id);
                    break;
                case 2:
                    Interface.MostrarOpcoesMetas(id);
                    break;
                case 3:
                    double minutos = Cronometro.ContarTempo();
                    Cronometro.AtualizarTempoTotalCliente(id, minutos);
                    break;
                case 4:
                    Sair = true;
                    break;

            }
        }
    }
    public static void PersoanlizarMetas(int id_estudo)
    {
        int opcao = 0;
        bool entradaValida = false;
        bool Sair = false;
        while (!Sair)
        {
            Console.Clear();
            System.Console.WriteLine("===================================================================");
            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("         === ATHENA - Personalize suas metas de estudo ===        ");
            Console.ResetColor();
            System.Console.WriteLine("===================================================================\n\n");

            System.Console.WriteLine("==================================");
            System.Console.WriteLine("             OPÇÔES               ");
            System.Console.WriteLine("==================================\n");
            do
            {
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Digite um número válido!");
                    Console.ResetColor();
                    Console.ReadKey();
                }
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
                    Sair = true;
                    break;
            }
        }
    }
    public static void MostrarOpcoesMetas(int id)
    {
        int opcao = 0;
        bool entradaValida = false;
        bool Sair = false;
        while (!Sair)
        {
            Interface.Titulo("ATHENA - Filtrar metas");

            System.Console.WriteLine("==================================");
            System.Console.WriteLine("             OPÇÔES               ");
            System.Console.WriteLine("==================================\n");
            do
            {
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Digite um número válido!");
                    Console.ResetColor();
                    Console.ReadKey();
                }
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
                    Sair = true;
                    break;

            }

        }
    }
}