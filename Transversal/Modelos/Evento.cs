using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transversal.Modelos
{
    public class Evento
    {
        public int IdEvento { get; set; }

        public string? NombreEvento { get; set; }
        
        public string? Descripcion { get; set; }

        public string? Fecha { get; set; }

        public string? Hora { get; set; }

        public DateTime? FechaHora { get; set; }

        public string? Direccion_Ubicacion { get; set; }
        
        public int CapMaxPermitida { get; set; }

        public int IdUsuarioCreacion { get; set; }

        public int CantidadAsistentes { get; set; }

        public int CuposDisponibles { get; set; }

        public bool EsUsuarioInscrito { get; set; }
    }
}
