namespace Finx.Api.Model
{
    public class CreateHistoricoRequest
    {
        public string Diagnostico { get; set; } = string.Empty;
        public string Exame { get; set; } = string.Empty;
        public string Prescricao { get; set; } = string.Empty;
        public DateTime Data { get; set; }
    }
}
