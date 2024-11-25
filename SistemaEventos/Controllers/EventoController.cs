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
    [Authorize] //JWT
    [ApiController]
    public class EventoController : ControllerBase
    {
        private readonly IEventoBLL _eventoBLL;

        public EventoController(IEventoBLL eventoBLL)
        {
            _eventoBLL = eventoBLL;
        }

        [HttpGet]
        [Route("ConsultarEventosDisponibles")]
        public async Task<IActionResult> ConsultarEventosDisponibles(string idUsuario)
        {
            var respuesta = new Response<List<Evento>>();

            try
            {
                respuesta.status = true;
                respuesta.value = await _eventoBLL.ConsultarEventosDisponibles(idUsuario);
            }
            catch (Exception e)
            {
                respuesta.status = false;
                respuesta.mensaje = e.Message;
            }

            return Ok(respuesta);
        }

        [HttpPost]
        [Route("CrearEvento")]
        public async Task<IActionResult> CrearEvento(Evento evento)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.status = true;
                respuesta.value = await _eventoBLL.CrearEvento(evento);
            }
            catch (Exception e)
            {
                respuesta.status = false;
                respuesta.mensaje = e.Message;
            }

            return Ok(respuesta);
        }

        [HttpPut]
        [Route("ModificarEvento")]
        public async Task<IActionResult> ModificarEvento(Evento evento)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.status = true;
                respuesta.value = await _eventoBLL.ModificarEvento(evento);
            }
            catch (Exception e)
            {
                respuesta.status = false;
                respuesta.mensaje = e.Message;
            }

            return Ok(respuesta);
        }

        [HttpDelete]
        [Route("EliminarEvento")]
        public async Task<IActionResult> EliminarEvento(int idEvento)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.status = true;
                respuesta.value = await _eventoBLL.EliminarEvento(idEvento);
            }
            catch (Exception e)
            {
                respuesta.status = false;
                respuesta.mensaje = e.Message;
            }

            return Ok(respuesta);
        }

        [HttpPost]
        [Route("InscripcionAEvento")]
        public async Task<IActionResult> InscripcionAEvento(int idEvento, int idUsuario)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.status = true;
                respuesta.value = await _eventoBLL.InscripcionAEvento(idEvento, idUsuario);
            }
            catch (Exception e)
            {
                respuesta.status = false;
                respuesta.mensaje = e.Message;
            }

            return Ok(respuesta);
        }

        [HttpPost]
        [Route("DimisionDeEvento")]
        public async Task<IActionResult> DimisionDeEvento(int idEvento, int idUsuario)
        {
            var respuesta = new Response<bool>();

            try
            {
                respuesta.status = true;
                respuesta.value = await _eventoBLL.DimisionDeEvento(idEvento, idUsuario);
            }
            catch (Exception e)
            {
                respuesta.status = false;
                respuesta.mensaje = e.Message;
            }

            return Ok(respuesta);
        }

    }
}
