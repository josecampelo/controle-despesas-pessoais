# Controle de Despesas Pessoais

Projeto acadêmico desenvolvido para a disciplina de **Software Product: Analysis, Specification, Project & Implementation** do 5º semestre do curso de Análise e Desenvolvimento de Sistemas. O objetivo é criar uma aplicação web completa para gerenciamento de finanças pessoais, com entregas incrementais ao longo do semestre.

---

## Status do Projeto

**Etapa 1: Estruturação do Projeto e CRUD Inicial de Transações (Criar e Ler) - Concluída em 13/09/2025**

---

## Funcionalidades da Etapa 1

* **Listagem de Transações:** Visualização de todas as receitas e despesas em uma tabela organizada e com estilo.
* **Criação de Transações:** Formulário para adicionar novas transações (receitas ou despesas) com validações.
* **Tipo de Transação Seguro:** Uso de `Enum` para garantir que o tipo seja apenas "Receita" ou "Despesa", com um dropdown na interface.
* **Layout Responsivo:** Interface limpa e funcional, adaptada para diferentes tamanhos de tela graças ao Bootstrap.
* **Persistência de Dados:** As informações são salvas em um banco de dados SQL Server através do Entity Framework Core.

---

## Tecnologias Utilizadas

Este projeto foi construído utilizando as seguintes tecnologias na sua camada de back-end, front-end e banco de dados:

* **Back-end:**
    * C# 12
    * .NET 8
    * ASP.NET Core MVC
    * Entity Framework Core 8 (Code-First)
* **Front-end:**
    * HTML5 / CSS3
    * Bootstrap 5.1
    * Razor Pages
* **Banco de Dados:**
    * Microsoft SQL Server
* **Ferramentas e Versionamento:**
    * Git & GitHub
    * Visual Studio 2022
    * Azure Data Studio

---

## Como Executar o Projeto

Siga os passos abaixo para rodar o projeto em seu ambiente local.

### Pré-requisitos

* [.NET 8 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
* [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/vs/)
* [SQL Server Express Edition](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)

### Passos

1.  **Clone o repositório:**
    ```bash
    git clone https://github.com/josecampelo/controle-despesas-pessoais.git
    ```

2.  **Abra o projeto:**
    * Navegue até a pasta clonada e abra a solução `ControleDespesas.sln` no Visual Studio.

3.  **Configure a String de Conexão:**
    * Abra o arquivo `appsettings.json`.
    * Verifique se a `DefaultConnection` em `ConnectionStrings` aponta para a sua instância local do SQL Server Express. O padrão é:
        `"Server=.\\SQLEXPRESS;Database=ControleDespesasDB_MVC;Trusted_Connection=True;TrustServerCertificate=True;"`

4.  **Aplique as Migrations:**
    * No Visual Studio, vá em `Ferramentas > Gerenciador de Pacotes do NuGet > Console do Gerenciador de Pacotes`.
    * Execute o comando para criar o banco de dados e as tabelas:
        ```powershell
        Update-Database
        ```

5.  **Execute a Aplicação:**
    * Pressione `F5` ou clique no botão de "play" para iniciar o projeto. O site abrirá no seu navegador padrão.

