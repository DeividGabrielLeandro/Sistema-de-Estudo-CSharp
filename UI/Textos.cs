namespace Init_db;

/// <summary>
/// Armazena os textos fixos utilizados nas telas do sistema,
/// centralizando as mensagens exibidas ao usuário.
/// </summary>
public class Textos
{
/// <summary>Texto com informações do roadmap e futuras atualizações do sistema.</summary>
public static readonly string Atualizacoes = @"# Próximas Atualizações

O ATHENA continuará evoluindo com foco em organização, produtividade e acompanhamento do desempenho nos estudos. O planejamento abaixo representa os próximos passos do projeto e poderá ser ajustado conforme o desenvolvimento avançar.

## ATHENA 1.1 — Finalização da Versão Terminal

* Finalizar as estatísticas básicas de estudo.
* Exibir tempo estudado hoje.
* Exibir tempo estudado na semana.
* Exibir tempo estudado no mês.
* Calcular média diária de estudo.
* Testar todas as funcionalidades da versão.
* Corrigir bugs encontrados durante os testes.
* Revisar e organizar o código.
* Revisar a estrutura do banco de dados.
* Finalizar a versão ATHENA 1.1.

## ATHENA 2.0 — Versão Aplicativo

* Migrar o ATHENA da interface de terminal para uma interface gráfica.
* Reestruturar as telas para uma experiência de aplicativo.
* Criar dashboard visual.
* Adaptar metas, categorias e sessões de estudo para a nova interface.
* Adaptar o cronômetro e o sistema de pausas.
* Adaptar as estatísticas de estudo.
* Melhorar a visualização do histórico de estudos.
* Implementar melhorias de usabilidade e navegação.
* Revisar a arquitetura do sistema durante a migração.

## Futuras versões

Após a versão Aplicativo, novas funcionalidades poderão ser desenvolvidas conforme as necessidades identificadas durante o uso do sistema, podendo incluir:

* Estatísticas e métricas mais avançadas.
* Relatórios de desempenho.
* Calendário de estudos.
* Sistema de sequência de estudos (Streak).
* Gamificação.
* Melhorias de segurança e autenticação.
* Exportação de dados e relatórios.
* Novos recursos de produtividade.

> **Observação:** O planejamento acima representa a direção atual do ATHENA. As funcionalidades e prioridades poderão ser alteradas durante o desenvolvimento conforme novas necessidades, ideias e aprendizados surgirem.";


    /// <summary>Texto de apresentação institucional e objetivos do projeto ATHENA.</summary>
    public static readonly string Sobre = @"
Inspirado em Atena, a deusa grega da sabedoria, estratégia e
conhecimento, o ATHENA é um sistema desenvolvido para auxiliar
estudantes na organização de seus estudos.

O projeto permite criar planos de estudo, acompanhar o tempo
dedicado a cada meta, registrar sessões de estudo livre,
editar objetivos e visualizar a evolução do aprendizado.

Mais do que um simples cronômetro, o ATHENA busca incentivar
a disciplina, a constância e o desenvolvimento de hábitos de
estudo saudáveis, tornando a jornada de aprendizado mais
organizada, produtiva e motivadora.

""O sucesso é a soma de pequenos esforços repetidos dia após dia.""

";
    /// <summary>Mensagem curta de apresentação inicial exibida na tela principal.</summary>
    public static readonly string MensagemInicial = @"
Inspirado em Atena, a deusa grega da sabedoria, estratégia e conhecimento,
o Athena foi criado para auxiliar estudantes na organização dos estudos,
no acompanhamento do progresso e na construção de uma jornada de aprendizado 
mais eficiente e disciplinada.";

    /// <summary>Frase motivacional de Immanuel Kant.</summary>
    public static readonly string MensagemMotivacional_Kant =
    @"O homem não é nada além daquilo que a educação faz dele"" - Immanuel Kant";

    /// <summary>Texto motivacional focado em constância nos estudos.</summary>
    public static readonly string MensagemMotivacional_Conhecimento =
    @"
Continue sua jornada de conhecimento e transforme disciplina 
em resultados. Cada minuto dedicado aos estudos é um passoa mais 
em direção aos seus objetivos.";

    /// <summary>Frase motivacional de Nelson Mandela.</summary>
    public static readonly string MensagemMotivacional_NelsonMandela =
    @"""A educação é a arma mais poderosa que você pode usar para mudar o mundo."" - Nelson Mandela";

    /// <summary>Frase motivacional de Sêneca.</summary>
    public static readonly string MensagemMotivacional_Seneca =
    @"""Não é que temos pouco tempo, mas sim que desperdiçamos muito dele"" - Sêneca";

    /// <summary>ASCII Art estilizada contendo o título 'ATHENA'.</summary>
    public static readonly string TituloAthena = @"
 █████╗ ████████╗██╗  ██╗███████╗███╗   ██╗ █████╗
██╔══██╗╚══██╔══╝██║  ██║██╔════╝████╗  ██║██╔══██╗
███████║   ██║   ███████║█████╗  ██╔██╗ ██║███████║
██╔══██║   ██║   ██╔══██║██╔══╝  ██║╚██╗██║██╔══██║
██║  ██║   ██║   ██║  ██║███████╗██║ ╚████║██║  ██║
╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚══════╝╚═╝  ╚═══╝╚═╝  ╚═╝";

/// <summary>Mensagem informativa exibida na tela de cadastro de usuários.</summary>
public static readonly string MensagemCadastro = @"
Junte-se ao Athena! Crie sua conta para acompanhar seu progresso, 
definir metas e organizar sua jornada de estudos.";

/// <summary>Mensagem de boas-vindas exibida na tela de login.</summary>
public static readonly string MensagemLogin = @"
Que bom ter você de volta! 
Faça login para continuar de onde parou e focar nas suas metas hoje.";

/// <summary>Instrução exibida na criação de novas metas.</summary>
public static readonly string MensagemMeta = @"
Defina uma nova meta para organizar sua rotina. 
Escolha um título claro e uma breve descrição do seu objetivo.";

/// <summary>Descrição explicativa do funcionamento e benefício das categorias.</summary>
public static readonly string Categoria = @"
As categorias ajudam a organizar suas metas de estudo. 
Crie grupos personalizados para separar seus objetivos por disciplina, 
projeto, concurso ou qualquer outro critério. As metas podem ser 
adicionadas ou removidas das categorias a qualquer momento.
";

/// <summary>Orientações para a seleção de categorias.</summary>
public static readonly string SelecionarCategoria = @"
As categorias permitem organizar suas metas de estudo em grupos personalizados, 
facilitando a visualização e o acompanhamento do seu progresso. Selecione uma 
das categorias abaixo para visualizar todas as metas vinculadas a ela, além das 
estatísticas e informações relacionadas.
";

/// <summary>Resumo explicativo sobre a área de gerenciamento da categoria selecionada.</summary>
public static readonly string SobreCategoria = @"
As categorias permitem organizar suas metas de estudo por disciplina, 
projeto ou qualquer outro critério que facilite seu planejamento. 
Nesta seção, você poderá acompanhar as estatísticas da categoria selecionada,
visualizar todas as metas vinculadas a ela e gerenciar seu progresso de forma 
prática e organizada.

";

/// <summary>Instruções passo a passo sobre o funcionamento de uma sessão de estudo.</summary>
public static readonly string SobreSessao = @"
Como funciona a sua sessão:

1- Marcar o tempo
O cronômetro vai contar o seu tempo real de foco e o tempo total da sessão com as pausas.

2- Finalizou, tá salvo!
Atenção: assim que você encerrar o cronômetro, a sessão é travada e não será possível alterar nada depois.

3- O que você estudou?
Para fechar a sessão, o sistema vai pedir um resumo rápido do que você fez.

Exemplos do que escrever:

Assisti à aulas sobre redação e escrevi uma com o tema 'tema da redação'

Resolvi questões de matemática (5 erradas e 6 certas)

";
}