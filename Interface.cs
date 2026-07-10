using System.Data.Common;
using System.Runtime.ConstrainedExecution;

namespace Init_db;

public class Interface
{
    public static void MenuPrincipal()
    {
        Cliente cliente = new Cliente();
        bool ClienteLogado = false;
        int id = -1;
        int opcao = 0;
        while (!ClienteLogado)
        {
            bool entradaValida = false;
            Console.Clear();
            System.Console.WriteLine("===================================================================");
            Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("         === ATHENA - Sistema de gerenciamento de estudo===        ");
            Console.ResetColor();
            System.Console.WriteLine("===================================================================\n");
            System.Console.WriteLine("Bem-vindo ao Athena!\n");
            System.Console.WriteLine("Inspirado em Atena, a deusa gnrega da sabedoria, estratégia e conhecimento,\no Athenas foi criado para auxiliar estudantes na organização dos estudos,\nno acompanhamento do progresso e na construção de uma jornada de aprendizado \nmais eficiente e disciplinada.\n");

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
            System.Console.WriteLine("==================================");

            System.Console.WriteLine("\"O homem não é nada além daquilo que a educação faz dele\" - Imannuel Kant \n");
            System.Console.WriteLine("Continue sua jornada de conhecimento \ne transforme disciplina em resultados.\nCada minuto dedicado aos estudos é um passoa \nmais em direção aos seus objetivos.");


            System.Console.WriteLine("==================================");
            System.Console.WriteLine("             OPÇÔES               ");
            System.Console.WriteLine("==================================\n");
            do
            {
                System.Console.WriteLine("[1] - Criar nova meta");
                System.Console.WriteLine("[2] - Mostrar todas as metas cadastradas");
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
                    Estudo.MostrarMetas(id);
                    break;
                case 3:
                double minutos = Cronometro.ContarTempo();
                Cronometro.AtualizarTempoTotalCliente(id,minutos);
                    break;
                case 4:
                    Sair = true;
                    break;

            }
        }
    }
}
