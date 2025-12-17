using System;

namespace Finx.Domain
{
    public class HistoricoMedico
    {
        public Guid Id { get; set; }
        public Guid PacienteId { get; set; }
        public string Diagnostico { get; set; }
        public string Exame { get; set; }
        public string Prescricao { get; set; }
        public DateTime Data { get; set; }
    }
}
