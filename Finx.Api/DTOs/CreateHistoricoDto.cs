using System;

namespace Finx.Api.DTOs
{
    public class CreateHistoricoDto
    {
        public string Diagnostico { get; set; } = string.Empty;
        public string Exame { get; set; } = string.Empty;
        public string Prescricao { get; set; } = string.Empty;
        public DateTime Data { get; set; }
    }
}
