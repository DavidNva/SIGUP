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
        public JsonResult EliminarCategoria(string id)
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

        [HttpPost]
        public JsonResult guardarMarca(EN_MarcaHerramienta marca_entidad)
        {
            object resultado = null;
            string mensaje = string.Empty;

            if (marca_entidad.idMarca == 0)
            {
                resultado = new RN_MarcaHerramienta().registrar(marca_entidad, out mensaje);
            }
            else
            {
                Console.WriteLine("Se intentó editar");
                //resultado = new RN_CategoriaHerramienta().Editar(marca_entidad, out mensaje);
            }
            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}