namespace Init_db;

/// <summary>
/// Armazena os textos fixos utilizados nas telas do sistema,
/// centralizando as mensagens exibidas ao usuário.
/// </summary>
public class Textos
{
public static readonly string Atualizacoes = @"# Próximas Atualizações

O ATHENA continuará recebendo novas funcionalidades com foco em organização, produtividade, acompanhamento do desempenho e segurança. Abaixo estão alguns dos recursos planejados para as próximas versões do sistema.

## ATHENA 1.1 — Organização e Cronômetro

* Adicionar categorias para metas de estudo.
* Implementar datas limite para conclusão das metas.
* Permitir arquivamento de metas.
* Criar histórico de metas arquivadas.
* Criar histórico de metas concluídas.
* Permitir reabertura de metas arquivadas.
* Adicionar opção para pausar e continuar o cronômetro.
* Registrar pausas durante as sessões de estudo.
* Criar histórico de sessões realizadas.
* Exibir a duração da última sessão de estudo.
* Exibir a maior sessão de estudo registrada.

## ATHENA 1.2 — Estatísticas

* Exibir tempo estudado hoje.
* Exibir tempo estudado na semana.
* Exibir tempo estudado no mês.
* Exibir tempo estudado no ano.
* Calcular médias diárias, semanais e mensais.
* Implementar sistema de sequência de estudos (Streak).
* Criar um dashboard com indicadores de desempenho.
* Exibir evolução do tempo estudado.
* Adicionar ranking pessoal de desempenho.

## ATHENA 1.3 — Produtividade

* Implementar a Técnica Pomodoro.
* Adicionar Modo Foco.
* Criar objetivos semanais, mensais e anuais.
* Permitir registro de anotações.
* Adicionar checklist de tarefas.

## ATHENA 1.4 — Relatórios

* Gerar relatórios semanais.
* Gerar relatórios mensais.
* Gerar relatórios anuais.
* Exportar relatórios em PDF.
* Permitir envio de relatórios por e-mail.
* Criar histórico de relatórios gerados.

## ATHENA 1.5 — Calendário

* Implementar calendário de estudos.
* Adicionar calendário no estilo GitHub.
* Exibir dias estudados.
* Identificar o melhor dia da semana para estudos.
* Identificar o melhor mês de desempenho.

## ATHENA 1.6 — Gamificação

* Implementar sistema de XP.
* Criar sistema de níveis.
* Adicionar conquistas.
* Implementar medalhas.
* Criar sistema de recompensas pessoais.
* Permitir definição de objetivos pessoais.

## ATHENA 1.7 — Login e Segurança

* Recuperação de senha por e-mail.
* Código de verificação por e-mail.
* Validação de senha forte.
* Bloqueio temporário após múltiplas tentativas de login.

> **Observação:** As funcionalidades acima representam o planejamento atual do ATHENA e poderão sofrer alterações, receber melhorias ou ser ampliadas durante o desenvolvimento das próximas versões.
";


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
    public static readonly string MensagemInicial = @"
Inspirado em Atena, a deusa grega da sabedoria, estratégia e conhecimento,
o Athena foi criado para auxiliar estudantes na organização dos estudos,
no acompanhamento do progresso e na construção de uma jornada de aprendizado 
mais eficiente e disciplinada.";

    public static readonly string MensagemMotivacional_Kant =
    @"O homem não é nada além daquilo que a educação faz dele"" - Immanuel Kant";

    public static readonly string MensagemMotivacional_Conhecimento =
    @"
Continue sua jornada de conhecimento e transforme disciplina 
em resultados. Cada minuto dedicado aos estudos é um passoa mais 
em direção aos seus objetivos.";
    public static readonly string MensagemMotivacional_NelsonMandela =
    @"""A educação é a arma mais poderosa que você pode usar para mudar o mundo."" - Nelson Mandela";
    public static readonly string MensagemMotivacional_Seneca =
    @"""Não é que temos pouco tempo, mas sim que desperdiçamos muito dele"" - Sêneca";

    public static readonly string TituloAthena = @"
 █████╗ ████████╗██╗  ██╗███████╗███╗   ██╗ █████╗
██╔══██╗╚══██╔══╝██║  ██║██╔════╝████╗  ██║██╔══██╗
███████║   ██║   ███████║█████╗  ██╔██╗ ██║███████║
██╔══██║   ██║   ██╔══██║██╔══╝  ██║╚██╗██║██╔══██║
██║  ██║   ██║   ██║  ██║███████╗██║ ╚████║██║  ██║
╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚══════╝╚═╝  ╚═══╝╚═╝  ╚═╝";

public static readonly string MensagemCadastro = @"
Junte-se ao Athena! Crie sua conta para acompanhar seu progresso, 
definir metas e organizar sua jornada de estudos.";
public static readonly string MensagemLogin = @"
Que bom ter você de volta! 
Faça login para continuar de onde parou e focar nas suas metas hoje.";
public static readonly string MensagemMeta = @"
Defina uma nova meta para organizar sua rotina. 
Escolha um título claro e uma breve descrição do seu objetivo.";

public static readonly string Categoria = @"
As categorias ajudam a organizar suas metas de estudo. 
Crie grupos personalizados para separar seus objetivos por disciplina, 
projeto, concurso ou qualquer outro critério. As metas podem ser 
adicionadas ou removidas das categorias a qualquer momento.
";
public static readonly string SelecionarCategoria = @"
As categorias permitem organizar suas metas de estudo em grupos personalizados, 
facilitando a visualização e o acompanhamento do seu progresso. Selecione uma 
das categorias abaixo para visualizar todas as metas vinculadas a ela, além das 
estatísticas e informações relacionadas.
";
public static readonly string SobreCategoria = @"
As categorias permitem organizar suas metas de estudo por disciplina, 
projeto ou qualquer outro critério que facilite seu planejamento. 
Nesta seção, você poderá acompanhar as estatísticas da categoria selecionada,
visualizar todas as metas vinculadas a ela e gerenciar seu progresso de forma 
prática e organizada.

";
}