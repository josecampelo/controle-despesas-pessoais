# Controle de Despesas Pessoais

Aplicação web desenvolvida em **ASP.NET Core 8 MVC** para gerenciamento de finanças pessoais. Este projeto acadêmico foi criado para a disciplina de *Software Product: Analysis, Specification, Project & Implementation*, com entregas incrementais que constroem uma solução completa ao longo do semestre.

---

## ✨ Funcionalidades

### Gerenciamento de Categorias e Gráfico de Despesas (Etapa 3)
- **Cadastro e Organização de Categorias:** Permite criar, editar e excluir categorias personalizadas (ex: Alimentação, Moradia, Transporte).
- **Associação de Categorias às Transações:** Cada receita ou despesa agora pode ser vinculada a uma categoria específica, garantindo melhor controle financeiro.
- **Gráfico de Pizza no Dashboard:** Mostra a distribuição percentual das despesas do mês por categoria, utilizando Chart.js e exibindo porcentagens diretamente nas fatias.

### Dashboard (Etapa 2)
- **Resumo Financeiro Mensal:** Cards de destaque exibem o total de receitas, despesas e o saldo final do mês corrente.
- **Interface Intuitiva:** Layout visualmente claro com cores e ícones que facilitam a rápida identificação do status financeiro.
- **Navegação Aprimorada:** Barra de navegação principal reestruturada para melhor usabilidade e responsividade.

### Gerenciamento de Transações (Etapa 1)
- **Listagem Completa:** Todas as receitas e despesas são exibidas em uma tabela organizada.
- **Registro de Transações:** Formulário simples e validado para adicionar novas receitas ou despesas.
- **Tipos de Transação:** Uso de `Enum` com um dropdown na interface para garantir a consistência dos dados (apenas "Receita" ou "Despesa").

---

## 🚀 Roadmap do Projeto

O projeto é dividido em 4 etapas principais:

- [x] **Etapa 1: Estruturação e CRUD Inicial** - _Concluída em 13/09/2025_
- [x] **Etapa 2: Dashboard com Resumo Financeiro** - _Concluída em 11/10/2025_
- [x] **Etapa 3: Gerenciamento de Categorias e Gráfico de Despesas**
- [ ] **Etapa 4: Finalização do CRUD (Editar/Excluir) e Filtros**

---

## 🛠️ Tecnologias Utilizadas

Este projeto foi construído com uma stack moderna e robusta:

- **Back-end:**
  - C# 12 / .NET 8
  - ASP.NET Core MVC
  - Entity Framework Core 8 (Code-First)
- **Front-end:**
  - HTML5 / CSS3
  - Bootstrap 5
  - Razor Pages
- **Banco de Dados:**
  - Microsoft SQL Server
- **Ferramentas e Versionamento:**
  - Visual Studio 2022
  - Git & GitHub
  - Azure Data Studio

---

## ⚙️ Como Executar o Projeto

Siga os passos abaixo para rodar o projeto em seu ambiente local.

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/pt-br/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/vs/)
- [SQL Server Express Edition](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads)

### Passos

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/josecampelo/controle-despesas-pessoais.git](https://github.com/josecampelo/controle-despesas-pessoais.git)
    cd controle-despesas-pessoais
    ```

2.  **Abra a solução (`.sln`) no Visual Studio.**

3.  **Configure a String de Conexão:**
    - No arquivo `appsettings.json`, verifique se a `DefaultConnection` está configurada para sua instância do SQL Server.
    - O padrão é: `"Server=.\\SQLEXPRESS;Database=ControleDespesasDB_MVC;Trusted_Connection=True;TrustServerCertificate=True;"`

4.  **Aplique as Migrations para criar o banco de dados:**
    - No Visual Studio, abra o **Console do Gerenciador de Pacotes** (`Ferramentas > ...`).
    - Execute o comando:
      ```powershell
      Update-Database
      ```

5.  **Execute a Aplicação:**
    - Pressione `F5` ou o botão de execução para iniciar o projeto.

---

## 📝 Licença

Este projeto é de natureza acadêmica e não possui uma licença formal para distribuição ou uso comercial.
