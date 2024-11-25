using BLL.Contrato;
using DAL.Contrato;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transversal.Modelos;

namespace Implementacion
{
    public class UsuarioBLL : IUsuarioBLL
    {
        private readonly IUsuarioDAL _usuarioDAL;

        public UsuarioBLL(IUsuarioDAL usuarioDAL)
        {
            _usuarioDAL = usuarioDAL;
        }

        public async Task<bool> RegistrarUsuario(Usuario usuario)
        {
            bool respuesta = await _usuarioDAL.RegistrarUsuario(usuario);
            return respuesta;
        }

        public async Task<Usuario> ConsultarUsuario(string nombreUsuario, string? contrasenaHash = null)
        {
            Usuario usuario = await _usuarioDAL.ConsultarUsuario(nombreUsuario, contrasenaHash);
            return usuario;
        }

    }
}
