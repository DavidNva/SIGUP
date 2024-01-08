using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
//using System.Data.SqlClient;
//using System.Data; /*Acceso a sql conections*/
using System.Globalization;
//using QuestPDF.Fluent;//Para exportar a pdf
//using QuestPDF.Helpers;
using System.IO;
using System.Reflection;

namespace CapaDatos
{
    public class BD_Prestamo
    {

        public List<EN_Prestamo> ListarPrestamosCompleto()
        {
            List<EN_Prestamo> lista = new List<EN_Prestamo>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {

                    StringBuilder sb = new StringBuilder(); //Permite hacer saltos de linea

                    sb.AppendLine("SELECT p.IdPrestamo, u.IdUsuario, CONCAT(u.Nombre, ' ', u.Apellidos)[NombreUsuario],");
                    sb.AppendLine("p.Cantidad as CantidadP, p.Unidad, p.CantidadPU,p.Area as Id_Area,a.NombreArea, p.CalificacionEntrega,");
                    sb.AppendLine("CONVERT(char(10), p.FechaPrestamo, 103)[FechaPrestamo],");
                    sb.AppendLine("CONVERT(char(10), p.FechaDevolucion, 103)[FechaDevolucion],");
                    sb.AppendLine("p.DiasDePrestamo, p.Notas, p.Activo,dp.IdHerramienta, h.IdHerramienta, h.Nombre AS NombreHerramienta,");
                    sb.AppendLine("dp.Cantidad as CantidadDP, dp.IdDetallePrestamo");
                    sb.AppendLine("from detalle_prestamo dp");
                    sb.AppendLine("INNER JOIN herramienta h ON h.IdHerramienta = dp.IdHerramienta");
                    sb.AppendLine("INNER JOIN Prestamo p ON p.IdPrestamo = dp.IdPrestamo");
                    sb.AppendLine("INNER JOIN Areas a on a.IdArea = p.Area");
                    sb.AppendLine("inner join usuario u on u.IdUsuario = p.IdUsuario order by DP.IdDetallePrestamo DESC");

                    //string query = "select * from fn_ListarPrestamos(@idLector)";

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    //cmd.Parameters.AddWithValue("@idLector", idLector);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva Libro la lista*/
                                new EN_Prestamo()
                                {
                                    idPrestamo = Convert.ToInt32(dr["IdPrestamo"]),
                                    id_Usuario = new EN_Usuario() { idUsuario = Convert.ToInt32(dr["IDUsuario"]), NombreCompletoUsuario = dr["NombreUsuario"].ToString() },
                                    cantidad = Convert.ToInt32(dr["CantidadP"]),
                                    fechaPrestamo = dr["FechaPrestamo"].ToString(),
                                    fechaDevolucion = dr["FechaDevolucion"].ToString(),
                                    diasPrestamo = Convert.ToInt32(dr["DiasDePrestamo"]),
                                    notas = dr["Notas"].ToString(),
                                    //Tipo = Convert.ToInt32(dr["Tipo"]),   
                                    activo = Convert.ToBoolean(dr["Activo"]),
                                    id_Herramienta = new EN_Herramienta() { idHerramienta = Convert.ToInt32(dr["IdHerramienta"]), nombre = dr["NombreHerramienta"].ToString() },
                                    id_Area = new EN_Area() { idArea = Convert.ToInt32(dr["Id_Area"]), nombreArea = dr["NombreArea"].ToString() },
                                    //oDetallePrestamo = new EN_DetallePrestamo() { IdDetallePrestamo = Convert.ToInt32(dr["IdDetallePrestamo"]), Total = dr["NombreLector"].ToString() },
                                });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Prestamo>();
            }

            return lista;
        }

        public bool Registrar(EN_Prestamo obj, DataTable DetallePrestamo,/* DataTable EjemplarActivo,*/ out string Mensaje)//out indica parametro de salida
        {
            bool respuesta = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarPrestamo", oConexion);
                    cmd.Parameters.AddWithValue("Id_Usuario", obj.id_Usuario.idUsuario);
                    cmd.Parameters.AddWithValue("CantidadTotal", obj.cantidad);
                    cmd.Parameters.AddWithValue("Unidad", obj.unidad);
                    cmd.Parameters.AddWithValue("CantidadPU", obj.cantidadPorUnidad);
                    cmd.Parameters.AddWithValue("AreaDeUso", obj.areaDeUso);
                    cmd.Parameters.AddWithValue("Id_Area", obj.id_Area.idArea);
                    cmd.Parameters.AddWithValue("DiasDePrestamo", obj.diasPrestamo);
                    cmd.Parameters.AddWithValue("Observaciones", obj.notas);
                    //SE AGREGO ESTA COLUMNA PARA APLICAR LA ELIMINACION EN CASCADA EN CASO DE QUE SE ELIMINE EL LIBRO
                    cmd.Parameters.AddWithValue("Id_Herramienta", obj.id_Herramienta.idHerramienta);

                    cmd.Parameters.AddWithValue("DetallePrestamo", DetallePrestamo);//El data table debe tener las mismas columnas de la estructura creada
                                                                                    //en sql (las 3 creadas: IdLibro, Cantidad, Total)

                    // cmd.Parameters.AddWithValue("EjemplarActivo", EjemplarActivo);

                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                respuesta = false;
                Mensaje = ex.Message;
            }
            return respuesta;
        }

        public bool AñadirPrestamo()
        {
            try
            {

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool ModificarPrestamo()
        {
            try
            {

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool EliminarPrestamo()
        {
            try
            {

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
