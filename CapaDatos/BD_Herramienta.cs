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

                    string query = "select IdHerramienta,  CONCAT( Nombre,' / ',cantidad) AS Nombre, Activo from Herramienta";
                    //string query2 = "select IdHerramienta, Nombre, Activo from Herramienta where Activo = 1 and cantidad > 0";
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

        public List<EN_Herramienta> ListarHerramientaParaHistorialPrestamo()
        {
            List<EN_Herramienta> lista = new List<EN_Herramienta>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {

                    string query = "select IdHerramienta, Nombre, Activo from Herramienta";
                    //string query2 = "select IdHerramienta, Nombre, Activo from Herramienta where Activo = 1 and cantidad > 0";
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

        public byte[] GenerarPDF() //public ActionResult DescargarPdfCategoria<T>(List<T> oLista)
        {
            List<EN_Herramienta> oLista = new List<EN_Herramienta>();

            //oLista = new RN_Categoria().Listar();
            oLista = new BD_Herramienta().listarHerramientas();

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
                            .Text("Herramientas").FontColor("#fff");

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

                        int totalCategorias = 0;
                        col1.Item().LineHorizontal(0.5f);
                        col1.Item().Table(tabla =>
                        {//Seccion de la tabla
                            tabla.ColumnsDefinition(columns =>
                            {
                                //columns.RelativeColumn(3);
                                columns.ConstantColumn(60);
                                //columns.RelativeColumn();
                                columns.RelativeColumn();
                              

                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.ConstantColumn(40);
                                columns.RelativeColumn();
                                //columns.RelativeColumn();
                                columns.ConstantColumn(45);
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#1B396A")
                                 .Padding(2).Text("Código").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Herramienta").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                               .Padding(2).Text("Marca").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                               .Padding(2).Text("Categoria").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                              .Padding(2).Text("Stock").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                               .Padding(2).Text("Observaciones").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Activo").FontColor("#fff");
                            });

                            foreach (EN_Herramienta cat in oLista)
                            //foreach (var item in Enumerable.Range(1, 45))
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.idHerramienta.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.nombre).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.id_categoHerramienta.descripcion).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.id_marcaHerramienta.descripcion).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.cantidad.ToString()).FontSize(10);


                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.observaciones).FontSize(10);


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
                                totalCategorias++;
                            }


                        });

                        col1.Item().AlignRight().Text($"Total de categorias: {totalCategorias}").FontSize(12);

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
