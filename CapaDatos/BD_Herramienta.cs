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
    public class BD_Herramienta
    {
        public int añadir_herramienta(EN_Herramienta herramienta, out string mensaje)
        {
            int resultado;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand sqlCommand = new SqlCommand("sp_RegistrarHerramienta", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@IdHerramienta", herramienta.idHerramienta);
                    sqlCommand.Parameters.AddWithValue("@Nombre", herramienta.nombre);
                    sqlCommand.Parameters.AddWithValue("@Cantidad", herramienta.cantidad);
                    sqlCommand.Parameters.AddWithValue("@IDMarca", herramienta.marca.idMarca);
                    sqlCommand.Parameters.AddWithValue("@IDCategoria", herramienta.categoHerramienta.idCategoria);
                    sqlCommand.Parameters.AddWithValue("@Observaciones", herramienta.observaciones);
                    sqlCommand.Parameters.AddWithValue("@Activo", herramienta.activo);

                    sqlCommand.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    resultado = Convert.ToInt32(sqlCommand.Parameters["Resultado"].Value);
                    mensaje = sqlCommand.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = 0;/*Regresa a 0*/
                mensaje = ex.Message;
                Console.WriteLine(ex.Message);
            }
            return resultado;
        }

        public bool modificar_herramienta(EN_Herramienta herramienta, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand sqlCommand = new SqlCommand("sp_EditarHerramienta", sqlConnection);

                    sqlCommand.Parameters.AddWithValue("@IdHerramienta", herramienta.idHerramienta);
                    sqlCommand.Parameters.AddWithValue("@Nombre", herramienta.nombre);
                    sqlCommand.Parameters.AddWithValue("@Cantidad", herramienta.cantidad);
                    sqlCommand.Parameters.AddWithValue("@IDMarca", herramienta.marca);
                    sqlCommand.Parameters.AddWithValue("@IDCategoria", herramienta.idHerramienta);
                    sqlCommand.Parameters.AddWithValue("@Observaciones", herramienta.observaciones);
                    sqlCommand.Parameters.AddWithValue("@Activo", herramienta.activo);

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

        public bool eliminar_herramienta(int id, out string mensaje)
        {
            bool resultado;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand sqlCommand = new SqlCommand("sp_EliminarHerramienta", sqlConnection);
                    //sqlCommand.Parameters.AddWithValue("@IdMarca", id);
                    sqlCommand.Parameters.Add("@IdHerramienta", SqlDbType.Int);
                    sqlCommand.Parameters["@IdHerramienta"].Value = id;

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

        public List<EN_Herramienta> listarHerramientas()
        {
            List<EN_Herramienta> lista = new List<EN_Herramienta>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    string query = "SELECT IdHerramienta, id_marca, id_categoria, nombre, cantidad,marca_herramienta.Descripcion as Desc_Marca, categoria_herramienta.Descripcion as Desc_Categoria, herramienta.FechaRegistro, observaciones, herramienta.activo FROM herramienta inner join marca_herramienta on marca_herramienta.IdMarca = herramienta.id_marca inner join categoria_herramienta on categoria_herramienta.IdCategoria = herramienta.id_categoria";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva categorias a la lista*/
                                new EN_Herramienta()
                                {
                                    idHerramienta = Convert.ToInt32(dr["IdHerramienta"]),
                                    nombre = dr["nombre"].ToString(),
                                    cantidad = Convert.ToInt32(dr["cantidad"]),
                                    activo = Convert.ToBoolean(dr["activo"]),
                                    observaciones = dr["observaciones"].ToString(),
                                    marca = new EN_MarcaHerramienta
                                    {
                                        idMarca = Convert.ToInt32(dr["id_marca"]),
                                        descripcion = dr["Desc_Marca"].ToString()
                                    },
                                    categoHerramienta = new EN_CategoriaHerramienta
                                    {
                                        idCategoria = Convert.ToInt32(dr["id_categoria"]),
                                        descripcion = dr["Desc_Categoria"].ToString()
                                    }
                                }) ;
                        }
                        Console.WriteLine(lista.Count);
                    }
                }
                return lista;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public List<EN_Herramienta> ListarHerramientaParaPrestamo()
        {
            List<EN_Herramienta> lista = new List<EN_Herramienta>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {

                    string query = "select IdHerramienta, Nombre, Activo from Herramienta where Activo = 1";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega un nuevo Lector a la lista*/
                                new EN_Herramienta()
                                {
                                    idHerramienta = Convert.ToInt32(dr["IdHerramienta"]),
                                    nombre = dr["Nombre"].ToString(),
                                    activo = Convert.ToBoolean(dr["Activo"])
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Herramienta>();
            }

            return lista;
        }
    }
}
