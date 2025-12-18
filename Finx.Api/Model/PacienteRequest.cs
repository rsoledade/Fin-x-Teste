namespace Finx.Api.Model
{
    public class PacienteRequest
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string Contato { get; set; } = string.Empty;
    }
}
