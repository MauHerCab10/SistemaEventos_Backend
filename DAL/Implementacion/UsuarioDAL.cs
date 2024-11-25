using DAL.Contrato;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using Transversal.Modelos;

namespace Implementacion
{
    public class UsuarioDAL : IUsuarioDAL
    {
        private readonly string cadenaConexion;
        public UsuarioDAL(IConfiguration configuration)
        {
            cadenaConexion = configuration.GetConnectionString("ConexionSQL") ?? "";
        }

        //Patrón SINGLETON inservible ya q estamos usando Inyección de Dependencias, por ende, el contenedor de servicios manejará la creación y el ciclo de vida de las instancias
        #region Patrón de Diseño SINGLETON
        //// Instancia estática para Singleton
        //private static UsuarioDAL? _instancia;

        //// Objeto para bloquear el acceso concurrente
        //private static readonly object _lock = new object();

        //public static UsuarioDAL ObtenerInstancia(IConfiguration configuration)
        //{
        //    // Doble verificación de bloqueo (double-check locking) para asegurar la creación de una sola instancia (implementación 'thread-safe')
        //    if (_instancia == null)
        //    {
        //        lock (_lock)
        //        {
        //            if (_instancia == null)
        //            {
        //                _instancia = new UsuarioDAL(configuration);
        //            }
        //        }
        //    }
        //    return _instancia;
        //}
        #endregion

        public async Task<bool> RegistrarUsuario(Usuario usuario)
        {
            bool respuesta = false;

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand command = new SqlCommand("sp_RegistrarUsuario", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@NombreApellido", usuario.NombreApellido);
                        command.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                        command.Parameters.AddWithValue("@ContrasenaHash", usuario.Contrasena);

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

        public async Task<Usuario> ConsultarUsuario(string nombreUsuario, string? contrasenaHash = null)
        {
            Usuario? usuario = null;

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                using (SqlCommand command = new SqlCommand("sp_ConsultarUsuario", connection))
                {
                    try
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                        command.Parameters.AddWithValue("@ContrasenaHash", contrasenaHash ?? (object)DBNull.Value);

                        if (connection.State == ConnectionState.Closed)
                            await connection.OpenAsync();

                        using (SqlDataReader dr = await command.ExecuteReaderAsync())
                        {
                            if (dr.Read())
                            {
                                usuario = new Usuario
                                {
                                    IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                    NombreApellido = dr["NombreApellido"].ToString()!,
                                    NombreUsuario = dr["NombreUsuario"].ToString()!,
                                    Contrasena = dr["ContrasenaHash"].ToString()!
                                };
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

            return usuario!;
        }

    }
}
