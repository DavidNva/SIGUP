using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using QuestPDF.Fluent;

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
                            lista.Add(/*Agrega una nueva Marcas a la lista*/
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

        public byte[] GenerarPDF() //public ActionResult DescargarPdfMarca<T>(List<T> oLista)
        {
            List<EN_MarcaHerramienta> oLista = new List<EN_MarcaHerramienta>();

            //oLista = new RN_Marca().Listar();
            oLista = new BD_MarcaHerramienta().Listar();

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
                            .Text("Marcas").FontColor("#fff");

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

                        int totalMarcas = 0;
                        col1.Item().LineHorizontal(0.5f);
                        col1.Item().Table(tabla =>
                        {//Seccion de la tabla
                            tabla.ColumnsDefinition(columns =>
                            {
                                //columns.RelativeColumn(3);
                                columns.ConstantColumn(100);
                                //columns.RelativeColumn();
                                columns.RelativeColumn();
                                //columns.RelativeColumn();
                                columns.ConstantColumn(100);
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#1B396A")
                                 .Padding(2).Text("Código").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Marca").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Activo").FontColor("#fff");
                            });

                            foreach (EN_MarcaHerramienta cat in oLista)
                            //foreach (var item in Enumerable.Range(1, 45))
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.idMarca.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.descripcion).FontSize(10);

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
                                totalMarcas++;
                            }


                        });

                        col1.Item().AlignRight().Text($"Total de marcas: {totalMarcas}").FontSize(12);

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
