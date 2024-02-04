using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class BD_Administrador
    {
        public List<EN_Administrador> ListarAdministrador()
        {
            List<EN_Administrador> lista = new List<EN_Administrador>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    string query = "select * from Administrador order by fechaRegistro desc ";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anterior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, irá agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega un nuevo Administrador a la lista*/
                                new EN_Administrador()
                                {
                                    idAdministrador = dr["IdAdministrador"].ToString(),
                                    nombres = dr["Nombres"].ToString(),
                                    apellidos = dr["Apellidos"].ToString(),
                                    telefono = dr["Telefono"].ToString(),
                                    correo = dr["Correo"].ToString(),
                                    clave = dr["Clave"].ToString(),
                                    reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                                    activo = Convert.ToBoolean(dr["Activo"])
                                }
                            );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Administrador>();
            }

            return lista;
        }

        public string Registrar(EN_Administrador obj, out string Mensaje)//out indica parametro de salida
        {
            string IdResultado = "0"; /*Recibe el id autogenerado*/

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarAdministrador", oConexion);
                    cmd.Parameters.AddWithValue("IdAdministrador", obj.idAdministrador);
                    cmd.Parameters.AddWithValue("Nombres", obj.nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.apellidos);
                    cmd.Parameters.AddWithValue("Telefono", obj.telefono);
                    cmd.Parameters.AddWithValue("Correo", obj.correo);
                    cmd.Parameters.AddWithValue("Clave", obj.clave);
                    cmd.Parameters.AddWithValue("Activo", obj.activo);
                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    IdResultado = cmd.Parameters["Resultado"].Value.ToString();
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                IdResultado = "0";
                Mensaje = ex.Message;

            }
            return IdResultado;

        }

        public bool Editar(EN_Administrador obj, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarAdministrador", oConexion);
                    cmd.Parameters.AddWithValue("IdAdministrador", obj.idAdministrador);
                    cmd.Parameters.AddWithValue("Nombres", obj.nombres);
                    cmd.Parameters.AddWithValue("Apellidos", obj.apellidos);
                    cmd.Parameters.AddWithValue("Telefono", obj.telefono);
                    cmd.Parameters.AddWithValue("Correo", obj.correo);
                    cmd.Parameters.AddWithValue("Activo", obj.activo);
                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        public bool Eliminar(string id, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarAdministrador", oConexion);
                    cmd.Parameters.AddWithValue("IdAdministrador", id);
                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        public bool CambiarClave(string idAdministrador, string nuevaClave, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update Administrador set clave = @nuevaClave, reestablecer = 0 where IdAdministrador = @Id", oConexion);
                    cmd.Parameters.AddWithValue("@Id", idAdministrador);
                    cmd.Parameters.AddWithValue("nuevaClave", nuevaClave);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    //El ExecuteNonQuery ejecuta una accion y devuelve el numero de filas afectadas
                    //Cuando eliminamos un registro de la tabla, entonces si el total de filas afectadas
                    //es mayor a 0 entonces será verdadero, pero si no es mayor a 0, entonces significa
                    //que hubo un problema al eliminar por lo que enviara un false, eso lo almacenamos en resultado
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        public bool ReestablecerClave(string idAdministrador, string clave, out string Mensaje)
        {//El administrador deberá nuevamente actualizar su contraseña
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update Administrador set clave = @clave, reestablecer = 1 where IdAdministrador = @Id", oConexion);
                    cmd.Parameters.AddWithValue("@Id", idAdministrador);
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    //El ExecuteNonQuery ejecuta una accion y devuelve el numero de filas afectadas
                    //Cuando eliminamos un registro de la tabla, entonces si el total de filas afectadas
                    //es mayor a 0 entonces será verdadero, pero si no es mayor a 0, entonces significa
                    //que hubo un problema al eliminar por lo que enviara un false, eso lo almacenamos en resultado
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

    }
}
