using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using System.IO;

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

        //Historial de prestamos
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
                                    DiasSolicitados = dr["DiasSolicitados"].ToString(),
                                    Usuario = dr["Usuario"].ToString(),
                                    IdUsuario = dr["IdUsuario"].ToString(),
                                    Herramienta = dr["Herramienta"].ToString(),
                                    //Precio = Convert.ToDecimal(dr["Precio"], new CultureInfo("es-MX")),
                                    //Cantidad = Convert.ToInt32(dr["Stock"]),//Checar este .tostring();
                                    Detalles = dr["Detalles"].ToString(),
                                    FechaDevolucion = dr["FechaDevolucion"].ToString(),
                                    Estado = Convert.ToBoolean(dr["Activo"]),//Devuelto = 1 o no devuelto = 0
                                    //Total = Convert.ToDecimal(dr["Total"], new CultureInfo("es-MX")),
                                    Observaciones = dr["Observaciones"].ToString()
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



        public byte[] GenerarPDF(string fechaInicio, string fechaFin, string codigoUsuario, string estado, string herramienta) //public ActionResult DescargarPdfCategoria<T>(List<T> oLista)
        {
            List<EN_Reporte> oLista = new List<EN_Reporte>();

            //oLista = new RN_Categoria().Listar();
            oLista = new BD_Reporte().Prestamos(fechaInicio, fechaFin, codigoUsuario, estado, herramienta);

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
                            .Text("Préstamos").FontColor("#fff");

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

                        int totalPrestamos = 0;
                        col1.Item().LineHorizontal(0.5f);
                        col1.Item().Table(tabla =>
                        {//Seccion de la tabla
                            tabla.ColumnsDefinition(columns =>
                            {
                                //columns.RelativeColumn(3);
                                columns.ConstantColumn(100);
                                //columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                //columns.RelativeColumn();
                                columns.ConstantColumn(100);
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#1B396A")
                                 .Padding(2).Text("Fecha").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Usuario").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Herramientas").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Estado").FontColor("#fff");
                            });

                            foreach (EN_Reporte historialPrestamos in oLista)
                            //foreach (var item in Enumerable.Range(1, 45))
                            {

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(historialPrestamos.FechaPrestamo.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(historialPrestamos.Usuario).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(historialPrestamos.Herramienta).FontSize(10);

                                if (historialPrestamos.Estado)
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text("Pendientes").FontSize(10).FontColor("#BB2D3B").Bold(); ;
                                }//.Background("#1B396A").Border(1) .BorderColor("#1B396A").
                                else
                                {//91B22C verde institucional /// verde-success-boostrap 157347
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text("Devueltos").FontSize(10).FontColor("#157347").Bold(); ;
                                }
                                totalPrestamos++;
                            }


                        });

                        col1.Item().AlignRight().Text($"Total de préstamos: {totalPrestamos}").FontSize(12);

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
