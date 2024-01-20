using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
//using QuestPDF.Fluent;//Para exportar a pdf
//using QuestPDF.Helpers;

namespace SistemaWeb_UnidadPracticas.Controllers
{
    public class SIGUPController : Controller
    {
        // GET: SIGUP
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategoriaHerramienta()
        {
            return View();
        }

        public ActionResult Herramientas()
        {
            return View();
        }

        public ActionResult Usuarios()
        {
            return View();
        }

        public ActionResult Administradores()
        {
            return View();
        }

        public ActionResult Prestamos()
        {
            return View();
        }


        /*--------------CATEGORIA---------------------*/
        #region CATEGORIA
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarCategoria() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_CategoriaHerramienta> oLista = new List<EN_CategoriaHerramienta>();
            oLista = new RN_CategoriaHerramienta().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpGet]
        public JsonResult ListarCategoriaEnHerramienta()
        {
            List<EN_CategoriaHerramienta> oLista = new List<EN_CategoriaHerramienta>();
            oLista = new RN_CategoriaHerramienta().ListarCategoriaEnHerramienta();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarCategoria(EN_CategoriaHerramienta objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
            string mensaje = string.Empty;

            if (objeto.idCategoria == 0)/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
             una categoria nueva, por lo que se ha dado dando clic con el boton de crear*/
            {
                resultado = new RN_CategoriaHerramienta().Registrar(objeto, out mensaje);/*El metodo registrar
                 de tipo int, devuelve el id registrado*/
            }
            else
            {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
                 a una categoria, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
                 con los alert comentados*/
                resultado = new RN_CategoriaHerramienta().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_CategoriaHerramienta().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        /*--------------MARCA HERRAMIENTA---------------------*/
        #region MARCA HERRAMIENTA
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarMarca() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_MarcaHerramienta> oLista = new List<EN_MarcaHerramienta>();
            oLista = new RN_MarcaHerramienta().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpGet]
        public JsonResult ListarMarcaEnHerramienta() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_MarcaHerramienta> oLista = new List<EN_MarcaHerramienta>();
            oLista = new RN_MarcaHerramienta().ListarMarcaEnHerramienta();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpPost]
        public JsonResult guardarMarca(EN_MarcaHerramienta marca)
        {
            object resultado = null;
            string mensaje = string.Empty;

            if (marca.idMarca == 0)
            {
                resultado = new RN_MarcaHerramienta().registrar(marca, out mensaje);
            }
            else
            {
                resultado = new RN_MarcaHerramienta().editarMarca(marca, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult eliminarMarca(int id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;
            respuesta =  new RN_MarcaHerramienta().eliminarMarca(id, out mensaje);
            if (respuesta)
            {
                return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { resultado = respuesta, mensaje = mensaje });
            }
        }

        #endregion

        /*--------------HERRAMIENTA---------------------*/
        #region HERRAMIENTA
        [HttpGet]
        public JsonResult listarHerramientas()
        {
            try
            {
                List<EN_Herramienta> herramientas = new List<EN_Herramienta>();
                RN_Herramienta herramienta = new RN_Herramienta();
                herramientas = herramienta.listarHerramientas();
                return Json(new { data = herramientas }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { mensaje = "Error al realizar la operacion" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GuardarHerramienta(EN_Herramienta objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
            string mensaje = string.Empty;

            if (objeto.idValidacionHerramienta == "0")/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
             una Administrador nueva, por lo que se ha dado dando clic con el boton de crear*/
            {
                resultado = new RN_Herramienta().Registrar(objeto, out mensaje);/*El metodo registrar
                 de tipo int, devuelve el id registrado*/
            }
            else
            {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
                 a una Administrador, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
                 con los alert comentados*/
                //resultado = new RN_Administrador().Editar(objeto, out mensaje);}
                //resultado = null;
                resultado = new RN_Herramienta().editarHerramienta(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarHerramienta(string id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Herramienta().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /*--------------TIPO USUARIO---------------------*/
        #region TipoUsuario
        public JsonResult ListarTipoUsuario()
        {
            List<EN_TipoUsuario> tipoUsuarios = new List<EN_TipoUsuario>();
            RN_TipoUsuario tp_negocio = new RN_TipoUsuario();
            try
            {
                tipoUsuarios = tp_negocio.ListarTiposUsuario();
                if (tipoUsuarios != null)
                {
                    return Json(new { success = true,  data = tipoUsuarios }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, data = tipoUsuarios, message = "Se ha devuelto una lista vacía, no hay datos existentes en la base, o existe un error de código en la capa datos, depura y verifica" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        /*--------------USUARIOS---------------------*/
        #region Usuarios
        public JsonResult ListarUsuarios()
        {
            RN_Usuarios rn_usuarios = new RN_Usuarios();
            List<EN_Usuario> usuarios = new List<EN_Usuario>();
            try
            {
                usuarios = rn_usuarios.ListarUsuarios();
                if (usuarios != null)
                {
                    return Json( new { success = true, data = usuarios }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, data = usuarios }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, data = usuarios }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AgregarUsuario(EN_Usuario usuario)
        {
            string resultado;
            RN_Usuarios rn_usuarios = new RN_Usuarios();
            try
            {
                resultado = rn_usuarios.AñadirUsuario(usuario);
                if (int.TryParse(resultado, out int filasAfectadas) && filasAfectadas == 1)
                {
                    return Json(new { success = true, message = "Inserción con éxito" });
                }
                else
                {
                    return Json(new { success = false, message = resultado });
                }
                
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = string.Format("Inserción de datos fallida {0}", ex.Message) });
            }
        }

        public JsonResult EditarUsuario(EN_Usuario usuario)
        {
            string resultado;
            try
            {
                RN_Usuarios rn_usuario = new RN_Usuarios();
                resultado = rn_usuario.EditarUsuario(usuario);
                if (int.TryParse(resultado, out int filasAfectadas) && filasAfectadas == 1)
                {
                    return Json(new { success = true, message = "Registro actualizado correctamente" });
                }
                else
                {
                    return Json(new { success = false, message = resultado });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = string.Format("Ha ocurrido un error en el controlador, contacta con el desarrollador si recibes este error, mensaje de error: {0}", ex.Message) });
            }
        }

        public JsonResult EliminarUsuario(int idUsuario)
        {
            string resultado;
            try
            {
                RN_Usuarios rn_usuario = new RN_Usuarios();
                resultado = rn_usuario.EliminarUsuario(idUsuario);
                if (int.TryParse(resultado, out int filasAfectadas) && filasAfectadas == 1)
                {
                    return Json(new { success = true, message = "Se ha eliminado el registro correctamente" });
                }
                else
                {
                    return Json(new { success = false, message = resultado });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = string.Format("Ha ocurrido un error en el controlador, contacta con el desarrollador si recibes este error, mensaje de error: {0}", ex.Message) });
            }
        }
        #endregion


        /*--------------PRESTAMOS--------------------*/
        #region PRESTAMOS
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarPrestamosCompleto() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Prestamo> oLista = new List<EN_Prestamo>();
            oLista = new RN_Prestamo().ListarPrestamosCompleto();/*Esta declarado en RN_Prestamos, capa negocio*/

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/

        }

        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarUsuarioParaPrestamo() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Usuario> oLista = new List<EN_Usuario>();
            oLista = new RN_Usuarios().ListarUsuarioParaPrestamo();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarHerramientaParaPrestamo() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Herramienta> oLista = new List<EN_Herramienta>();
            oLista = new RN_Herramienta().ListarHerramientaParaPrestamo();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarAreaParaPrestamo() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Area> oLista = new List<EN_Area>();
            oLista = new RN_Area().ListarAreaParaPrestamo();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        [HttpPost]
        public JsonResult GuardarPrestamo(List<EN_Prestamo> oListaPrestamo, EN_Prestamo objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            DataTable detallePrestamo = new DataTable();
            detallePrestamo.Locale = new CultureInfo("es-MX"); //Comenzamos a crear las columnas que necesita esta table
            detallePrestamo.Columns.Add("IdHerramienta");//antes era IdLibro
            detallePrestamo.Columns.Add("Cantidad", typeof(int));
            object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
            string mensaje = string.Empty;

            foreach (EN_Prestamo oEjemplar in oListaPrestamo)//por cada carrito en la lista carrito
            {
                decimal subTotal = Convert.ToDecimal(objeto.cantidad) /** oCarrito.oId_Libro.Precio*/;
                //        //Antes se multiplicaaba por el precio, ahoa simplemente pasamos la cantidad directamente

                //total += subTotal;//Va aumentando el valor de total con cada iteracion
                //Dar una condicionar que si el oEjemplar es diferente de null
                if (oListaPrestamo == null || oEjemplar == null || oEjemplar.id_Herramienta == null)
                {
                    mensaje = "No hay un ejemplar disponible para la herramienta seleccionada";
                }
                else
                {
                    detallePrestamo.Rows.Add(new object[]
                    {
                        //oCarrito.oId_Ejemplar.IdEjemplarLibro,//Estamos trabajando con ejemplar, pero en este caso solo es una lista entonces no hay problema
                        //oCarrito.oId_Libro.oId_Ejemplar.IdEjemplarLibro,//Estamos trabajando con ejemplar, pero en este caso solo es una lista entonces no hay problema
                        //oCarrito.Cantidad,
                        oEjemplar.id_Herramienta.idHerramienta,
                        oEjemplar.cantidad
                        //subTotal

                    });
                }

            }

            if (objeto.idPrestamo == 0)/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
             un Prestamo nuevo, por lo que se ha dado dando clic con el boton de crear*/
            {
                resultado = new RN_Prestamo().Registrar(objeto, detallePrestamo, out mensaje);/*El metodo registrar
                 de tipo int, devuelve el id registrado*/
            }
            else
            {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
                 a un Prestamo, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
                 con los alert comentados*/
                resultado = new RN_Prestamo().Editar(objeto, out mensaje);
                
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult FinalizarPrestamo(/*int id, int idEjemplarLibro, int idLibro */EN_Prestamo objeto)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Prestamo().FinalizarPrestamo(objeto, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarPrestamo(int id, string idHerramienta)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Prestamo().Eliminar(id, idHerramienta, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }
        #endregion

        /*--------------Administrador---------------------*/
        #region Administradores
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarAdministradores() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_Administrador> oLista = new List<EN_Administrador>();
            oLista = new RN_Administrador().ListarAdministrador();/*Esta declarado en RN_Usuarios, capa negocio*/

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/

        }

        [HttpPost]
        public JsonResult GuardarAdministrador(EN_Administrador objeto) /*De este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            object resultado;/*Va a permitir almacenar cualquier tipo de resultado (en este caso int o booelan, dependiendi si es creacion o edicion)*/
            string mensaje = string.Empty;

            if (objeto.idValidacionAdmin == "0")/*Es decir, si el id es 0 en inicio (el valor es 0 inicialmente) significa que es
             una Administrador nuevo, por lo que se ha dado dando clic con el boton de crear*/
            {
                resultado = new RN_Administrador().Registrar(objeto, out mensaje);/*El metodo registrar
                 de tipo int, devuelve el id registrado*/
            }
            else
            {/*Pero si el id es diferente de 0, es decir ya existe, entonces se esta editando
                 a una Administrador, por lo que indica que se ha dado clic en el boton de editar, eso lo comprobamos
                 con los alert comentados*/
                //resultado = new RN_Administrador().Editar(objeto, out mensaje);}
                //resultado = null;
                resultado = new RN_Administrador().Editar(objeto, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarAdministrador(string id)
        {
            bool respuesta = false;
            string mensaje = string.Empty;

            respuesta = new RN_Administrador().Eliminar(id, out mensaje);

            return Json(new { resultado = respuesta, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}