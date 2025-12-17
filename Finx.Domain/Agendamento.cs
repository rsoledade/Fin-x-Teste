using System;

namespace Finx.Domain
{
    public class Agendamento
    {
        public Guid Id { get; set; }
        public Guid HospitalId { get; set; }
        public Guid PacienteId { get; set; }
        public DateTime Data { get; set; }
    }
}
