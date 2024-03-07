using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using QuestPDF.Fluent;
using System.IO;

namespace CapaDatos
{
    public class BD_Administrador
    {
        public List<EN_Administrador> ListarAdministrador()
        {
            List<EN_Administrador> lista = new List<EN_Administrador>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    string query = "select * from Administrador order by fechaRegistro desc ";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anterior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, irá agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega un nuevo Administrador a la lista*/
                                new EN_Administrador()
                                {
                                    idAdministrador = dr["IdAdministrador"].ToString(),
                                    nombres = dr["Nombres"].ToString(),
                                    apellidos = dr["Apellidos"].ToString(),
                                    telefono = dr["Telefono"].ToString(),
                                    correo = dr["Correo"].ToString(),
                                    clave = dr["Clave"].ToString(),
                                    reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                                    activo = Convert.ToBoolean(dr["Activo"])
                                }
                            );
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_Administrador>();
            }

            return lista;
        }

        public string Registrar(EN_Administrador obj, out string Mensaje)//out indica parametro de salida
        {
            string IdResultado = "0"; /*Recibe el id autogenerado*/

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
                    IdResultado = cmd.Parameters["Resultado"].Value.ToString();
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                IdResultado = "0";
                Mensaje = ex.Message;

            }
            return IdResultado;

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

        public bool CambiarClave(string idAdministrador, string nuevaClave, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update Administrador set clave = @nuevaClave, reestablecer = 0 where IdAdministrador = @Id", oConexion);
                    cmd.Parameters.AddWithValue("@Id", idAdministrador);
                    cmd.Parameters.AddWithValue("nuevaClave", nuevaClave);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    //El ExecuteNonQuery ejecuta una accion y devuelve el numero de filas afectadas
                    //Cuando eliminamos un registro de la tabla, entonces si el total de filas afectadas
                    //es mayor a 0 entonces será verdadero, pero si no es mayor a 0, entonces significa
                    //que hubo un problema al eliminar por lo que enviara un false, eso lo almacenamos en resultado
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        public bool ReestablecerClave(string idAdministrador, string clave, out string Mensaje)
        {//El administrador deberá nuevamente actualizar su contraseña
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update Administrador set clave = @clave, reestablecer = 1 where IdAdministrador = @Id", oConexion);
                    cmd.Parameters.AddWithValue("@Id", idAdministrador);
                    cmd.Parameters.AddWithValue("@clave", clave);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    //El ExecuteNonQuery ejecuta una accion y devuelve el numero de filas afectadas
                    //Cuando eliminamos un registro de la tabla, entonces si el total de filas afectadas
                    //es mayor a 0 entonces será verdadero, pero si no es mayor a 0, entonces significa
                    //que hubo un problema al eliminar por lo que enviara un false, eso lo almacenamos en resultado
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;

            }
            return resultado;
        }

        public byte[] GenerarPDF() //public ActionResult DescargarPdfAdministrador<T>(List<T> oLista)
        {
            List<EN_Administrador> oLista = new List<EN_Administrador>();

            //oLista = new RN_Administrador().Listar();
            oLista = new BD_Administrador().ListarAdministrador();

            var data = Document.Create(document =>
            {
                document.Page(page =>
                {
                    // page content
                    page.Margin(30);
                    // page.Header().Height(100).Background(Colors.Blue.Medium);
                    page.Header().ShowOnce().Row(row =>
                    {//el ShowOnce sirve para que el header solo aparezca en la primera hoja
                     //D:\ConsolePdf\ExportarPdf_Web\Content\images\cuborubikcode.png
                     //C:\Users\david\OneDrive\Documents\Proyectos Programacion\SIGUP\SIGUP\SistemaWeb_UnidadPracticas\images
                     //var rutaImagen = Path.Combine("images", "computadora.jpg");

                        var rutaImagen = Path.Combine("C:\\images\\itssnp.png");
                        //var rutaImagen = Path.Combine("C:\\Users\\david\\OneDrive\\Documents\\Proyectos Programacion\\SIGUP\\SIGUP\\SistemaWeb_UnidadPracticas\\images\\computadora.jpg");

                        byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);

                        row.ConstantItem(150).Image(imageData);

                        //row.ConstantItem(140).Height(60).Placeholder();//Elegimos el ancho del item

                        row.RelativeItem().Column(col =>//El ancho se coloca relativamente automatica
                        {
                            col.Item().AlignCenter().Text("Unidad de Prácticas").Bold().FontSize(14);
                            col.Item().AlignCenter().Text("ITSSNP - SIGUP").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("Sistema de Gestión y Control").Bold().FontSize(9);
                            col.Item().AlignCenter().Text("de Préstamos y Laboratorios").Bold().FontSize(9);
                            //col.Item().Background(Colors.Orange.Medium).Height(10);
                            //col.Item().Background(Colors.Green.Medium).Height(10);
                        });
                        row.RelativeItem().Column(col =>
                        {
                            //verde institucional 91B22C
                            //azul institucional 1B396A
                            col.Item().Border(1).BorderColor("#1B396A").
                           AlignCenter().Text("SIGUP").Bold();

                            col.Item().Background("#1B396A").Border(1)
                            .BorderColor("#1B396A").AlignCenter()
                            .Text("Administradores").FontColor("#fff");

                            col.Item().Border(1).BorderColor("#1B396A").
                            AlignCenter().Text(DateTime.Now.ToString("dd-MM-yyyy"));

                        });

                    });

                    // page.Content().Background(Colors.Yellow.Medium);
                    page.Content().PaddingVertical(10).Column(col1 =>
                    {
                        //col1.Item().Column(col2 =>//Columna de datos de usuario
                        //{
                        //    col2.Item().Text("Datos del Usuario").Underline().Bold();

                        //    col2.Item().Text(txt =>
                        //    {
                        //        txt.Span("Nombre: ").SemiBold().FontSize(10);
                        //        txt.Span("David Nava").FontSize(10);
                        //    });

                        //    col2.Item().Text(txt =>
                        //    {
                        //        txt.Span("DNI: ").SemiBold().FontSize(10);
                        //        txt.Span("0877625727").FontSize(10);
                        //    });

                        //    col2.Item().Text(txt =>
                        //    {
                        //        txt.Span("Dirección: ").SemiBold().FontSize(10);
                        //        txt.Span("Calle Luis Cabrera S/N").FontSize(10);
                        //    });
                        //});

                        int totalAdministradores = 0;
                        col1.Item().LineHorizontal(0.5f);
                        col1.Item().Table(tabla =>
                        {//Seccion de la tabla
                            tabla.ColumnsDefinition(columns =>
                            {
                                //columns.RelativeColumn(3);
                                columns.ConstantColumn(70);
                                //columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.ConstantColumn(100);
                                //columns.RelativeColumn();
                                columns.ConstantColumn(50);
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#1B396A")
                                 .Padding(2).Text("Código").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Administrador").FontColor("#fff");
                                header.Cell().Background("#1B396A")
                               .Padding(2).Text("Correo").FontColor("#fff");
                                header.Cell().Background("#1B396A")
                               .Padding(2).Text("Teléfono").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Activo").FontColor("#fff");
                            });

                            foreach (EN_Administrador cat in oLista)
                            //foreach (var item in Enumerable.Range(1, 45))
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.idAdministrador.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.nombres + " " + cat.apellidos).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.correo).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.telefono ).FontSize(10);

                                if (cat.activo)
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text("Sí").FontSize(10).FontColor("#157347");
                                }
                                else
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text("No").FontSize(10).FontColor("#BB2D3B");
                                }
                                totalAdministradores++;
                            }


                        });

                        col1.Item().AlignRight().Text($"Total de Administradores: {totalAdministradores}").FontSize(12);

                        //col1.Item().Background(Colors.Grey.Lighten3).Padding(10)//Seccion de comentarios
                        //.Column(column =>
                        //{
                        //    column.Item().Text("Comentarios").FontSize(14);
                        //    column.Item().Text(Placeholders.LoremIpsum());
                        //    column.Spacing(5);
                        //});

                        col1.Spacing(10);
                    });

                    page.Footer()
                    .AlignRight()
                    .Text(txt =>
                    {
                        txt.Span("Pagina ").FontSize(10);
                        txt.CurrentPageNumber().FontSize(10);

                        txt.Span(" de ").FontSize(10);
                        txt.TotalPages().FontSize(10);
                    });
                    //page.Footer().Height(50).Background(Colors.Red.Medium);
                });
            }).GeneratePdf();

            MemoryStream stream = new MemoryStream(data);
            //return stream.
            return stream.ToArray();
            //return File(stream, "applicacion/pdf", "detallePrestamo.pdf");
            //return View();
        }

    }
}
