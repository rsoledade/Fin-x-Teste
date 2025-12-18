# Finx-Test

Sistema de gerenciamento de pacientes e histórico médico com integração a sistemas externos.

---

## 📋 Visão Geral

Este projeto implementa uma solução completa para gestão de pacientes, histórico médico e integrações com sistemas hospitalares (HIS), desenvolvido seguindo as melhores práticas de arquitetura limpa, CQRS com MediatR, e padrões de segurança modernos.

### Desafios Implementados

#### ✅ Desafio 1: API RESTful Completa
- **CRUD de Pacientes**: Endpoints completos com validação de CPF
  - `GET /api/pacientes` - Listar pacientes (paginado)
  - `GET /api/pacientes/{id}` - Obter paciente por ID
  - `POST /api/pacientes` - Criar paciente
  - `PUT /api/pacientes/{id}` - Atualizar paciente
  - `DELETE /api/pacientes/{id}` - Excluir paciente (Admin apenas)

- **CRUD de Histórico Médico**: Endpoints completos com rotas aninhadas
  - `GET /api/pacientes/{pacienteId}/historico` - Listar histórico do paciente
  - `POST /api/pacientes/{pacienteId}/historico` - Adicionar registro
  - `PUT /api/pacientes/{pacienteId}/historico/{id}` - Atualizar registro
  - `DELETE /api/pacientes/{pacienteId}/historico/{id}` - Excluir registro

- **Autenticação JWT**: Sistema robusto de autenticação
  - `POST /api/auth/login` - Autenticação (retorna JWT)
  - Roles: `User` e `Admin`
  - Algoritmo: HS256 com claims personalizados

- **Integração com Exames Externos**:
  - `GET /api/exames/{cpf}` - Consulta exames externos (com fallback mock)
  - Implementação com Polly para resiliência (retry/circuit breaker)

#### ✅ Desafio 2: Integração FinX → HIS
Localização: `src/Finx.Integrations/` e `docs/`

- **Contratos e Interfaces**:
  - `IFileStorage` - Interface para armazenamento de arquivos (Azure Blob)
  - `IExameClient` - Interface para consulta de exames externos
  - DTOs de integração e exemplos de payloads

- **Adaptadores Implementados**:
  - `ExameHttpClient` - Cliente HTTP para API externa
  - `MockExameClient` - Cliente mock para testes
  - `ExameClientWithFallback` - Implementação com fallback automático
  - `LocalFileStorage` - Implementação local do IFileStorage

- **Documentação**:
  - Diagramas de integração em `docs/`
  - Exemplos de payloads e contratos
  - Observações sobre segurança (TLS, autenticação, mapeamento de IDs)

#### ✅ Desafio 3: Unificação de Pacientes Duplicados
Localização: `src/Finx.Scripts/unify_pacientes.sql`

- **Script SQL Idempotente** que:
  - Identifica duplicatas por CPF
  - Identifica duplicatas por código do paciente no hospital
  - Mantém registro com `DataCadastro` mais recente (sobrevivente)
  - Atualiza referências em `PacienteHospital` e `Agendamento`
  - Executa em transação com checagens de integridade
  - Pode ser executado múltiplas vezes sem efeitos colaterais

---

## 🏗️ Arquitetura

### Camadas do Projeto

```
src/
├── Finx.Api/              # Camada de apresentação (Controllers, DTOs, Handlers)
├── Finx.Domain/           # Camada de domínio (Entidades, Interfaces)
├── Finx.Infrastructure/   # Camada de infraestrutura (EF Core, Repositórios)
├── Finx.Integrations/     # Integrações externas (Contratos, Adaptadores)
├── Finx.Scripts/          # Scripts SQL (unificação, migrations)
└── Finx.Tests/            # Testes unitários e de integração
```

### Padrões Implementados

- **CQRS com MediatR**: Separação clara de comandos e queries
- **Repository Pattern**: Abstração da camada de dados
- **Dependency Injection**: Injeção de dependências nativa do ASP.NET Core
- **Validation Pipeline**: Validações com FluentValidation integradas ao MediatR
- **Clean Architecture**: Separação em camadas com dependências unidirecionais

### Tecnologias Utilizadas

- **.NET 9.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 8.0**
- **MediatR 11.1** (CQRS)
- **FluentValidation 11.4** (Validações)
- **JWT Bearer Authentication**
- **Serilog** (Logs estruturados)
- **Polly** (Resiliência e retry policies)
- **xUnit + Moq** (Testes)
- **Docker & Docker Compose**
- **Swagger (Swashbuckle)**

---

## 🚀 Como Executar

### Opção 1: Via Docker (Recomendado para Avaliadores)

Esta opção **não exige SQL Server instalado na máquina**, pois o banco sobe via container.

1. **Suba a stack completa** (na raiz do repositório, onde está o `docker-compose.yml`):
   ```bash
   docker compose up --build
   ```

2. **Acesse**:
   - API: `http://localhost:8080`
   - Swagger UI: `http://localhost:8080/swagger`

#### O que acontece no startup

- O serviço `sqlserver` (SQL Server 2022 Developer) sobe primeiro.
- O serviço `finx-api` aguarda o SQL Server ficar pronto.
- A API aplica automaticamente:
  - criação do banco (caso não exista)
  - migrations pendentes (criação da estrutura/tabelas)

> Observação: no Docker a connection string é injetada via variável de ambiente (`ConnectionStrings__DefaultConnection`).

### Opção 2: Execução Local

1. **Pré-requisitos**:
   - .NET 9.0 SDK
   - SQL Server acessível na máquina (ou ajuste a connection string)

2. **Restore e Build**:
   ```bash
   cd src
   dotnet restore
   dotnet build
   ```

3. **Execute a API**:
   ```bash
   dotenv run -e ../.env dotnet run --project Finx.Api
   ```

- Swagger UI: `http://localhost:5140/swagger` (ou `https://localhost:7109/swagger`)

Em desenvolvimento local, a connection string está em `Finx.Api/appsettings.Development.json`.

---

## 🧪 Testes

### Executar Todos os Testes
```bash
cd src
dotnet test
```

### Cobertura de Testes

- **Testes Unitários**:
  - Validação de CPF (casos válidos, inválidos, dígitos repetidos)
  - Handlers de criação, atualização e exclusão de pacientes
  - Handlers de histórico médico
  - Validadores FluentValidation

- **Testes de Integração** (em desenvolvimento):
  - Fluxo completo de autenticação + criação de paciente
  - Endpoints de exames externos com fallback

### Estrutura de Testes
```
Finx.Tests/
├── Unit/
│   ├── CreatePacienteCommandHandlerTests.cs
│   ├── UpdatePacienteCommandHandlerTests.cs
│   ├── DeletePacienteCommandHandlerTests.cs
│   └── ...
└── Integration/
    ├── AuthIntegrationTests.cs
    └── ExamesIntegrationTests.cs
```

---

## 🗄️ Banco de Dados

### Migrations

As migrations são aplicadas automaticamente na inicialização quando `ConnectionStrings:DefaultConnection` está configurada.

**Aplicar manualmente**:
```bash
dotnet ef database update --project Finx.Infrastructure --startup-project Finx.Api
```

**Criar nova migration**:
```bash
dotnet ef migrations add NomeDaMigration --project Finx.Infrastructure --startup-project Finx.Api
```

### Script de Unificação

Execute o script SQL para unificar pacientes duplicados:
```bash
# Localização: src/Finx.Scripts/unify_pacientes.sql
# Execute no seu SQL Server Management Studio ou via CLI
```

---

## 🔐 Segurança

### Práticas Implementadas

- ✅ JWT com claims mínimos (`sub`, `roles`)
- ✅ Roles `User` e `Admin` para autorização por endpoint
- ✅ Algoritmo HS256 para assinatura de tokens
- ✅ Senhas e segredos NUNCA commitados (uso de `user-secrets` e variáveis de ambiente)
- ✅ Validação de CPF com algoritmo de dígitos verificadores
- ✅ Proteção contra ataques de injeção via ORM (EF Core)
- ✅ HTTPS configurável para produção

### Variáveis de Ambiente

**Obrigatórias**:
- `Jwt__Secret` - Chave secreta para assinatura JWT
- `ConnectionStrings__DefaultConnection` - String de conexão do banco

**Opcionais**:
- `ExternalExamesApi__BaseUrl` - URL da API externa de exames
- `ExternalExamesApi__ApiKey` - Chave de API externa

---

## 📊 Observabilidade

### Health Checks

- **Readiness**: `GET /health/ready` - Verifica se a API está pronta (DB conectado)
- **Liveness**: `GET /health/live` - Verifica se a API está respondendo

### Logs Estruturados

Implementado com Serilog, incluindo:
- Logs de requisições HTTP
- Logs de erros com stack trace
- Logs de validação
- Logs de integração externa

### Resiliência

- **Retry Policy**: 3 tentativas com backoff exponencial para chamadas externas
- **Circuit Breaker**: Proteção contra falhas em cascata
- **Timeouts**: Configurados em todos os HttpClients

---

## 📝 Documentação Adicional

### Postman Collection

Importe a collection em `docs/postman_collection.json` para testar todos os endpoints.

### Swagger/OpenAPI

- Local: `http://localhost:5140/swagger`
- Docker: `http://localhost:8080/swagger`

### Diagramas

Consulte `docs/` para:
- Diagrama de integração FinX → HIS
- Fluxo de autenticação JWT
- Modelo de dados (ER)

---

## ✅ Checklist de Qualidade

- ✅ `dotnet build` sem erros
- ✅ `dotnet test` todos os testes passando
- ✅ `dotnet format` aplicado
- ✅ Sem segredos no repositório
- ✅ README atualizado com mapeamento dos desafios
- ✅ Docker e docker-compose funcionais
- ✅ Health checks implementados
- ✅ Validações de CPF testadas
- ✅ Script SQL idempotente documentado

---

## 🤝 Contribuição

Este é um projeto de teste técnico. Para sugestões ou melhorias, abra uma issue ou PR.

---

## 📄 Licença

Este projeto é para fins de avaliação técnica.

---

## 📞 Contato

Para dúvidas sobre a implementação, consulte a documentação interna ou entre em contato com o desenvolvedor.
