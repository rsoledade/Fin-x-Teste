using System;

namespace Finx.Domain
{
    public class Hospital
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Cnpj { get; set; }
        public string Grupo { get; set; }
    }
}