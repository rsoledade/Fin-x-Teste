-- Script idempotente para unificacao de pacientes
-- Regra principal:
-- 1) Identificar duplicatas por CPF
-- 2) Identificar duplicatas por (HospitalId, Codigo) em PacienteHospital
-- 3) Manter o paciente com DataCadastro mais recente como sobrevivente
-- 4) Atualizar PacienteHospital e Agendamento para referenciar o paciente sobrevivente
-- 5) Remover pacientes duplicados
-- NOTA: Ajuste nomes de tabelas/colunas conforme o schema real do banco

BEGIN TRANSACTION;

-- 1) Unificar por CPF
;WITH Duplicates AS (
    SELECT
        p.Cpf,
        MIN(p.Id) OVER (PARTITION BY p.Cpf ORDER BY p.DataCadastro DESC) AS SurvivorId,
        p.Id AS DuplicateId,
        ROW_NUMBER() OVER (PARTITION BY p.Cpf ORDER BY p.DataCadastro DESC) AS rn
    FROM Paciente p
    WHERE p.Cpf IS NOT NULL AND p.Cpf <> ''
)
-- Atualiza referencias em PacienteHospital
UPDATE ph
SET PacienteId = d.SurvivorId
FROM PacienteHospital ph
JOIN Duplicates d ON ph.PacienteId = d.DuplicateId
WHERE d.rn > 1;

-- Atualiza referencias em Agendamento
UPDATE a
SET PacienteId = d.SurvivorId
FROM Agendamento a
JOIN Duplicates d ON a.PacienteId = d.DuplicateId
WHERE d.rn > 1;

-- Remove duplicados em PacienteHospital (caso haja entradas com mesmo HospitalId e Codigo para survivor)
DELETE ph
FROM PacienteHospital ph
JOIN Duplicates d ON ph.PacienteId = d.DuplicateId
WHERE d.rn > 1
  AND EXISTS (
    SELECT 1 FROM PacienteHospital ph2
    WHERE ph2.HospitalId = ph.HospitalId
      AND ph2.Codigo = ph.Codigo
      AND ph2.PacienteId = d.SurvivorId
  );

-- Remove pacientes duplicados
DELETE p
FROM Paciente p
JOIN Duplicates d ON p.Id = d.DuplicateId
WHERE d.rn > 1;

-- 2) Unificar por (HospitalId, Codigo) para casos sem CPF
;WITH HospitalDuplicates AS (
    SELECT
        ph.HospitalId,
        ph.Codigo,
        MIN(ph.PacienteId) OVER (PARTITION BY ph.HospitalId, ph.Codigo ORDER BY p.DataCadastro DESC) AS SurvivorId,
        ph.PacienteId AS DuplicateId,
        ROW_NUMBER() OVER (PARTITION BY ph.HospitalId, ph.Codigo ORDER BY p.DataCadastro DESC) AS rn
    FROM PacienteHospital ph
    JOIN Paciente p ON p.Id = ph.PacienteId
    WHERE (p.Cpf IS NULL OR p.Cpf = '')
)
-- Atualiza referencias em Agendamento
UPDATE a
SET PacienteId = hd.SurvivorId
FROM Agendamento a
JOIN HospitalDuplicates hd ON a.PacienteId = hd.DuplicateId
WHERE hd.rn > 1;

-- Remove duplicados em PacienteHospital (mantendo apenas a entrada do survivor)
DELETE ph
FROM PacienteHospital ph
JOIN HospitalDuplicates hd ON ph.PacienteId = hd.DuplicateId
WHERE hd.rn > 1
  AND EXISTS (
    SELECT 1 FROM PacienteHospital ph2
    WHERE ph2.HospitalId = ph.HospitalId
      AND ph2.Codigo = ph.Codigo
      AND ph2.PacienteId = hd.SurvivorId
  );

-- Remove pacientes duplicados sem CPF
DELETE p
FROM Paciente p
JOIN HospitalDuplicates hd ON p.Id = hd.DuplicateId
WHERE hd.rn > 1;

COMMIT TRANSACTION;

-- Observações:
-- - Este script é idempotente quando executado repetidamente, pois usa janelas para identificar survivors e duplicates.
-- - Recomenda-se executar em janela de manutenção com backup/pre-checks.
-- - Tests: executar primeiro em ambiente de homologação com transação e logs.
