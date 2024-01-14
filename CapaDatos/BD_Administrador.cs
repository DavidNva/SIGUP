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
            List<EN_Administrador> Administrador = new List<EN_Administrador>();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(BD_Conexion.cn))
                {
                    string query = "select * from Administrador";
                    using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                    {
                        sqlCommand.CommandType = CommandType.Text;
                        sqlConnection.Open();
                        using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                        {
                            while (sqlDataReader.Read())
                            {
                                EN_Administrador usuario = new EN_Administrador
                                {
                                    idAdministrador = sqlDataReader["IdAdministrador"].ToString(),
                                    //Convert.ToInt32(sqlDataReader["IdAdministrador"]),
                                    nombres = sqlDataReader["Nombres"].ToString(),
                                    apellidos = sqlDataReader["Apellidos"].ToString(),
                                    telefono = sqlDataReader["Telefono"].ToString(),
                                    correo = sqlDataReader["Correo"].ToString(),
                                    activo = Convert.ToBoolean( sqlDataReader["Activo"].ToString())
                                };
                                Administrador.Add(usuario);
                            }
                        }
                    }
                }
                return Administrador;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string Registrar(EN_Administrador obj, out string Mensaje)//out indica parametro de salida
        {
            string IdAutogenerado = "0"; /*Recibe el id autogenerado*/

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

    }
}
