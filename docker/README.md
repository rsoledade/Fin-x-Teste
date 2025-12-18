# Docker Setup - Finx API

Este diretório contém a configuração Docker para executar a aplicação Finx localmente.

## Pré-requisitos

- Docker Desktop instalado
- Docker Compose v3.8+

## Configuração Rápida

1. **Copie o arquivo de exemplo de variáveis de ambiente:**
   ```bash
   cp .env.example .env
   ```

2. **Edite o arquivo `.env` se necessário** (já contém valores padrão para desenvolvimento)

3. **Execute o Docker Compose:**
   ```bash
   docker-compose up --build
   ```

## Serviços

A stack Docker inclui:

- **API** (porta 5000) - Aplicação .NET 9
- **SQL Server 2022** (porta 1433) - Banco de dados
- **Redis 7** (porta 6379) - Cache (opcional)

## Endpoints

Após iniciar os containers:

- API: http://localhost:5000
- Health Check (Ready): http://localhost:5000/health/ready
- Health Check (Live): http://localhost:5000/health/live
- OpenAPI/Swagger: http://localhost:5000/swagger (em Dev)

## Comandos Úteis

### Iniciar em background
```bash
docker-compose up -d
```

### Ver logs
```bash
docker-compose logs -f api
```

### Parar os serviços
```bash
docker-compose down
```

### Limpar tudo (incluindo volumes)
```bash
docker-compose down -v
```

### Rebuild da API
```bash
docker-compose up --build api
```

## Variáveis de Ambiente

| Variável | Descrição | Padrão |
|----------|-----------|--------|
| MSSQL_SA_PASSWORD | Senha do SQL Server | DevPassword!23 |
| MSSQL_DB | Nome do banco de dados | Finx |
| JWT_SECRET | Chave secreta JWT | dev_secret_change_me |

## Troubleshooting

### SQL Server não inicia
- Verifique se a senha no `.env` atende aos requisitos (8+ caracteres, maiúscula, minúscula, número, especial)
- Verifique os logs: `docker-compose logs sqlserver`

### API não conecta no banco
- Aguarde o SQL Server ficar "healthy": `docker-compose ps`
- Verifique a connection string nos logs da API

### Migrations não aplicadas
- As migrations são aplicadas automaticamente na inicialização
- Verifique os logs da API para erros de migration

## Produção

Para ambiente de produção:
1. Altere as senhas e secrets no `.env`
2. Configure TLS/HTTPS
3. Use um banco de dados gerenciado
4. Configure backup dos volumes
