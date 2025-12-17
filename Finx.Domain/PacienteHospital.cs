using System;

namespace Finx.Domain
{
    public class PacienteHospital
    {
        public Guid PacienteId { get; set; }
        public Guid HospitalId { get; set; }
        public string Codigo { get; set; }
    }
}
