using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transversal.Modelos;

namespace BLL.Contrato
{
    public interface IEventoBLL
    {
        Task<List<Evento>> ConsultarEventosDisponibles(string idUsuario);

        Task<bool> CrearEvento(Evento evento);

        Task<bool> ModificarEvento(Evento evento);

        Task<bool> EliminarEvento(int idEvento);

        Task<bool> InscripcionAEvento(int idEvento, int idUsuario);

        Task<bool> DimisionDeEvento(int idEvento, int idUsuario);
    }
}
