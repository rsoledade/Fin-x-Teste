namespace Finx.Api.DTOs
{
    public class CreatePacienteDto
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Contato { get; set; }
    }
}
