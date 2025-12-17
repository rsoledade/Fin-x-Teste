using System;

namespace Finx.Api.DTOs
{
    public class PacienteDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime? DataNascimento { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string Contato { get; set; }
    }
}
