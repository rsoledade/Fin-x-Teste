using System;

namespace Finx.Api.DTOs
{
    public class HistoricoDto
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public string Diagnostico { get; set; }
        public string Exame { get; set; }
        public string Prescricao { get; set; }
        public DateTime Data { get; set; }
    }
}
