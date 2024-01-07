using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using QuestPDF.Fluent;//Para exportar a pdf
//using QuestPDF.Helpers;
using System.IO;

namespace CapaDatos
{
    public class BD_Area
    {
        public List<EN_Area> ListarAreaParaPrestamo()
        {
            List<EN_Area> lista = new List<EN_Area>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    //string query = "select IDArea, CONCAT(Nombre,' ',Apellidos) [NombreArea],  Activo from Area where Activo  = 1";
                    string query = "select IdArea, NombreArea from Areas";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega un nuevo Lector a la lista*/
                                new EN_Area()
                                {
                                    idArea = Convert.ToInt32(dr["IdArea"]),
                                    nombreArea = dr["NombreArea"].ToString()
                                    //activo = Convert.ToBoolean(dr["Activo"])
                                }
                                );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Area>();
            }

            return lista;
        }
        public bool añadir_area()
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

        public bool modificar_area()
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

        public bool eliminar_area()
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
