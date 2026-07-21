# ATHENA

> Sistema de gerenciamento de estudos desenvolvido em **C#** e **SQL Server**, criado para auxiliar estudantes na organização dos estudos, registro do tempo dedicado às atividades e acompanhamento da evolução ao longo do tempo.

---

# Sobre o projeto

O ATHENA nasceu de uma necessidade pessoal.

Durante meus estudos, percebi que utilizar um cronômetro aumentava significativamente meu foco e ajudava a manter uma rotina mais consistente. No entanto, sentia falta de uma ferramenta que, além de registrar o tempo estudado, permitisse organizar metas e acompanhar meu progresso.

Foi a partir dessa ideia que decidi desenvolver o ATHENA.

Inicialmente criado como um projeto para praticar C# e SQL Server, o sistema evoluiu para uma aplicação mais completa, incorporando autenticação de usuários, gerenciamento de metas de estudo, controle do tempo estudado e acompanhamento da produtividade.

Atualmente o ATHENA é uma aplicação de console, mas o objetivo é continuar evoluindo o projeto até transformá-lo em uma aplicação desktop com interface gráfica.

---

# Funcionalidades

Atualmente o sistema possui:

- ✅ Cadastro de usuários
- ✅ Login de usuários
- ✅ Cadastro de metas de estudo
- ✅ Pesquisa de metas
- ✅ Edição de metas
- ✅ Exclusão de metas
- ✅ Marcação de metas como concluídas
- ✅ Visualização de metas pendentes
- ✅ Visualização de metas concluídas
- ✅ Cronômetro utilizando `Stopwatch`
- ✅ Registro automático do tempo estudado
- ✅ Controle do tempo total de estudo do usuário
- ✅ Validação de entradas
- ✅ Mensagens centralizadas
- ✅ Persistência dos dados utilizando SQL Server

---

# Tecnologias utilizadas

- C#
- .NET
- SQL Server
- Microsoft.Data.SqlClient

---

# Estrutura do projeto

```
ATHENA
│
├── Banco.cs
├── Cliente.cs
├── Cronometro.cs
├── Estudo.cs
├── Interface.cs
├── Mensagens.cs
├── Textos.cs
├── Validacao.cs
└── Program.cs
```

---

# Arquitetura

Durante a versão **1.0**, o projeto passou por uma grande refatoração com o objetivo de tornar o código mais organizado e de fácil manutenção.

Entre as melhorias realizadas estão:

- Separação das responsabilidades entre as classes
- Centralização das mensagens do sistema
- Centralização das validações de entrada
- Documentação XML dos principais métodos
- Organização da interface do sistema
- Correção de bugs
- Melhorias na segurança para impedir o acesso às metas de outros usuários

---

# Roadmap

Funcionalidades planejadas para as próximas versões:

- [ ] Criptografia de senhas
- [ ] Recuperação de senha
- [ ] Relatórios em PDF
- [ ] Estatísticas de produtividade
- [ ] Sistema de conquistas
- [ ] Dashboard de desempenho
- [ ] Aplicação dos princípios SOLID
- [ ] Arquitetura em camadas
- [ ] Testes unitários
- [ ] Interface gráfica (Windows Forms ou WPF)

---

# O que aprendi

Este projeto representa meu maior aprendizado em C# até o momento.

Durante seu desenvolvimento pratiquei conceitos como:

- Programação Orientada a Objetos
- Manipulação de banco de dados com SQL Server
- CRUD completo
- Refatoração de código
- Organização de projetos
- Documentação XML
- Validação de dados
- Tratamento de erros
- Persistência de dados
- Boas práticas de programação

Além das funcionalidades implementadas, o projeto continua servindo como ambiente de estudos para arquitetura de software, Clean Code e desenvolvimento de aplicações mais robustas.

---

# Próximos passos

A versão **1.0** representa a primeira versão estável do ATHENA.

As próximas versões terão como foco:

- melhorar a arquitetura do sistema;
- implementar novos recursos para acompanhamento dos estudos;
- aumentar a segurança da aplicação;
- desenvolver uma interface gráfica;
- aplicar princípios SOLID e outras boas práticas de engenharia de software.

---

# Considerações finais

O ATHENA faz parte da minha jornada de aprendizado em desenvolvimento de software.

Em vez de desenvolver vários projetos pequenos, optei por investir em uma aplicação que pudesse evoluir continuamente, permitindo aplicar novos conhecimentos à medida que estudo temas como arquitetura de software, banco de dados, segurança e boas práticas de programação.

Cada nova versão representa não apenas a evolução do sistema, mas também a evolução das minhas habilidades como desenvolvedor.

---

## Versão

**Versão atual:** **v1.0.0**
