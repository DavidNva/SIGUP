using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data; /*Acceso a sql conections*/
//using QuestPDF.Fluent;//Para exportar a pdf
//using QuestPDF.Helpers;
using System.IO;

namespace CapaDatos
{
    public class BD_CategoriaHerramienta
    {
        public List<EN_CategoriaHerramienta> Listar()
        {
            List<EN_CategoriaHerramienta> lista = new List<EN_CategoriaHerramienta>();
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    string query = "SELECT IdCategoria, Descripcion, Activo FROM Categoria_Herramienta";
                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
                    {
                        while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
                        {
                            lista.Add(/*Agrega una nueva categorias a la lista*/
                                new EN_CategoriaHerramienta()
                                {
                                    idCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                    descripcion = dr["Descripcion"].ToString(),
                                    activo = Convert.ToBoolean(dr["Activo"])
                                });
                        }
                    }
                }
            }
            catch (Exception)
            {
                lista = new List<EN_CategoriaHerramienta>();
            }

            return lista;
        }

        //public List<EN_CategoriaHerramienta> ListarCategoriaEnLibro()
        //{
        //    List<EN_CategoriaHerramienta> lista = new List<EN_CategoriaHerramienta>();
        //    try
        //    {
        //        using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
        //        {
        //            string query = "SELECT IdCategoria, Descripcion, Activo FROM CATEGORIA where Activo = 1";
        //            SqlCommand cmd = new SqlCommand(query, oConexion);
        //            cmd.CommandType = CommandType.Text;/*En este caso es de tipo Text (no usamos para este ejemplo, procedimientos almacenados*/

        //            oConexion.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())/*Lee todos los resultados que aparecen en la ejecucion del select anter ior*/
        //            {
        //                while (dr.Read())/*Mientras reader esta leyendo, ira agregando a la lista dicha lectura*/
        //                {
        //                    lista.Add(/*Agrega una nueva categorias a la lista*/
        //                        new EN_CategoriaHerramienta()
        //                        {
        //                            idCategoria = Convert.ToInt32(dr["IdCategoria"]),
        //                            descripcion = dr["Descripcion"].ToString(),
        //                            activo = Convert.ToBoolean(dr["Activo"])
        //                        });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        lista = new List<EN_CategoriaHerramienta>();
        //    }

        //    return lista;
        //}

        public int Registrar(EN_CategoriaHerramienta obj, out string Mensaje)//out indica parametro de salida
        {
            int IdAutogenerado = 0; /*Recibe el id autogenerado*/

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarCategoria", oConexion);
                    /*Los primeros valores de estos parametros, es la del procedimiento del sql y el segundo de donde toma el valor (las propieaddes de la clase EN_CategoriaHerramienta*/
                    cmd.Parameters.AddWithValue("Descripcion", obj.descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.activo);
                    //Dos parametros de salida, un entero de resultaado y un string de mensaje
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                    IdAutogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                IdAutogenerado = 0;/*Regresa a 0*/
                Mensaje = ex.Message;

            }
            return IdAutogenerado; /*Si cambia a un nuevo valor al agregar un nuevo id*/

        }

        public bool Editar(EN_CategoriaHerramienta obj, out string Mensaje)//out indica parametro de salida
        {
            bool resultado = false;

            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oConexion = new SqlConnection(BD_Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarCategoria", oConexion);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.idCategoria);
                    cmd.Parameters.AddWithValue("Descripcion", obj.descripcion);
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
                    SqlCommand cmd = new SqlCommand("sp_EliminarCategoria", oConexion);
                    cmd.Parameters.AddWithValue("IdCategoria", id);
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

        //public byte[] GenerarPDF() //public ActionResult DescargarPdfCategoria<T>(List<T> oLista)
        //{
        //    List<EN_CategoriaHerramienta> oLista = new List<EN_CategoriaHerramienta>();

        //    //oLista = new RN_Categoria().Listar();
        //    oLista = new BD_CategoriaHerramienta().Listar();

        //    var data = Document.Create(document =>
        //    {
        //        document.Page(page =>
        //        {
        //            // page content
        //            page.Margin(30);
        //            // page.Header().Height(100).Background(Colors.Blue.Medium);
        //            page.Header().ShowOnce().Row(row =>
        //            {//el ShowOnce sirve para que el header solo aparezca en la primera hoja
        //             //D:\ConsolePdf\ExportarPdf_Web\Content\images\cuborubikcode.png
        //                var rutaImagen = Path.Combine("D:\\ConsolePdf\\ExportarPdf_Web\\Content\\images\\cuborubikcode.png");

        //                byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);

        //                row.ConstantItem(150).Image(imageData);

        //                //row.ConstantItem(140).Height(60).Placeholder();//Elegimos el ancho del item

        //                row.RelativeItem().Column(col =>//El ancho se coloca relativamente automatica
        //                {
        //                    col.Item().AlignCenter().Text("Biblioteca: Luis Cabrera").Bold().FontSize(14);
        //                    col.Item().AlignCenter().Text("Puebla, Puebla").Bold().FontSize(9);
        //                    col.Item().AlignCenter().Text("123 456 7890").Bold().FontSize(9);
        //                    col.Item().AlignCenter().Text("example@gmail.com").Bold().FontSize(9);
        //                    //col.Item().Background(Colors.Orange.Medium).Height(10);
        //                    //col.Item().Background(Colors.Green.Medium).Height(10);
        //                });
        //                row.RelativeItem().Column(col =>
        //                {
        //                    col.Item().Border(1).BorderColor("#257272").
        //                   AlignCenter().Text("Biblioteca");

        //                    col.Item().Background("#257272").Border(1)
        //                    .BorderColor("#257272").AlignCenter()
        //                    .Text("Categorias").FontColor("#fff");

        //                    col.Item().Border(1).BorderColor("#257272").
        //                    AlignCenter().Text(DateTime.Now.ToString("dd-MM-yyyy"));

        //                });

        //            });

        //            // page.Content().Background(Colors.Yellow.Medium);
        //            page.Content().PaddingVertical(10).Column(col1 =>
        //            {
        //                col1.Item().Column(col2 =>//Columna de datos de usuario
        //                {
        //                    col2.Item().Text("Datos del Usuario").Underline().Bold();

        //                    col2.Item().Text(txt =>
        //                    {
        //                        txt.Span("Nombre: ").SemiBold().FontSize(10);
        //                        txt.Span("David Nava").FontSize(10);
        //                    });

        //                    col2.Item().Text(txt =>
        //                    {
        //                        txt.Span("DNI: ").SemiBold().FontSize(10);
        //                        txt.Span("0877625727").FontSize(10);
        //                    });

        //                    col2.Item().Text(txt =>
        //                    {
        //                        txt.Span("Dirección: ").SemiBold().FontSize(10);
        //                        txt.Span("Calle Luis Cabrera S/N").FontSize(10);
        //                    });
        //                });

        //                int totalCategorias = 0;
        //                col1.Item().LineHorizontal(0.5f);
        //                col1.Item().Table(tabla =>
        //                {//Seccion de la tabla
        //                    tabla.ColumnsDefinition(columns =>
        //                    {
        //                        //columns.RelativeColumn(3);
        //                        columns.ConstantColumn(100);
        //                        //columns.RelativeColumn();
        //                        columns.RelativeColumn();
        //                        //columns.RelativeColumn();
        //                        columns.ConstantColumn(100);
        //                    });

        //                    tabla.Header(header =>
        //                    {
        //                        header.Cell().Background("#257272")
        //                         .Padding(2).Text("Código").FontColor("#fff");

        //                        header.Cell().Background("#257272")
        //                        .Padding(2).Text("Descripción").FontColor("#fff");

        //                        header.Cell().Background("#257272")
        //                        .Padding(2).Text("Activo").FontColor("#fff");
        //                    });

        //                    foreach (EN_CategoriaHerramienta cat in oLista)
        //                    //foreach (var item in Enumerable.Range(1, 45))
        //                    {

        //                        tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                        .Padding(2).Text(cat.IdCategoria.ToString()).FontSize(10);

        //                        tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                        .Padding(2).Text(cat.Descripcion).FontSize(10);

        //                        if (cat.Activo)
        //                        {
        //                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                            .Padding(2).Text("Sí").FontSize(10);
        //                        }
        //                        else
        //                        {
        //                            tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
        //                            .Padding(2).Text("No").FontSize(10);
        //                        }
        //                        totalCategorias++;
        //                    }


        //                });

        //                col1.Item().AlignRight().Text($"Total de categorias: {totalCategorias}").FontSize(12);

        //                //col1.Item().Background(Colors.Grey.Lighten3).Padding(10)//Seccion de comentarios
        //                //.Column(column =>
        //                //{
        //                //    column.Item().Text("Comentarios").FontSize(14);
        //                //    column.Item().Text(Placeholders.LoremIpsum());
        //                //    column.Spacing(5);
        //                //});

        //                col1.Spacing(10);
        //            });

        //            page.Footer()
        //            .AlignRight()
        //            .Text(txt =>
        //            {
        //                txt.Span("Pagina ").FontSize(10);
        //                txt.CurrentPageNumber().FontSize(10);

        //                txt.Span(" de ").FontSize(10);
        //                txt.TotalPages().FontSize(10);
        //            });
        //            //page.Footer().Height(50).Background(Colors.Red.Medium);
        //        });
        //    }).GeneratePdf();

        //    MemoryStream stream = new MemoryStream(data);
        //    //return stream.
        //    return stream.ToArray();
        //    //return File(stream, "applicacion/pdf", "detallePrestamo.pdf");
        //    //return View();
        //}


    
    }
}
