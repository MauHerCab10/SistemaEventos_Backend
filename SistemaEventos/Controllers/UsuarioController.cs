using BLL.Contrato;
using Implementacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEventos.Comun;
using Transversal.Modelos;

namespace SistemaEventos.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly Utilidades _utilidades;
        private readonly IUsuarioBLL _usuarioBLL;

        public UsuarioController(Utilidades utilidades, IUsuarioBLL usuarioBLL)
        {
            _usuarioBLL = usuarioBLL;
            _utilidades = utilidades;
        }


        [HttpPost]
        [Route("RegistrarUsuario")]
        public async Task<IActionResult> RegistrarUsuario(Usuario usuario)
        {
            var usuarioEncontrado = await _usuarioBLL.ConsultarUsuario(usuario.NombreUsuario);

            if (usuarioEncontrado != null)
                return StatusCode(200, new { isSuccess = false, mensaje = "Usuario ya existe." });

            var nuevoUsuario = new Usuario
            {
                NombreApellido = usuario.NombreApellido,
                NombreUsuario = usuario.NombreUsuario,
                Contrasena = _utilidades.EncriptarContraseña(usuario.Contrasena)
            };

            bool registro = await _usuarioBLL.RegistrarUsuario(nuevoUsuario);

            if (registro)
                return StatusCode(200, new { isSuccess = true, mensaje = "¡Usuario creado satisfactoriamente!" });
            else
                return StatusCode(200, new { isSuccess = false, mensaje = "Error al crear el usuario." });
        }

        [HttpPost]
        [Route("LoginUsuario")]
        public async Task<IActionResult> LoginUsuario(Login usuario)
        {
            var usuarioEncontrado = await _usuarioBLL.ConsultarUsuario(usuario.NombreUsuario, _utilidades.EncriptarContraseña(usuario.Contrasena));

            if (usuarioEncontrado == null)
                return StatusCode(200, new { isSuccess = false, token = string.Empty, mensaje = "Favor validar los datos ingresados." });
            else
                return StatusCode(200, new { isSuccess = true, idUsuario = usuarioEncontrado.IdUsuario, token = _utilidades.GenerarJWT(usuarioEncontrado) });
        }

        [HttpGet]
        [Route("ValidarToken")]
        public IActionResult ValidarToken(string token)
        {
            bool esTokenValido = _utilidades.ValidarToken(token);
            return StatusCode(200, new { isSuccess = esTokenValido });
        }

    }
}
