# Vehicle Sales API

API responsável pelo gerenciamento de veículos para venda e registro de compras realizadas por clientes autenticados.

O projeto foi desenvolvido utilizando .NET 10, seguindo princípios de Clean Architecture, separando responsabilidades entre Domínio, Aplicação, Infraestrutura e API.

## Objetivo

Disponibilizar uma API REST para:

- Cadastrar veículos para venda
- Atualizar dados de veículos
- Consultar veículo por identificador
- Listar veículos disponíveis para venda
- Listar veículos vendidos
- Efetivar a compra de um veículo
- Integrar autenticação de compradores através de serviço externo (AWS Cognito)

---

# Arquitetura

A solução está organizada nos seguintes projetos:

```text
src

|-- Domain
|   |-- Models
|   |-- Repositories
|   `-- Enums
|
|-- Application
|   |-- Requests
|   |-- UseCases
|   `-- Services
|
|-- Infrastructure
|   |-- Persistence
|   |   |-- Data
|   |   |-- Entity
|   |   |-- Mappings
|   |   `-- Repositories
|   |
|   `-- InfrastructureModuleDependency.cs
|
`-- WebApi
    |-- Controllers
    |-- Extensions
    |-- Middleware
    `-- Program.cs
```

## Responsabilidades

### Domain

Contém as regras de negócio e contratos da aplicação.

Exemplos:

- Vehicle
- Sale
- IVehicleRepository
- ISaleRepository

### Application

Responsável pelos casos de uso da solução.

Exemplos:

- VehicleUseCase
- SaleUseCase
- CreateVehicleRequest
- UpdateVehicleRequest
- PurchaseVehicleRequest

### Infrastructure

Responsável pelo acesso a dados utilizando Entity Framework Core e PostgreSQL.

Exemplos:

- AppDataContext
- VehicleRepository
- SaleRepository
- Entity Mappings

### WebApi

Camada responsável pela exposição da API REST.

Exemplos:

- Controllers
- Middleware
- Swagger
- Health Checks

---

# Tecnologias Utilizadas

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker
- Swagger / OpenAPI
- Health Checks
- AWS Cognito
- Kubernetes
- Terraform

---

# Requisitos

## SDK

Verificar instalação:

```bash
dotnet --version
```

Versão esperada:

```text
.NET SDK 10
```

## Docker

Verificar instalação:

```bash
docker --version
```

---

# Banco de Dados Local

A API utiliza PostgreSQL para persistência dos dados.

Para desenvolvimento local, utilize o Docker Compose:

```yaml
services:
  postgres:
    image: postgres:17

    environment:
      POSTGRES_DB: vehiclesales
      POSTGRES_USER: postgres
      POSTGRES_PASSWORD: postgres

    ports:
      - "5432:5432"

    volumes:
      - postgres_data:/var/lib/postgresql/data

volumes:
  postgres_data:
```

Subir o banco:

```bash
docker compose up -d
```

Verificar containers:

```bash
docker ps
```

---

# Configuração

## Desenvolvimento

Arquivo appsettings.Development.json:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=vehiclesales;Username=postgres;Password=postgres"
  }
}
```

## Produção

Em produção a conexão deverá apontar para o PostgreSQL hospedado no AWS RDS.

Exemplo:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=my-rds.amazonaws.com;Port=5432;Database=vehiclesales;Username=admin;Password=*****"
  }
}
```

---

# Instalação

Restaurar dependências:

```bash
dotnet restore
```

Compilar solução:

```bash
dotnet build
```

---

# Executando Localmente

Entrar no projeto WebApi:

```bash
cd WebApi
```

Executar:

```bash
dotnet run
```

---

# Swagger

Disponível em:

```text
https://localhost:{porta}/swagger
```

---

# Health Check

Endpoint utilizado para monitoramento da API:

```http
GET /health
```

Exemplo:

```http
GET https://localhost:5001/health
```

Resposta esperada:

```text
Healthy
```

---

# Entity Framework Migrations

Criar migration:

```bash
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project WebApi --output-dir Persistence/Data/Migrations
```

Aplicar migration:

```bash
dotnet ef database update --project Infrastructure --startup-project WebApi
```

Listar migrations:

```bash
dotnet ef migrations list --project Infrastructure --startup-project WebApi
```

Remover última migration:

```bash
dotnet ef migrations remove --project Infrastructure --startup-project WebApi
```

---

# Endpoints

## Veículos

### Criar veículo

```http
POST /v1/vehicle
```

Request:

```json
{
  "brand": "Toyota",
  "model": "Corolla",
  "year": 2024,
  "color": "Prata",
  "price": 120000
}
```

### Consultar veículo

```http
GET /v1/vehicle/{id}
```

### Atualizar veículo

```http
PATCH /v1/vehicle/{id}
```

Request:

```json
{
  "price": 115000
}
```

Todos os campos são opcionais.

### Listar veículos disponíveis

```http
GET /v1/vehicle/available
```

Retorno ordenado por preço crescente.

### Listar veículos vendidos

```http
GET /v1/vehicle/sold
```

Retorno ordenado por preço crescente.

---

## Vendas

### Realizar compra

```http
POST /v1/sale
```

Request:

```json
{
  "vehicleId": "a3bd43de-c3c4-4f64-b516-2c89fd9f768c"
}
```

Regras:

- Veículo deve existir
- Veículo deve estar disponível
- Usuário deve estar autenticado
- Após a compra o veículo passa para status Sold

---

# Modelo de Dados

## Vehicle

```json
{
  "id": "guid",
  "brand": "Toyota",
  "model": "Corolla",
  "year": 2024,
  "color": "Prata",
  "price": 120000,
  "status": "Available",
  "createdAt": "2026-08-03T10:00:00Z",
  "updatedAt": "2026-08-03T10:30:00Z"
}
```

## Sale

```json
{
  "id": "guid",
  "vehicleId": "guid",
  "buyerId": "guid",
  "salePrice": 120000,
  "purchasedAt": "2026-08-03T12:00:00Z"
}
```

---

# Autenticação

A autenticação não faz parte deste projeto.

O gerenciamento de usuários será realizado por um serviço externo utilizando AWS Cognito.

Fluxo esperado:

```text
Cliente
  |
  v
AWS Cognito
  |
 JWT
  |
  v
Vehicle Sales API
```

A API será responsável apenas por validar o token JWT e recuperar o identificador do usuário para efetivar a compra.

---

# CI/CD

Fluxo esperado para implantação:

```text
Pull Request
    |
    v
Build
    |
    v
Testes Automatizados
    |
    v
Build Docker Image
    |
    v
Amazon ECR
    |
    v
Deploy Kubernetes
```

---

# Infraestrutura

A infraestrutura será provisionada utilizando Terraform em repositórios independentes.

Componentes previstos:

- AWS VPC
- AWS EKS
- AWS RDS PostgreSQL
- AWS Cognito
- Security Groups
- IAM Roles
- Kubernetes Manifests

---

# Licença

Projeto desenvolvido por Gabriel Lima da Silva para fins de demonstração técnica e avaliação de arquitetura de software.