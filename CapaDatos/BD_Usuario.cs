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
        public bool AñadirUsuario()
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(BD_Conexion.cn))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("AgregarUsuario", sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool EditarUsuario()
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

        public bool EliminarUsuario()
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
