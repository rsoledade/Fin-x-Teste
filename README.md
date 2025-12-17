# Fin-x-Teste

Teste de programação

## Como rodar via Docker (recomendado para avaliador)

1. Copie o arquivo de exemplo de variáveis de ambiente:

   - `cp docker/.env.example docker/.env` (ou crie `docker/.env` manualmente)
   - Edite `docker/.env` e defina `MSSQL_SA_PASSWORD` com senha forte. Não comite este arquivo.

2. Suba a stack via docker-compose:

   - `cd docker && docker-compose up --build`

   A API ficará disponível em `http://localhost:5000` e as migrations serão aplicadas automaticamente na inicialização (se houver migrations pendentes).

3. Health checks:

   - Readiness: `GET /health/ready`
   - Liveness: `GET /health/live`

## Como rodar localmente sem Docker

1. Abra a solução em sua IDE.
2. Execute `dotnet build` a partir de `src/`.
3. Para executar com InMemory DB (padrão de desenvolvimento):
   - `dotnet run --project Finx.Api` (vai usar InMemory quando `DefaultConnection` não estiver configurada).

## Migrations

- As migrations estão presentes em `src/Finx.Infrastructure/Migrations`.
- A API tenta aplicar migrations pendentes automaticamente na inicialização quando uma `ConnectionString:DefaultConnection` é fornecida.
- Se preferir aplicar manualmente:
  - Configure `ConnectionStrings:DefaultConnection` e execute `dotnet ef database update --project Finx.Infrastructure --startup-project Finx.Api`.

## Execução de testes

- `dotnet test` executa os testes em `Finx.Api.Tests`.

## Notas de segurança

- Nunca comite `docker/.env` ou outros arquivos que contenham segredos. Utilize variáveis de ambiente seguras no CI/avaliador.
- O `docker/.env.example` é apenas um exemplo de configuração.

## Conteúdo principal

- `src/Finx.Api` - API
- `src/Finx.Domain` - Modelos de domínio
- `src/Finx.Infrastructure` - EF Core, repositórios e migrations
- `src/Finx.Integrations` - Adaptadores e contratos de integração
- `src/Finx.Scripts` - Scripts SQL (ex.: `unify_pacientes.sql`)
- `docker/` - `docker-compose.yml` e `docker/.env.example`
