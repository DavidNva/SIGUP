using System;
using System.Collections.Generic;
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
                return Json(new { resultado = respuesta, mensaje = "Se intentó realizar la petición en la capa datos y falló" });
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
        public JsonResult añadirHerramienta(EN_Herramienta herramienta)
        {
            object resultado = null;
            string mensaje = string.Empty;

            if (herramienta.idHerramienta == 0)
            {
                resultado = new RN_Herramienta().añadirHerramienta(herramienta, out mensaje);
            }
            else
            {
                resultado = new RN_Herramienta().editarHerramienta(herramienta, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
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
        #region TipoUsuario
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
                return Json(new { success = false, message = "Inserción con éxito" });
            }
        }
        #endregion
    }
}