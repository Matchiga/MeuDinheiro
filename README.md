## Meu Dinheiro API - Gerencie suas finanças pessoais com facilidade!

Esta é uma API RESTful construída com ASP.NET Core e Entity Framework Core que fornece um conjunto de endpoints para gerenciar suas receitas, despesas e obter um resumo mensal das suas finanças.

### Funcionalidades Principais

- **Autenticação:** Suporte à autenticação de usuários usando ASP.NET Core Identity com  entidades personalizadas (`PessoaComAcesso` e `PerfilDeAcesso`).
- **Gerenciamento de Receitas:**
    - Criar, listar, buscar, atualizar e excluir receitas.
    - Listar receitas por mês/ano específico.
- **Gerenciamento de Despesas:**
    - Criar, listar, buscar, atualizar e excluir despesas.
    - Categorizar despesas (com uma categoria padrão predefinida).
    - Listar despesas por mês/ano específico.
- **Resumo Mensal:**
    - Obter um resumo financeiro completo de um mês específico, incluindo:
        - Total de receitas.
        - Total de despesas.
        - Saldo final.
        - Gastos por categoria.
- **Documentação Swagger:** A API possui documentação interativa completa usando o Swagger, disponível em `/swagger` na raiz do projeto.

### Tecnologias Utilizadas

- **Backend:** 
    - C#
    - ASP.NET Core 8
    - Entity Framework Core
    - Microsoft.AspNetCore.Identity
    - Swashbuckle (para documentação Swagger)
- **Banco de Dados:**
    - SQL Server (com configuração via `appsettings.json` ou string de conexão direta).
- **Ferramentas:**
    - Visual Studio (ou sua IDE preferida).
    - Git (para versionamento de código).

### Instalação e Execução

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/Matchiga/MeuDinheiro.git
   ```

2. **Restaure os pacotes NuGet:**
   - Abra o projeto no Visual Studio ou use o comando `dotnet restore` na pasta do projeto.

3. **Configure o banco de dados:**
   - Atualize a string de conexão no arquivo `appsettings.json` ou dentro da classe `MDContext` para apontar para o seu servidor SQL Server.
   - Execute as migrações do Entity Framework para criar o banco de dados:
     ```bash
     dotnet ef database update
     ```

4. **Execute a aplicação:**
   - Use o comando `dotnet run` ou execute o projeto no Visual Studio.
   - Acesse a documentação do Swagger em: `https://localhost:7168/swagger` (verifique a porta correta em `launchSettings.json`).

### Pontos Importantes do Código

- **`DAO<T>`:** Uma classe genérica para acesso a dados (DAO) que fornece métodos básicos (CRUD) para interagir com o banco de dados.
- **Endpoints:** Implementados usando Minimal APIs do ASP.NET Core para melhor legibilidade e concisão.
- **Autenticação:** O endpoint `/auth` (configurado com `MapIdentityApi`) fornece endpoints para registro, login e gerenciamento de usuários.
- **Tratamento de Erros:** Middleware personalizado para retornar mensagens de erro mais amigáveis (código de status 401 e 403).

### Próximos Passos

- Implementar testes unitários e de integração.
- Adicionar validações de dados mais robustas nos endpoints.
- Criar uma interface de usuário (frontend) para interagir com a API.

## Contribuições

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues para reportar bugs, solicitar novas funcionalidades ou enviar pull requests.
