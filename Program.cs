using Init_db;
using Spectre.Console;

/// <summary>
/// Ponto de entrada (Entry Point) da aplicação.
/// Configura as cores do terminal, limpa a tela e exibe o menu principal do sistema.
/// </summary>

// Define a cor de fundo do terminal para preto usando sequências de escape ANSI
Console.Write("\x1b]11;#000000\x1b\\");

// Define a cor do texto do terminal para um tom claro (#EFEEE8) usando sequências de escape ANSI
Console.Write("\x1b]10;#EFEEE8\x1b\\");

// Garante o alinhamento e a limpeza completa do terminal padrão C#
Console.Clear();

// Exibe a interface do menu principal e inicia o fluxo do sistema
Interface.MenuPrincipal();