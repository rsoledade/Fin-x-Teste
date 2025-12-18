-- Idempotent script to create initial schema and indexes for Finx
-- Compatible with SQL Server (T-SQL)

SET NOCOUNT ON;

-- Create Paciente table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Paciente]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Paciente]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        [Nome] NVARCHAR(200) NOT NULL,
        [Cpf] NVARCHAR(20) NULL,
        [DataNascimento] DATETIME2 NULL,
        [DataCadastro] DATETIME2 NULL,
        [Contato] NVARCHAR(200) NULL
    );
END
GO

-- Create HistoricoMedico table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HistoricoMedico]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[HistoricoMedico]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        [PacienteId] UNIQUEIDENTIFIER NOT NULL,
        [Diagnostico] NVARCHAR(1000) NULL,
        [Exame] NVARCHAR(500) NULL,
        [Prescricao] NVARCHAR(1000) NULL,
        [Data] DATETIME2 NULL
    );
    ALTER TABLE [dbo].[HistoricoMedico] ADD CONSTRAINT FK_Historico_Paciente FOREIGN KEY (PacienteId) REFERENCES [dbo].[Paciente](Id);
END
GO

-- Create Hospital table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Hospital]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Hospital]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        [Nome] NVARCHAR(200) NOT NULL,
        [Cnpj] NVARCHAR(20) NULL,
        [Grupo] NVARCHAR(100) NULL
    );
END
GO

-- Create PacienteHospital table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PacienteHospital]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[PacienteHospital]
    (
        [PacienteId] UNIQUEIDENTIFIER NOT NULL,
        [HospitalId] UNIQUEIDENTIFIER NOT NULL,
        [Codigo] NVARCHAR(100) NOT NULL,
        CONSTRAINT PK_PacienteHospital PRIMARY KEY (PacienteId, HospitalId)
    );
    ALTER TABLE [dbo].[PacienteHospital] ADD CONSTRAINT FK_PacienteHospital_Paciente FOREIGN KEY (PacienteId) REFERENCES [dbo].[Paciente](Id);
    ALTER TABLE [dbo].[PacienteHospital] ADD CONSTRAINT FK_PacienteHospital_Hospital FOREIGN KEY (HospitalId) REFERENCES [dbo].[Hospital](Id);
END
GO

-- Create Agendamento table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Agendamento]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Agendamento]
    (
        [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        [HospitalId] UNIQUEIDENTIFIER NOT NULL,
        [PacienteId] UNIQUEIDENTIFIER NOT NULL,
        [Data] DATETIME2 NOT NULL
    );
    ALTER TABLE [dbo].[Agendamento] ADD CONSTRAINT FK_Agendamento_Hospital FOREIGN KEY (HospitalId) REFERENCES [dbo].[Hospital](Id);
    ALTER TABLE [dbo].[Agendamento] ADD CONSTRAINT FK_Agendamento_Paciente FOREIGN KEY (PacienteId) REFERENCES [dbo].[Paciente](Id);
END
GO

-- Create index on Paciente.Cpf if not exists
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_Paciente_Cpf' AND object_id = OBJECT_ID(N'[dbo].[Paciente]'))
BEGIN
    CREATE INDEX IX_Paciente_Cpf ON [dbo].[Paciente]([Cpf]);
END
GO

-- Create index on PacienteHospital(HospitalId, Codigo) if not exists
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = N'IX_PacienteHospital_Hospital_Codigo' AND object_id = OBJECT_ID(N'[dbo].[PacienteHospital]'))
BEGIN
    CREATE INDEX IX_PacienteHospital_Hospital_Codigo ON [dbo].[PacienteHospital]([HospitalId], [Codigo]);
END
GO

PRINT 'Schema creation script executed successfully.';
