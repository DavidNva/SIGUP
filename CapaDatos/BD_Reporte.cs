using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class BD_Reporte
    {
        public EN_Dashboard VerDashBoard()
        {
            EN_Dashboard objeto = new EN_Dashboard();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ReporteDashboard", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            objeto = new EN_Dashboard
                            {
                                TotalUsuario = Convert.ToInt32(dr["TotalUsuario"]),
                                TotalPrestamo = Convert.ToInt32(dr["TotalPrestamo"]),
                                TotalHerramienta = Convert.ToInt32(dr["TotalHerramienta"]),
                                TotalEjemplaresHerramienta = Convert.ToInt32(dr["TotalEjemplaresHerramientas"]),
                            };
                        }
                    }
                }
            }
            catch (Exception)
            {
                objeto = new EN_Dashboard();
            }

            return objeto;
        }

        public List<EN_Reporte> Prestamos(string fechaInicio, string fechaFin, string codigoUsuario, string estado, string herramienta)
        {
            List<EN_Reporte> lista = new List<EN_Reporte>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {



                    //string query = "select Id  from Prestamo";
                    //StringBuilder sb = new StringBuilder();
                    //sb.AppendLine("select CONVERT(char(10), p.FechaPrestamo,103) [FechaPrestamo] , CONCAT(lc.Nombres, ' ', lc.Apellidos)[Lector],");
                    //sb.AppendLine("l.Titulo[Libro], dp.CantidadEjemplares, p.Estado, dp.Total,l.Codigo as IdLibro");
                    //sb.AppendLine("from DetallePrestamo dp");
                    //sb.AppendLine("inner join Libro l on l.Codigo = dp.IDLibro");
                    //sb.AppendLine("inner join Prestamo p on p.IdPrestamo = dp.IdPrestamo");
                    //sb.AppendLine("inner join Lector lc on lc.IdLector = p.Id_Lector");
                    //SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    //cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    //oConexion.Open();
                    SqlCommand cmd = new SqlCommand("sp_ReportePrestamos", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/
                    cmd.Parameters.AddWithValue("fechaInicio", fechaInicio);
                    cmd.Parameters.AddWithValue("fechaFin", fechaFin);
                    cmd.Parameters.AddWithValue("codigoUsuario", codigoUsuario);
                    cmd.Parameters.AddWithValue("estado", estado);
                    cmd.Parameters.AddWithValue("herramienta", herramienta);
                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega un nuevo usuario a la lista*/
                                new EN_Reporte()
                                {
                                    /*Lo que esta dentro de los corchetes es el nombre de la columna de la tabla generada con el procedimiento almacenado*/
                                    FechaPrestamo = dr["FechaPrestamo"].ToString(),
                                    Usuario = dr["Usuario"].ToString(),
                                    IdUsuario = dr["IdUsuario"].ToString(),
                                    Herramienta = dr["Herramienta"].ToString(),
                                    //Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),
                                    Cantidad = Convert.ToInt32(dr["Stock"]),//Checar este .tostring();
                                    Estado = Convert.ToBoolean(dr["Activo"]),//Devuelto = 1 o no devuelto = 0
                                    //Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-MX")),
                                    Codigo = dr["Codigo"].ToString()
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Reporte>();
            }

            return lista;
        }
    } 
}
