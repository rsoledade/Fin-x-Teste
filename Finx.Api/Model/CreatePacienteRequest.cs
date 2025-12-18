namespace Finx.Api.Model
{
    public class CreatePacienteRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string Contato { get; set; } = string.Empty;
    }
}
