using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transversal.Modelos;

namespace BLL.Contrato
{
    public interface IUsuarioBLL
    {
        Task<bool> RegistrarUsuario(Usuario usuario);

        Task<Usuario> ConsultarUsuario(string nombreUsuario, string? contrasenaHash = null);
    }
}
