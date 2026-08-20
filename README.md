# ATHENA

> Sistema de gerenciamento de estudos desenvolvido em **C#** e **SQL Server**, criado para auxiliar na organização das metas, controle das sessões de estudo, registro de tempo e acompanhamento da produtividade.

**Versão atual:** `v1.1.0`
**Status:** Em desenvolvimento

---

# Sobre o projeto

O **ATHENA** nasceu de uma necessidade pessoal.

Durante meus estudos, percebi que utilizar um cronômetro aumentava significativamente meu foco e ajudava a manter uma rotina mais consistente. Porém, apenas registrar o tempo não era suficiente. Eu também precisava de uma forma de organizar minhas metas, acompanhar o que havia sido estudado e visualizar minha evolução ao longo do tempo.

Foi a partir dessa ideia que surgiu o ATHENA.

O projeto começou como uma aplicação para praticar **C# e SQL Server**, mas evoluiu gradualmente para um sistema de gerenciamento de estudos com autenticação de usuários, metas, categorias, cronômetro, sessões de estudo, histórico de atividades e estatísticas de produtividade.

Atualmente, o ATHENA funciona como uma **aplicação de terminal**, utilizando **Spectre.Console** para proporcionar uma interface interativa e organizada.

A versão atual representa a finalização da **ATHENA 1.1 — Versão Terminal**.

---

# Funcionalidades

## 👤 Usuários

* ✅ Cadastro de usuários
* ✅ Login
* ✅ Autenticação de usuários
* ✅ Senhas protegidas com hash
* ✅ Controle de acesso aos dados do usuário

---

## 🎯 Metas de estudo

* ✅ Criar metas de estudo
* ✅ Pesquisar metas
* ✅ Editar metas
* ✅ Excluir metas
* ✅ Marcar metas como concluídas
* ✅ Visualizar metas pendentes
* ✅ Visualizar metas concluídas
* ✅ Definir prioridade
* ✅ Definir data limite
* ✅ Ordenar por data de criação
* ✅ Ordenar por tempo estudado
* ✅ Ordenar por título
* ✅ Visualizar histórico de metas concluídas

---

## 📂 Categorias

O ATHENA permite organizar as metas por categorias para facilitar a separação dos diferentes assuntos estudados.

* ✅ Criar categorias
* ✅ Editar categorias
* ✅ Excluir categorias
* ✅ Adicionar metas às categorias
* ✅ Remover metas das categorias
* ✅ Visualizar metas de uma categoria
* ✅ Iniciar uma sessão de estudo a partir de uma categoria
* ✅ Escolher metas para vinculação às categorias

---

## ⏱️ Cronômetro e sessões de estudo

O sistema possui um cronômetro desenvolvido utilizando `Stopwatch`, permitindo registrar as sessões de estudo realizadas pelo usuário.

* ✅ Iniciar cronômetro
* ✅ Pausar cronômetro
* ✅ Continuar cronômetro
* ✅ Registrar tempo estudado
* ✅ Registrar tempo durante as pausas
* ✅ Calcular duração das sessões
* ✅ Registrar histórico das sessões
* ✅ Visualizar a duração da última sessão
* ✅ Visualizar a maior sessão de estudo

---

## 📝 Registro de atividade

Cada sessão de estudo pode registrar informações sobre o que foi realizado durante aquele período.

* ✅ Adicionar descrição à sessão
* ✅ Registrar o conteúdo ou atividade realizada
* ✅ Consultar o histórico das sessões
* ✅ Visualizar as atividades realizadas

---

## 📊 Estatísticas

O ATHENA possui estatísticas básicas para acompanhar o tempo dedicado aos estudos.

* ✅ Tempo estudado no dia
* ✅ Tempo estudado na semana
* ✅ Tempo estudado no mês
* ✅ Histórico das sessões de estudo
* ✅ Maior sessão de estudo
* ✅ Duração da última sessão

As estatísticas serão ampliadas nas próximas versões para oferecer uma visão mais completa do desempenho do usuário.

---

# 🖥️ Interface

A versão 1.1 utiliza **Spectre.Console** para construir uma experiência de terminal mais organizada e interativa.

Entre os recursos utilizados estão:

* ✅ Menus interativos
* ✅ Navegação por seleção
* ✅ Dashboard inicial
* ✅ Painéis
* ✅ Tabelas
* ✅ Mensagens coloridas
* ✅ Organização das informações por seções
* ✅ Feedback visual durante as operações

A escolha do Spectre.Console permitiu transformar a aplicação de um terminal tradicional em uma interface mais estruturada, mantendo o projeto como uma aplicação de console.

---

# Tecnologias utilizadas

### Linguagem e plataforma

* **C#**
* **.NET**

### Banco de dados

* **SQL Server**
* **Microsoft.Data.SqlClient**

### Interface

* **Spectre.Console**

### Segurança

* **BCrypt**

### Controle de versão

* **Git**
* **GitHub**

---

# Estrutura do projeto

A arquitetura atual do ATHENA foi organizada buscando separar as responsabilidades da aplicação.

```text
ATHENA
│
├── Database
│   └── ...
│
├── Models
│   └── ...
│
├── Services
│   └── ...
│
├── UI
│   └── ...
│
├── Program.cs
└── ...
```

A estrutura foi evoluindo conforme novas funcionalidades foram adicionadas ao sistema, buscando evitar que toda a lógica da aplicação permanecesse concentrada em poucas classes.

---

# Arquitetura e organização

Durante o desenvolvimento do ATHENA, o projeto passou por diversas refatorações.

Entre as principais melhorias estão:

* Separação de responsabilidades
* Organização da camada de acesso ao banco de dados
* Separação entre modelos, serviços e interface
* Centralização de mensagens
* Centralização de validações
* Documentação XML dos principais métodos
* Tratamento de erros
* Validação de entradas
* Controle de acesso aos dados dos usuários
* Refatoração de código
* Melhor organização da estrutura do projeto

A arquitetura continuará sendo aprimorada durante o desenvolvimento das próximas versões.

---

# ATHENA 1.1 — Versão Terminal

A versão **1.1** representa uma grande evolução em relação à primeira versão do sistema.

O foco dessa versão foi transformar o ATHENA em uma aplicação de terminal mais completa, organizada e funcional.

### Principais avanços

* Interface completamente reorganizada com Spectre.Console
* Sistema de categorias
* Prioridade das metas
* Data limite para conclusão
* Histórico de metas concluídas
* Sistema de sessões de estudo
* Pausa e continuação do cronômetro
* Registro de atividades
* Histórico das sessões
* Estatísticas básicas
* Melhorias na organização do banco de dados
* Refatoração do código
* Correção de bugs
* Testes das principais funcionalidades

Com isso, a etapa planejada para a **ATHENA 1.1 — Versão Terminal** foi concluída.

---

# Roadmap

Com a versão Terminal 1.1 finalizada, o próximo grande objetivo do projeto é a criação da **ATHENA 2.0**.

## 🚧 ATHENA 2.0 — Versão Aplicativo

* [ ] Migrar a interface de terminal para uma interface gráfica
* [ ] Reestruturar as telas para uma experiência de aplicativo
* [ ] Criar dashboard visual
* [ ] Adaptar metas para a nova interface
* [ ] Adaptar categorias
* [ ] Adaptar sessões de estudo
* [ ] Adaptar o cronômetro
* [ ] Adaptar o sistema de pausas
* [ ] Adaptar as estatísticas
* [ ] Melhorar a visualização do histórico
* [ ] Implementar melhorias de usabilidade e navegação
* [ ] Revisar a arquitetura durante a migração

A tecnologia da interface gráfica ainda poderá ser definida durante o desenvolvimento da versão 2.0, considerando opções como **Windows Forms** ou **WPF**.

---

## 🔮 Futuras versões

Após a migração para uma aplicação gráfica, novas funcionalidades poderão ser desenvolvidas conforme as necessidades identificadas durante a utilização do sistema.

Entre as possibilidades estão:

* [ ] Estatísticas e métricas mais avançadas
* [ ] Relatórios de desempenho
* [ ] Calendário de estudos
* [ ] Sistema de sequência de estudos (*Streak*)
* [ ] Gamificação
* [ ] Melhorias de segurança e autenticação
* [ ] Exportação de dados e relatórios
* [ ] Novos recursos de produtividade

> **Observação:** O roadmap representa a direção atual do ATHENA. As funcionalidades e prioridades poderão ser alteradas conforme novas necessidades, ideias e aprendizados surgirem durante o desenvolvimento.

---

# O que aprendi

O ATHENA representa, até o momento, um dos meus principais projetos de aprendizado em desenvolvimento de software.

Durante seu desenvolvimento, pude colocar em prática conceitos como:

* Programação Orientada a Objetos
* C#
* .NET
* SQL Server
* CRUD
* Manipulação de banco de dados
* Relacionamentos entre tabelas
* Autenticação
* Hash de senhas
* Validação de dados
* Tratamento de erros
* Refatoração
* Organização de código
* Separação de responsabilidades
* Documentação XML
* Interfaces de terminal
* Controle de tempo utilizando `Stopwatch`
* Git e GitHub

Além das funcionalidades implementadas, o ATHENA continua sendo utilizado como um ambiente de aprendizado para estudar **arquitetura de software, Clean Code, princípios SOLID, segurança, testes e desenvolvimento de aplicações mais robustas**.

---

# Próximos passos

Com a conclusão da versão **1.1**, o próximo grande ciclo de desenvolvimento será a transformação do ATHENA em uma aplicação gráfica.

O objetivo não é apenas trocar a interface do terminal, mas aproveitar essa migração para continuar evoluindo a arquitetura do sistema, melhorar a experiência de uso e aplicar os conhecimentos adquiridos durante o desenvolvimento da versão atual.

---

# Considerações finais

O ATHENA faz parte da minha jornada de aprendizado em desenvolvimento de software.

Em vez de desenvolver diversos projetos pequenos e abandoná-los após aprender determinado conceito, escolhi construir uma aplicação que pudesse continuar evoluindo junto com meus conhecimentos.

Cada versão do ATHENA representa uma etapa desse processo.

A **v1.0** estabeleceu a base do sistema.

A **v1.1** expandiu significativamente suas funcionalidades e consolidou a versão de terminal.

A **v2.0** terá como objetivo levar o ATHENA para uma nova etapa, transformando-o em uma aplicação gráfica e continuando a evolução de sua arquitetura.

O projeto continua em desenvolvimento.

---

## Versão

**Versão atual:** `v1.1.0`

**Status:** Em desenvolvimento

**Próxima versão:** `v2.0.0 — Aplicativo`

