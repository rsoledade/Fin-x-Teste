using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finx.Infrastructure.Migrations
{
    public partial class UnifyPatientsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
-- Unification logic from script
BEGIN TRANSACTION;

;WITH Duplicates AS (
    SELECT
        p.Cpf,
        MIN(p.Id) OVER (PARTITION BY p.Cpf ORDER BY p.DataCadastro DESC) AS SurvivorId,
        p.Id AS DuplicateId,
        ROW_NUMBER() OVER (PARTITION BY p.Cpf ORDER BY p.DataCadastro DESC) AS rn
    FROM Pacientes p
    WHERE p.Cpf IS NOT NULL AND p.Cpf <> ''
)

UPDATE ph
SET PacienteId = d.SurvivorId
FROM PacienteHospitais ph
JOIN Duplicates d ON ph.PacienteId = d.DuplicateId
WHERE d.rn > 1;

UPDATE a
SET PacienteId = d.SurvivorId
FROM Agendamentos a
JOIN Duplicates d ON a.PacienteId = d.DuplicateId
WHERE d.rn > 1;

DELETE ph
FROM PacienteHospitais ph
JOIN Duplicates d ON ph.PacienteId = d.DuplicateId
WHERE d.rn > 1
  AND EXISTS (
    SELECT 1 FROM PacienteHospitais ph2
    WHERE ph2.HospitalId = ph.HospitalId
      AND ph2.Codigo = ph.Codigo
      AND ph2.PacienteId = d.SurvivorId
  );

DELETE p
FROM Pacientes p
JOIN Duplicates d ON p.Id = d.DuplicateId
WHERE d.rn > 1;

;WITH HospitalDuplicates AS (
    SELECT
        ph.HospitalId,
        ph.Codigo,
        MIN(ph.PacienteId) OVER (PARTITION BY ph.HospitalId, ph.Codigo ORDER BY p.DataCadastro DESC) AS SurvivorId,
        ph.PacienteId AS DuplicateId,
        ROW_NUMBER() OVER (PARTITION BY ph.HospitalId, ph.Codigo ORDER BY p.DataCadastro DESC) AS rn
    FROM PacienteHospitais ph
    JOIN Pacientes p ON p.Id = ph.PacienteId
    WHERE (p.Cpf IS NULL OR p.Cpf = '')
)

UPDATE a
SET PacienteId = hd.SurvivorId
FROM Agendamentos a
JOIN HospitalDuplicates hd ON a.PacienteId = hd.DuplicateId
WHERE hd.rn > 1;

DELETE ph
FROM PacienteHospitais ph
JOIN HospitalDuplicates hd ON ph.PacienteId = hd.DuplicateId
WHERE hd.rn > 1
  AND EXISTS (
    SELECT 1 FROM PacienteHospitais ph2
    WHERE ph2.HospitalId = ph.HospitalId
      AND ph2.Codigo = ph.Codigo
      AND ph2.PacienteId = hd.SurvivorId
  );

DELETE p
FROM Pacientes p
JOIN HospitalDuplicates hd ON p.Id = hd.DuplicateId
WHERE hd.rn > 1;

COMMIT TRANSACTION;
";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // No-op: data migration
        }
    }
}
