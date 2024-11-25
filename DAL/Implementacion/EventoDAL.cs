using DAL.Contrato;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Transversal.Modelos;

namespace Implementacion
{
    public class EventoDAL : IEventoDAL
    {
        private readonly string cadenaConexion;
        public EventoDAL(IConfiguration configuration)
        {
            cadenaConexion = configuration.GetConnectionString("ConexionSQL") ?? "";
        }

        //Patrón SINGLETON inservible ya q estamos usando Inyección de Dependencias, por ende, el contenedor de servicios manejará la creación y el ciclo de vida de las instancias
        #region Patrón de Diseño SINGLETON
        //// Instancia estática para Singleton
        //private static EventoDAL? _instancia;

        //// Objeto para bloquear el acceso concurrente
        //private static readonly object _lock = new object();

        //public static EventoDAL ObtenerInstancia(IConfiguration configuration)
        //{
        //    // Doble verificación de bloqueo (double-check locking) para asegurar la creación de una sola instancia (implementación 'thread-safe')
        //    if (_instancia == null)
        //    {
        //        lock (_lock)
        //        {
        //            if (_instancia == null)
        //            {
        //                _instancia = new EventoDAL(configuration);
        //            }
        //        }
        //    }
        //    return _instancia;
        //}
        #endregion

        public async Task<List<Evento>> ConsultarEventosDisponibles(string idUsuario)
        {
            List<Evento>? listaEventosDisponibles = null;

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand command = new SqlCommand("sp_ConsultarEventosDisponibles", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@IdUsuarioSolicitante", idUsuario);

                        if (connection.State == ConnectionState.Closed)
                            await connection.OpenAsync();

                        using (SqlDataReader dr = await command.ExecuteReaderAsync())
                        {
                            listaEventosDisponibles = new List<Evento>();

                            while (dr.Read())
                            {
                                Evento evento = new Evento
                                {
                                    IdEvento = Convert.ToInt32(dr["IdEvento"]),
                                    NombreEvento = dr["NombreEvento"].ToString()!,
                                    Descripcion = dr["Descripcion"].ToString()!,
                                    FechaHora = Convert.ToDateTime(dr["FechaHora"])!,
                                    Fecha = Convert.ToDateTime(dr["FechaHora"])!.ToString("dd/MM/yyyy"),
                                    Hora = Convert.ToDateTime(dr["FechaHora"])!.ToString("HH:mm"),
                                    Direccion_Ubicacion = dr["Direccion_Ubicacion"].ToString()!,
                                    CapMaxPermitida = Convert.ToInt32(dr["CapMaxPermitida"])!,
                                    IdUsuarioCreacion = Convert.ToInt32(dr["IdUsuarioCreacion"])!,
                                    CantidadAsistentes = Convert.ToInt32(dr["CantidadAsistentes"])!,
                                    CuposDisponibles = Convert.ToInt32(dr["CuposDisponibles"])!,
                                    EsUsuarioInscrito = dr["UsuarioInscrito"] != DBNull.Value ? true : false
                                };

                                listaEventosDisponibles.Add(evento);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                            await connection.CloseAsync();
                    }
                }
            }

            return listaEventosDisponibles!;
        }

        public async Task<bool> CrearEvento(Evento evento)
        {
            bool respuesta = false;

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand command = new SqlCommand("sp_CrearEvento", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@NombreEvento", evento.NombreEvento);
                        command.Parameters.AddWithValue("@Descripcion", evento.Descripcion);
                        command.Parameters.AddWithValue("@FechaHora", evento.FechaHora);
                        command.Parameters.AddWithValue("@Direccion_Ubicacion", evento.Direccion_Ubicacion);
                        command.Parameters.AddWithValue("@CapMaxPermitida", evento.CapMaxPermitida);
                        command.Parameters.AddWithValue("@IdUsuarioCreacion", evento.IdUsuarioCreacion);

                        if (connection.State == ConnectionState.Closed)
                            await connection.OpenAsync();

                        int regsAfectados = command.ExecuteNonQuery();

                        if (regsAfectados > 0)
                            respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }
                }
            }

            return respuesta;
        }

        public async Task<bool> ModificarEvento(Evento evento)
        {
            bool respuesta = false;

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand command = new SqlCommand("sp_ModificarEvento", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@IdEvento", evento.IdEvento);
                        command.Parameters.AddWithValue("@CapMaxPermitida", evento.CapMaxPermitida);
                        command.Parameters.AddWithValue("@FechaHora", evento.FechaHora);
                        command.Parameters.AddWithValue("@Direccion_Ubicacion", evento.Direccion_Ubicacion);

                        if (connection.State == ConnectionState.Closed)
                            await connection.OpenAsync();

                        int regsAfectados = command.ExecuteNonQuery();

                        if (regsAfectados > 0)
                            respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }
                }
            }

            return respuesta;
        }

        public async Task<bool> EliminarEvento(int idEvento)
        {
            bool respuesta = false;

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand command = new SqlCommand("sp_EliminarEvento", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdEvento", idEvento);

                        if (connection.State == ConnectionState.Closed)
                            await connection.OpenAsync();

                        int regsAfectados = command.ExecuteNonQuery();

                        if (regsAfectados > 0)
                            respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }
                }
            }

            return respuesta;
        }

        public async Task<bool> InscripcionAEvento(int idEvento, int idUsuario)
        {
            bool respuesta = false;

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand command = new SqlCommand("sp_InscripcionEvento", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdEvento", idEvento);
                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);

                        if (connection.State == ConnectionState.Closed)
                            await connection.OpenAsync();

                        int regsAfectados = command.ExecuteNonQuery();

                        if (regsAfectados > 0)
                            respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }
                }
            }

            return respuesta;
        }

        public async Task<bool> DimisionDeEvento(int idEvento, int idUsuario)
        {
            bool respuesta = false;

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand command = new SqlCommand("sp_DimisionDeEvento", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@IdEvento", idEvento);
                        command.Parameters.AddWithValue("@IdUsuario", idUsuario);

                        if (connection.State == ConnectionState.Closed)
                            await connection.OpenAsync();

                        int regsAfectados = command.ExecuteNonQuery();

                        if (regsAfectados > 0)
                            respuesta = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }
                }
            }

            return respuesta;
        }

    }
}
