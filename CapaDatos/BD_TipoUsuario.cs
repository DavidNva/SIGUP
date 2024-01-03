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
    public class BD_TipoUsuario
    {
        public List<EN_TipoUsuario> ListarTiposUsuarios()
        {
            List<EN_TipoUsuario> tiposUsuarios = new List<EN_TipoUsuario>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(BD_Conexion.cn))
                {
                    string query = "SELECT * FROM tipo_usuario";
                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.Text;
                        sqlConnection.Open();
                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                EN_TipoUsuario tipoUsuario = new EN_TipoUsuario
                                {
                                    idTipo = Convert.ToInt32(sqlDataReader["IdTipo"]),
                                    nombre = sqlDataReader["nombre_tipo"].ToString()
                                };
                                tiposUsuarios.Add(tipoUsuario) ;
                            }
                        }
                    }
                }
                return tiposUsuarios;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool añadir_TipoUsuario()
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

        public bool modificar_TipoUsuario()
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

        public bool eliminar_TipoUsuario()
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
