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
        public string Registrar(EN_Herramienta obj, out string Mensaje)//out indica parametro de salida
        {
            string IdAutogenerado = "0"; /*Recibe el id autogenerado*/

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarHerramienta", oConexion);
                    cmd.Parameters.AddWithValue("IdHerramienta", obj.idHerramienta);
                    cmd.Parameters.AddWithValue("Nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("Cantidad", obj.cantidad);
                    cmd.Parameters.AddWithValue("IDMarca", obj.id_marcaHerramienta.idMarca);
                    cmd.Parameters.AddWithValue("IDCategoria", obj.id_categoHerramienta.idCategoria);
                    cmd.Parameters.AddWithValue("Observaciones", obj.observaciones);
                    cmd.Parameters.AddWithValue("Activo", obj.activo);
                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    IdAutogenerado = cmd.Parameters["Resultado"].Value.ToString();
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                IdAutogenerado = "0";
                Mensaje = ex.Message;

            }
            return IdAutogenerado;

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
                    sqlCommand.Parameters.AddWithValue("@IDMarca", herramienta.id_marcaHerramienta.idMarca);
                    sqlCommand.Parameters.AddWithValue("@IDCategoria", herramienta.id_categoHerramienta.idCategoria);
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

        public bool Eliminar(string id, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarHerramienta", oConexion);
                    cmd.Parameters.AddWithValue("IdHerramienta", id);
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

        public List<EN_Herramienta> listarHerramientas()
        {
            List<EN_Herramienta> lista = new List<EN_Herramienta>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    string query = "SELECT IdHerramienta, id_marca, id_categoria, nombre, cantidad,marca_herramienta.Descripcion as Desc_Marca, categoria_herramienta.Descripcion as Desc_Categoria, herramienta.FechaRegistro, observaciones, herramienta.activo FROM herramienta inner join marca_herramienta on marca_herramienta.IdMarca = herramienta.id_marca inner join categoria_herramienta on categoria_herramienta.IdCategoria = herramienta.id_categoria order by FechaRegistro desc";
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
                                    idHerramienta = dr["IdHerramienta"].ToString(),
                                    nombre = dr["nombre"].ToString(),
                                    cantidad = Convert.ToInt32(dr["cantidad"]),
                                    id_marcaHerramienta = new EN_MarcaHerramienta
                                    {
                                        idMarca = Convert.ToInt32(dr["id_marca"]),
                                        descripcion = dr["Desc_Marca"].ToString()
                                    },
                                    id_categoHerramienta = new EN_CategoriaHerramienta
                                    {
                                        idCategoria = Convert.ToInt32(dr["id_categoria"]),
                                        descripcion = dr["Desc_Categoria"].ToString()
                                    },
                                   
                                    observaciones = dr["observaciones"].ToString(),
                                     activo = Convert.ToBoolean(dr["activo"])

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
                                    idHerramienta = dr["IdHerramienta"].ToString(),
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
