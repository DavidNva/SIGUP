using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;

namespace CapaDatos
{
    public class BD_MarcaHerramienta
    {
        public int añadir_marca(EN_MarcaHerramienta marca, out string mensaje)
        {
            int idAutogenerado = 0;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand sqlCommand = new SqlCommand("sp_RegistrarMarca", conexion);
                    sqlCommand.Parameters.AddWithValue("@DescripcionMarca", marca.descripcion);
                    sqlCommand.Parameters.AddWithValue("@Activo", marca.activo);

                    sqlCommand.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    conexion.Open();
                    sqlCommand.ExecuteNonQuery();
                    idAutogenerado = Convert.ToInt32(sqlCommand.Parameters["Resultado"].Value);
                    mensaje = sqlCommand.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idAutogenerado = 0;/*Regresa a 0*/
                mensaje = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return idAutogenerado;
        }

        public bool modificar_marca(EN_MarcaHerramienta marca, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand sqlCommand = new SqlCommand("sp_editarMarca", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@IdMarca", marca.idMarca);
                    sqlCommand.Parameters.AddWithValue("@Descripcion", marca.descripcion);
                    sqlCommand.Parameters.AddWithValue("@Activo", marca.activo);

                    sqlCommand.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(sqlCommand.Parameters["Resultado"].Value);
                    mensaje = sqlCommand.Parameters["Mensaje"].Value.ToString();
                }
                    
                return resultado;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool eliminar_marca(int id, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand sqlCommand = new SqlCommand("sp_eliminarMarca", sqlConnection);
                    //sqlCommand.Parameters.AddWithValue("@IdMarca", id);
                    sqlCommand.Parameters.Add("@IdMarca", SqlDbType.Int);
                    sqlCommand.Parameters["@IdMarca"].Value = id;

                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    sqlCommand.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(sqlCommand.Parameters["Resultado"].Value);
                    mensaje = sqlCommand.Parameters["Mensaje"].Value.ToString();
                }
                return resultado;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<EN_MarcaHerramienta> Listar()
        {
            List<EN_MarcaHerramienta> lista = new List<EN_MarcaHerramienta>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    string query = "SELECT IdMarca, Descripcion, Activo FROM marca_herramienta ORDER BY IdMarca DESC";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva categorias a la lista*/
                                new EN_MarcaHerramienta()
                                {
                                    idMarca = Convert.ToInt32(dr["IdMarca"]),
                                    descripcion = dr["Descripcion"].ToString(),
                                    activo = Convert.ToBoolean(dr["Activo"])
                                });
                        }
                        Console.WriteLine(lista.Count);
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public List<EN_MarcaHerramienta> ListarMarcaEnHerramienta()
        {
            List<EN_MarcaHerramienta> lista = new List<EN_MarcaHerramienta>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    string query = "SELECT IdMarca, Descripcion FROM marca_herramienta";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(
                                new EN_MarcaHerramienta()
                                {
                                    idMarca = Convert.ToInt32(dr["IdMarca"]),
                                    descripcion = dr["Descripcion"].ToString()
                                });
                        }
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
         
    }
}
