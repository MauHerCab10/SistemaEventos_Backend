using BLL.Contrato;
using DAL.Contrato;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transversal.Modelos;

namespace Implementacion
{
    public class EventoBLL : IEventoBLL
    {
        private readonly IEventoDAL _eventoDAL;

        public EventoBLL(IEventoDAL eventoDAL)
        {
            _eventoDAL = eventoDAL;
        }

        public async Task<List<Evento>> ConsultarEventosDisponibles(string idUsuario)
        {
            List<Evento>? listaEventosDisponibles = await _eventoDAL.ConsultarEventosDisponibles(idUsuario);
            return listaEventosDisponibles;
        }

        public async Task<bool> CrearEvento(Evento evento)
        {
            string fechaHora = $"{evento.Fecha} {evento.Hora}";
            DateTime fechaCompleta = DateTime.ParseExact(fechaHora, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            evento.FechaHora = fechaCompleta;

            bool respuesta = await _eventoDAL.CrearEvento(evento);
            return respuesta;
        }

        public async Task<bool> ModificarEvento(Evento evento)
        {
            string fechaHora = $"{evento.Fecha} {evento.Hora}";
            DateTime fechaCompleta = DateTime.ParseExact(fechaHora, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            evento.FechaHora = fechaCompleta;

            bool respuesta = await _eventoDAL.ModificarEvento(evento);
            return respuesta;
        }

        public async Task<bool> EliminarEvento(int idEvento)
        {
            bool respuesta = await _eventoDAL.EliminarEvento(idEvento);
            return respuesta;
        }

        public async Task<bool> InscripcionAEvento(int idEvento, int idUsuario)
        {
            bool respuesta = await _eventoDAL.InscripcionAEvento(idEvento, idUsuario);
            return respuesta;
        }

        public async Task<bool> DimisionDeEvento(int idEvento, int idUsuario)
        {
            bool respuesta = await _eventoDAL.DimisionDeEvento(idEvento, idUsuario);
            return respuesta;
        }

    }
}
