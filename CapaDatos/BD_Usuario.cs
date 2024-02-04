using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaEntidad;

namespace CapaDatos
{
    public class BD_Usuario
    {
        public List<EN_Usuario> ListarUsuarios()
        {
            List<EN_Usuario> usuarios = new List<EN_Usuario>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(BD_Conexion.cn))
                {
                    string query = "SELECT * FROM usuario INNER JOIN tipo_usuario ON tipo_usuario.IdTipo = usuario.Tipo";
                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.Text;
                        sqlConnection.Open();
                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                EN_Usuario usuario = new EN_Usuario
                                {
                                    idUsuario = Convert.ToInt32(sqlDataReader["IdUsuario"]),
                                    Nombre = sqlDataReader["Nombre"].ToString(),
                                    Apellidos = sqlDataReader["Apellidos"].ToString(),
                                    tipoUsuario = new EN_TipoUsuario
                                    {
                                        idTipo = Convert.ToInt32(sqlDataReader["IdTipo"]),
                                        nombre = sqlDataReader["nombre_tipo"].ToString()
                                    }
                                };
                                usuarios.Add(usuario);
                            }
                        }
                    }
                }
                return usuarios;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public string AñadirUsuario(EN_Usuario usuario)
        {
            string resultado;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(BD_Conexion.cn))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("sp_RegistrarUsuario", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@IdUsuario", usuario.idUsuario);
                        sqlCommand.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        sqlCommand.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                        sqlCommand.Parameters.AddWithValue("@TipoUsuario", usuario.tipoUsuario.idTipo);
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlConnection.Open();
                        resultado = sqlCommand.ExecuteNonQuery().ToString();
                    }
                }
                return resultado;
            }
            catch (SqlException sqlex)
            {
                return string.Format("Error de sql: {0} Código de error: {1} SI HAS RECIBIDO ESTE ERROR COMUNICATE CON LOS DESARROLLADORES ENVIANDO CAPTURA DE EL MISMO", sqlex.Message, sqlex.ErrorCode);
            }
            catch (Exception ex)
            {
                return string.Format("Error aún no controlado {0}", ex.Message);
            }
        }

        public string EditarUsuario(EN_Usuario usuario)
        {
            string resultado;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(BD_Conexion.cn))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("sp_EditarUsuario", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@IdUsuario", usuario.idUsuario);
                        sqlCommand.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                        sqlCommand.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                        sqlCommand.Parameters.AddWithValue("@TipoUsuario", usuario.tipoUsuario.idTipo);
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlConnection.Open();
                        resultado = sqlCommand.ExecuteNonQuery().ToString();
                    }
                }
                return resultado;
            }
            catch (SqlException sqlex)
            {
                return resultado = string.Format("Error de sql: {0} Código de error: {1} SI HAS RECIBIDO ESTE ERROR COMUNICATE CON LOS DESARROLLADORES ENVIANDO CAPTURA DE EL MISMO", sqlex.Message, sqlex.ErrorCode);
            }catch(Exception ex)
            {
                return resultado = string.Format("Error no controlado: {0}", ex.Message);
            }
        }

        public string EliminarUsuario(int idUsuario)
        {
            string resultado;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(BD_Conexion.cn))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("sp_EliminarUsuario", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@idUsuario", idUsuario);
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlConnection.Open();
                        resultado = sqlCommand.ExecuteNonQuery().ToString();
                    }
                }
                return resultado;
            }
            catch (SqlException sqlex)
            {
                return resultado = string.Format("Error de sql: {0} Código de error: {1} SI HAS RECIBIDO ESTE ERROR COMUNICATE CON LOS DESARROLLADORES ENVIANDO CAPTURA DE EL MISMO", sqlex.Message, sqlex.ErrorCode);
            }catch(Exception ex)
            {
                return resultado = string.Format("Error no controlado: {0}", ex.Message);
            }
        }

        public List<EN_Usuario> ListarUsuarioParaPrestamo()
        {
            List<EN_Usuario> lista = new List<EN_Usuario>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    //string query = "select IDUsuario, CONCAT(Nombre,' ',Apellidos) [NombreUsuario],  Activo from Usuario where Activo  = 1";
                    string query = "select IDUsuario, CONCAT(Nombre,' ',Apellidos) [NombreUsuario] from Usuario";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega un nuevo Lector a la lista*/
                                new EN_Usuario()
                                {
                                    idUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                    NombreCompletoUsuario = dr["NombreUsuario"].ToString()
                                    //activo = Convert.ToBoolean(dr["Activo"])
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Usuario>();
            }

            return lista;
        }
         
    }
}
