# Athena

> Uma aplicação de gerenciamento de estudos desenvolvida em **C#** e **SQL Server**, criada para registrar sessões de estudo, acompanhar metas e monitorar a evolução da produtividade ao longo do tempo.

---

## Sobre o projeto

O Athena nasceu de uma necessidade pessoal.

Durante meus estudos, percebi que utilizar um cronômetro aumentava significativamente meu foco e me ajudava a manter uma rotina mais consistente. No entanto, sentia falta de uma ferramenta que também registrasse esse tempo e permitisse acompanhar minha evolução.

Foi a partir dessa ideia que decidi desenvolver o Athena.

O projeto começou como uma forma de praticar C# e SQL Server, mas aos poucos foi evoluindo para uma aplicação mais completa, permitindo registrar sessões de estudo, controlar metas e acompanhar o desempenho diário.

Embora atualmente seja um sistema de console desenvolvido para uso pessoal, a intenção é evoluí-lo continuamente até que possa ser utilizado por qualquer pessoa através de uma interface gráfica.

---

## Funcionalidades

Atualmente o sistema possui as seguintes funcionalidades:

- Cadastro de usuários
- Login utilizando e-mail e senha
- Cadastro de metas de estudo
- Edição de metas
- Exclusão de metas
- Cronômetro utilizando `Stopwatch`
- Registro do tempo estudado
- Armazenamento das informações em SQL Server

---

## Tecnologias utilizadas

- C#
- .NET
- SQL Server
- Microsoft.Data.SqlClient

---

## Estrutura do projeto

```text
Athena
│
├── Banco.cs
├── Cliente.cs
├── Cronometro.cs
├── Estudo.cs
├── Interface.cs
└── Program.cs
```

---

## Roadmap

O Athena continua em desenvolvimento e novas funcionalidades serão implementadas conforme o projeto evolui.

### Funcionalidades planejadas

- [ ] Criptografia de senhas
- [ ] Recuperação de senha por e-mail
- [ ] Validação para criação de senhas mais seguras
- [ ] Relatório semanal em PDF
- [ ] Relatório mensal em PDF
- [ ] Envio automático de relatórios por e-mail
- [ ] Sistema de conquistas
- [ ] Estatísticas de produtividade
- [ ] Melhor tratamento de exceções
- [ ] Refatoração do código
- [ ] Aplicação dos princípios SOLID
- [ ] Arquitetura em camadas
- [ ] Interface gráfica utilizando Windows Forms

---

## Objetivo

Mais do que registrar horas de estudo, o Athena busca incentivar a criação de uma rotina consistente.

A ideia é que o usuário consiga visualizar sua evolução ao longo do tempo, acompanhando quanto estudou no dia, na semana, no mês e até no ano. Essas informações servem como motivação para manter a disciplina e acompanhar seu próprio progresso.

Atualmente o projeto utiliza um banco de dados local e foi desenvolvido para uso pessoal. No futuro, pretendo transformá-lo em uma aplicação desktop para que outras pessoas também possam utilizá-lo.

---

## O que aprendi durante o desenvolvimento

O Athena é o projeto em que mais venho aplicando os conhecimentos adquiridos durante meus estudos em C#.

Ao longo do desenvolvimento, tenho praticado conceitos como:

- Programação Orientada a Objetos
- Manipulação de banco de dados com SQL Server
- Organização de código
- Persistência de dados
- Estruturação de aplicações em C#
- Desenvolvimento incremental de software

Além das funcionalidades, o projeto também serve como ambiente de estudo para arquitetura de software, Clean Code e boas práticas de desenvolvimento.

---

## Próximos passos

Depois de concluir as funcionalidades planejadas, pretendo dedicar um período exclusivamente para melhorar a qualidade do projeto.

Entre as principais melhorias previstas estão:

- Refatoração da arquitetura
- Separação de responsabilidades entre as classes
- Melhor tratamento de erros
- Aplicação dos princípios SOLID
- Testes unitários
- Interface gráfica
- Melhor experiência para o usuário

Meu objetivo é que o Athena evolua junto com meu crescimento como desenvolvedor.

---

## Considerações finais

Este projeto faz parte da minha jornada de aprendizado em desenvolvimento de software.

Em vez de desenvolver vários projetos pequenos, decidi investir tempo em uma aplicação que pudesse evoluir continuamente, permitindo aplicar novos conhecimentos conforme estudo temas como arquitetura de software, segurança, banco de dados e boas práticas de programação.

Cada nova funcionalidade representa um passo no meu aprendizado, e a expectativa é que o Athena continue crescendo junto com minha experiência como desenvolvedor.
