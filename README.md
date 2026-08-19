# Store API

API desenvolvida para praticar **Arquitetura Limpa** e o **padrão Repository** com ASP.NET Core Minimal API.

## Objetivo

Projeto de estudo focado em:

- **Arquitetura Limpa** — separação de responsabilidades em camadas independentes, onde as regras de negócio não dependem de frameworks ou infraestrutura.
- **Padrão Repository** — abstração do acesso a dados via interfaces definidas no domínio e implementadas na infraestrutura.
- **Autenticação e autorização** — JWT com controle de acesso por papel (role).

## Estrutura das Camadas

```
Store.Domain          # Entidades, Value Objects, interfaces de repositório e abstrações
Store.Application     # Casos de uso (handlers MediatR)
Store.Infrastructure  # Implementações de repositório, EF Core, mapeamentos, segurança
Store.Api             # Endpoints Minimal API, autenticação/autorização, configuração da aplicação
Store.AppHost         # Orquestração via .NET Aspire (API + banco de dados PostgreSQL)
Store.Test            # Testes unitários do Domain — Entidades e Value Objects (MSTest)
```

### Domain

Camada central, sem dependências de outras camadas do projeto.

- **Entidades:** `Customer`, `Address`, `Store`, `Order`, `OrderProduct`, `Product`, `User`
- **Classe base:** `Entity` com `Id` (Guid), `CreatedAt` e `UpdatedAt`
- **Value Objects:** `Email`, `Phone`, `Document` (CPF/CNPJ, validado via `BrDocuments`), `ZipCode`, `Currency`, `Role`, `Status`
- **Enums:** `ERole` (Admin, Seller, Purchaser, StockClerk), `EStatus` (Pending, Paid, Shipped, Delivered, Canceled), `EDocumentType` (CPF, CNPJ)
- **Interfaces de repositório:** interfaces segregadas (`IAllReadableRepository<T>`, `IByIdReadableRepository<T>`, `ICreatableRepository<T>`, `IUpdatableRepository<T>`, `IDeletableRepository`), compostas por entidade
- **Interfaces de segurança:** `IPasswordService`, `ITokenService`
- **Result pattern:** `Result` e `Result<T>` (via `FluentResults`) para encapsular sucesso/falha sem lançar exceções
- **Error:** representa erros de domínio retornados via `Result`
- **Validações:** cada entidade valida seus dados em métodos estáticos `Create`/`Update`, acumulando erros numa lista antes de retornar

### Application

Orquestra os casos de uso com **MediatR**.

Cada operação segue a estrutura:

```
UseCases/{Entidade}/{Operação}/
    Command.cs   # IRequest com os dados de entrada
    Response.cs  # DTO de saída
    Handler.cs   # Lógica da operação, depende apenas de IRepository
```

Operações disponíveis por entidade:

| Entidade | Operações |
|---|---|
| Customer | Create, GetAll, GetById, Update, Delete |
| Address | Create, GetAll, GetById, Update, Delete |
| Store | Create, GetAll, GetById, Update, Delete |
| Product | Create, GetAll, GetById, Update, Delete |
| Order | Create, GetAll, GetById, Update, Delete |
| OrderProduct | Create, GetByOrderId, Update, Delete |
| User | Create, Authenticate |

### Infrastructure

- **EF Core + PostgreSQL** via Npgsql
- **`StoreContext`** com `DbSet` para cada entidade
- **Mappings** com `IEntityTypeConfiguration<T>` para cada entidade
- **Implementações dos repositórios** registradas como `Transient` no container de DI
- **Segurança:**
  - `PasswordService` — hash e verificação de senha via `Microsoft.AspNetCore.Identity.PasswordHasher`
  - `TokenService` — geração de JWT (HMAC-SHA256, expiração de 2h), registrado como `Singleton`, recebendo a chave de `JwtSecretKey`

### Api

- **Minimal API** com endpoints agrupados por entidade (`MapGroup`)
- **FluentValidation** — validação de entrada nos endpoints `POST`/`PUT`, com um `AbstractValidator` por `Command` em `Validators/{Entidade}`, registrados automaticamente via `AddValidatorsFromAssembly`. Requisições inválidas retornam `400 Bad Request` antes de chegar ao handler
- **Autenticação JWT Bearer** via `Microsoft.AspNetCore.Authentication.JwtBearer`
- **Autorização baseada em papéis (role)**, com policy `admin` e checagens `RequireRole` por endpoint
- **OpenAPI** gerado nativamente pelo ASP.NET Core
- **Scalar** como interface para explorar e testar a API em `/scalar`

## Orquestração com .NET Aspire

O projeto `Store.AppHost` orquestra a aplicação via **.NET Aspire**:

- Provisiona um container **PostgreSQL** (`Aspire.Hosting.PostgreSQL`), com senha via parâmetro secreto e volume de dados persistente
- Cria o banco `store` dentro do container
- Sobe o `Store.Api`, referenciando o banco (`WithReference`) e aguardando sua disponibilidade (`WaitFor`) antes de iniciar
- O `Store.Api` consome a connection string via `Aspire.Npgsql.EntityFrameworkCore.PostgreSQL` (`AddNpgsqlDbContext<StoreContext>("store")`), dispensando a configuração manual de `ConnectionStrings` em `appsettings.json` ao rodar pelo AppHost

Para executar com orquestração:

```
dotnet run --project Store.AppHost
```

## Autenticação e Autorização

Autenticação via JWT Bearer. O token é obtido em `POST /users/authenticate` e deve ser enviado no header `Authorization: Bearer {token}`.

Papéis (`role`) existentes: `admin`, `seller`, `purchaser`, `stock_clerk`.

Cada endpoint exige autenticação (`RequireAuthorization`) e, na maioria dos casos, um papel específico:

| Recurso | GET (lista/por id) | POST | PUT | DELETE |
|---|---|---|---|---|
| `/customers` | admin, seller | admin, seller | admin, seller | admin |
| `/addresses` | autenticado | admin, seller | admin, seller | admin |
| `/stores` | autenticado | admin | admin, seller | admin |
| `/products` | autenticado | admin, stock_clerk | admin, stock_clerk | admin, stock_clerk |
| `/orders` | GetAll: admin, seller / GetById: autenticado | admin, purchaser | admin, seller | admin |
| `/orders/{orderId}/items` | autenticado | admin, purchaser | admin, seller | admin, seller |
| `/users` (criar) | — | admin | — | — |
| `/users/authenticate` | endpoint público | — | — | — |

## Endpoints

- `GET /customers?skip=0&take=10`, `GET /customers/{id}`, `POST /customers`, `PUT /customers/{id}`, `DELETE /customers/{id}`
- `GET /addresses?skip=0&take=10`, `GET /addresses/{id}`, `POST /addresses`, `PUT /addresses/{id}`, `DELETE /addresses/{id}`
- `GET /stores?skip=0&take=10`, `GET /stores/{id}`, `POST /stores`, `PUT /stores/{id}`, `DELETE /stores/{id}`
- `GET /products?skip=0&take=10`, `GET /products/{id}`, `POST /products`, `PUT /products/{id}`, `DELETE /products/{id}`
- `GET /orders?skip=0&take=10`, `GET /orders/{id}`, `POST /orders`, `PUT /orders/{id}`, `DELETE /orders/{id}`
- `GET /orders/{orderId}/items`, `POST /orders/{orderId}/items`, `PUT /orders/{orderId}/items/{productId}`, `DELETE /orders/{orderId}/items/{productId}`
- `POST /users` (cria usuário, requer role `admin`)
- `POST /users/authenticate?email=...&password=...` (retorna o token JWT)

Listagens (`GetAll`) suportam paginação via query string `skip` e `take` (padrão `skip=0`, `take=10`).

## Tecnologias

| Tecnologia | Uso |
|---|---|
| .NET 10 | Runtime |
| ASP.NET Core Minimal API | Camada HTTP |
| Entity Framework Core | ORM |
| Npgsql | Driver PostgreSQL |
| MediatR | Mediador para handlers de casos de uso |
| FluentValidation | Validação de entrada dos endpoints (`Command`s) |
| FluentResults | Implementação do Result pattern |
| JWT Bearer (Microsoft.AspNetCore.Authentication.JwtBearer) | Autenticação |
| Microsoft.Extensions.Identity.Core (PasswordHasher) | Hash de senha |
| BrDocuments | Validação de CPF/CNPJ |
| EmailValidation | Validação de e-mail |
| libphonenumber-csharp | Validação de telefone |
| Scalar | Interface OpenAPI |
| Serilog | Logging estruturado (console + arquivo) |
| MSTest | Testes unitários |

## Logs

Logging estruturado via **Serilog**.

- **Sinks:** Console e arquivo (`logs/app-.log`, rotação diária, retenção de 7 dias)
- **Enrichers:** `FromLogContext`, `WithMachineName`, `WithEnvironmentName`, `WithThreadId`
- **Request logging:** `UseSerilogRequestLogging` registra método, path, status code e tempo de resposta de cada requisição
- **`LogUsernameMiddleware`** — injeta o nome do usuário autenticado (claim `name`) no `LogContext`, associando os logs da requisição ao usuário que a originou
- **Handlers** — todos os handlers de `Store.Application` (`ILogger<Handler>`) logam entrada, falhas de validação/não encontrado (`LogWarning`) e conclusão com sucesso (`LogInformation`), usando placeholders estruturados (ex.: `{Id}`, `{Email}`) em vez de interpolação direta

Configuração em `appsettings.json`, seção `Serilog`.

## Testes

O projeto `Store.Test` cobre a camada `Store.Domain` com testes unitários (MSTest):

- **Entidades:** `Address`, `Customer`, `Order`, `OrderProduct`, `Product`, `Store`, `User`
- **Value Objects:** `Document`, `Email`, `Phone`, `Role`, `ZipCode`

## Configuração

Requer uma string de conexão PostgreSQL e uma chave JWT em `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=...;Database=...;Username=...;Password=..."
  },
  "JwtSecretKey": "..."
}
```
