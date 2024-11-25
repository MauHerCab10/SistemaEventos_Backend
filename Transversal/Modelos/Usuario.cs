using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transversal.Modelos
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string NombreApellido { get; set; } = null!;
        
        public string NombreUsuario { get; set; } = null!;

        public string Contrasena { get; set; } = null!;

        public DateTime? FechaCreacion { get; set; }
    }
}
