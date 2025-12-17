using System;

namespace Finx.Domain.Entities
{
    public class Paciente
    {
        public Guid Id { get; set; }

        // Nome do paciente
        public string? Nome { get; set; }

        // CPF do paciente, no formato XXX.XXX.XXX-XX
        public string? Cpf { get; set; }

        // Data de nascimento do paciente
        public DateTime? DataNascimento { get; set; }

        // Data de cadastro do paciente
        public DateTime? DataCadastro { get; set; }

        // Contato do paciente, pode incluir telefone ou email
        public string? Contato { get; set; }
    }
}
