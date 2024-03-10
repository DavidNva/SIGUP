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
using QuestPDF.Fluent;

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
                    sb.AppendLine("p.Cantidad as CantidadP, p.Unidad, p.CantidadPU,p.AreaDeUso, p.Area as Id_Area,a.NombreArea, p.CalificacionEntrega,");
                    sb.AppendLine("CONVERT(char(10), p.FechaPrestamo, 103)[FechaPrestamo],");
                    sb.AppendLine("CONVERT(char(10), p.FechaDevolucion, 103)[FechaDevolucion],");
                    sb.AppendLine("p.DiasDePrestamo, p.Notas, p.Activo,dp.IdHerramienta, h.IdHerramienta, h.Nombre AS NombreHerramienta,");
                    sb.AppendLine("dp.Cantidad as CantidadDP, dp.IdDetallePrestamo");
                    sb.AppendLine("from detalle_prestamo dp");
                    sb.AppendLine("INNER JOIN herramienta h ON h.IdHerramienta = dp.IdHerramienta");
                    sb.AppendLine("INNER JOIN Prestamo p ON p.IdPrestamo = dp.IdPrestamo");
                    sb.AppendLine("INNER JOIN Areas a on a.IdArea = p.Area");
                    sb.AppendLine("inner join usuario u on u.IdUsuario = p.IdUsuario order by p.IdPrestamo DESC");

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
                                    unidad = dr["Unidad"].ToString(),
                                    cantidadPorUnidad = Convert.ToInt32(dr["CantidadPU"]),
                                    areaDeUso = dr["AreaDeUso"].ToString(),
                                    fechaPrestamo = dr["FechaPrestamo"].ToString(),
                                    fechaDevolucion = dr["FechaDevolucion"].ToString(),
                                    diasPrestamo = Convert.ToInt32(dr["DiasDePrestamo"]),
                                    notas = dr["Notas"].ToString(),
                                    //Tipo = Convert.ToInt32(dr["Tipo"]),   
                                    activo = Convert.ToBoolean(dr["Activo"]),
                                    calificacionEntrega = Convert.ToInt32(dr["CalificacionEntrega"]),
                                    id_Herramienta = new EN_Herramienta() { idHerramienta = dr["IdHerramienta"].ToString(), nombre = dr["NombreHerramienta"].ToString() },
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


        public bool Editar(EN_Prestamo obj, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;
            //DateTime FechaPrestamo = Convert.ToDateTime(obj.fechaPrestamo);
            string fechaPrestamo = obj.fechaPrestamo;
            DateTime FechaPrestamo = DateTime.ParseExact(fechaPrestamo, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            //DateTime fechaPrestamo = Convert.ToDateTime(obj.fechaPrestamo);

            //DateTime fechaDevolucion = Convert.ToDateTime(obj.fechaDevolucion);


            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarPrestamo", oConexion);
                    cmd.Parameters.AddWithValue("IdPrestamo", obj.idPrestamo);
                    cmd.Parameters.AddWithValue("Id_Usuario", obj.id_Usuario.idUsuario);
                    cmd.Parameters.AddWithValue("CantidadTotal", obj.cantidad);
                    cmd.Parameters.AddWithValue("Unidad", obj.unidad);
                    cmd.Parameters.AddWithValue("CantidadPU", obj.cantidadPorUnidad);
                    cmd.Parameters.AddWithValue("AreaDeUso", obj.areaDeUso);
                    cmd.Parameters.AddWithValue("Id_Area", obj.id_Area.idArea);
                    //cmd.Parameters.AddWithValue("Activo", obj.Activo); 
                    //cmd.Parameters.AddWithValue("FechaPrestamo", obj.FechaPrestamo);
                    //cmd.Parameters.AddWithValue("FechaDevolucion", obj.FechaDevolucion);

                    cmd.Parameters.AddWithValue("FechaPrestamo", FechaPrestamo);
                    //cmd.Parameters.AddWithValue("FechaPrestamo", obj.fechaPrestamo);
                    //cmd.Parameters.AddWithValue("FechaDevolucion", fechaDevolucion.ToString("MM-dd-yyyy"));

                    cmd.Parameters.AddWithValue("DiasDePrestamo", obj.diasPrestamo);
                    cmd.Parameters.AddWithValue("Observaciones", obj.notas);

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

        //sp_FinalizarPrestamo
        public bool FinalizarPrestamo(EN_Prestamo obj, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            //DateTime fechaPrestamo = Convert.ToDateTime(obj.fechaPrestamo);

            string fechaDevolucion = obj.fechaDevolucion;
            DateTime FechaDevolucion = DateTime.ParseExact(fechaDevolucion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //DateTime fechaDevolucion = Convert.ToDateTime(obj.fechaDevolucion);


            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_FinalizarPrestamo", oConexion);
                    cmd.Parameters.AddWithValue("IdPrestamo", obj.idPrestamo);
                    cmd.Parameters.AddWithValue("FechaDevolucion", FechaDevolucion);
                    cmd.Parameters.AddWithValue("Observaciones", obj.notas);
                   
                    cmd.Parameters.AddWithValue("IdHerramienta", obj.id_Herramienta.idHerramienta);

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
       
        public bool Eliminar(int id, string idHerramienta, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarPrestamo", oConexion);
                    cmd.Parameters.AddWithValue("IdPrestamo", id);
                    cmd.Parameters.AddWithValue("IdHerramienta", idHerramienta);
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

        public byte[] GenerarPDF() //public ActionResult DescargarPdfPrestamo<T>(List<T> oLista)
        {
            List<EN_Prestamo> oLista = new List<EN_Prestamo>();

            //oLista = new RN_Prestamo().Listar();
            oLista = new BD_Prestamo().ListarPrestamosCompleto();

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
                                columns.ConstantColumn(60);
                                //columns.RelativeColumn(3);
                                //columns.ConstantColumn(100);
                                //columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.ConstantColumn(35);
                                columns.ConstantColumn(75);
                                columns.RelativeColumn();
                                //columns.RelativeColumn();

                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#1B396A")
                               .Padding(2).Text("Préstamo").FontColor("#fff");
                                //header.Cell().Background("#1B396A")
                                // .Padding(2).Text("Código").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Herramienta").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Usuario").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Fecha Préstamo").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Fecha de Entrega").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Días Solic.").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                                .Padding(2).Text("Detalles").FontColor("#fff");

                                header.Cell().Background("#1B396A")
                               .Padding(2).Text("Observac.").FontColor("#fff");


                            });

                            foreach (EN_Prestamo cat in oLista)
                            //foreach (var item in Enumerable.Range(1, 45))
                            {
                                if (cat.activo)
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text("Pendiente").FontSize(10).FontColor("#BB2D3B"); //Rojo
                                }
                                else
                                {
                                    tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                    .Padding(2).Text("Devuelto").FontSize(10).FontColor("#157347");//Verde
                                }
                                //tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                //.Padding(2).Text(cat.idPrestamo.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                .Padding(2).Text(cat.id_Herramienta.nombre).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(cat.id_Usuario.NombreCompletoUsuario).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(cat.fechaPrestamo).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(cat.fechaDevolucion).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                               .Padding(2).Text(cat.diasPrestamo.ToString()).FontSize(10);

                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                              .Padding(2).Text(cat.unidad + " / CP: " + cat.cantidadPorUnidad).FontSize(10);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                              .Padding(2).Text(cat.notas).FontSize(10);


                                totalPrestamos++;
                            }


                        });

                        col1.Item().AlignRight().Text($"Total de Préstamos: {totalPrestamos}").FontSize(12);

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
