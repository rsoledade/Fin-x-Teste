# Desafio 1 — MVP

Este documento descreve o escopo mínimo (Obrigatório) e itens opcionais para o Desafio 1.

## Obrigatório
- Endpoints CRUD de Pacientes:
  - `GET /api/pacientes`
  - `GET /api/pacientes/{id}`
  - `POST /api/pacientes`
  - `PUT /api/pacientes/{id}`
  - `DELETE /api/pacientes/{id}`
- CRUD de Histórico Médico para paciente:
  - `GET /api/pacientes/{id}/historico`
  - `POST /api/pacientes/{id}/historico`
  - `PUT /api/pacientes/{id}/historico/{histId}`
  - `DELETE /api/pacientes/{id}/historico/{histId}`
- Autenticação JWT:
  - `POST /api/auth/login` retornando JWT (201)
  - Claims mínimos: `sub`, `roles` (roles: `User`, `Admin`)
- Endpoint para exames externos:
  - `GET /api/exames/{cpf}` — implementação mock ou integração com BrasilAPI
- Modelos básicos no domínio: `Paciente`, `HistoricoMedico`, `Hospital`, `PacienteHospital`, `Agendamento`
- Indexação: índice em `Cpf` e índice composto em `(HospitalId, Codigo)`
- Handlers via MediatR para comandos e queries principais
- Validações essenciais: CPF (formato) e dados obrigatórios
- Testes mínimos: unitário para validação de CPF e integração básica para criação/consulta de paciente

## Opcionais
- Integração real com serviço de exames (HTTP client configurado)
- Implementação de `IFileStorage` (Azure Blob) para anexos
- Paginação e filtros avançados em listagem de pacientes
- Políticas de retry/circuit-breaker (Polly) para chamadas externas
- Dockerização completa e `docker-compose` com DB
- Logs estruturados com Serilog e health checks

## Observações
- Não commitar segredos; usar `dotnet user-secrets` ou variáveis de ambiente
- Expirações de token curtas para produção; algoritmo HS256
- Documentar DTOs e códigos de resposta (200/201/400/401/404)
