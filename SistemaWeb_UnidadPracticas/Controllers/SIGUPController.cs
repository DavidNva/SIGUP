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



        /*--------------CATEGORIA---------------------*/
        #region CATEGORIA
        [HttpGet] /*Una URL que devuelve datos, un httpost se le pasan los valores y despues devuelve los datos  */
        public JsonResult ListarCategoria() /*D este json se puede controlar que mas ver, igualar elementos, etc*/
        {
            List<EN_CategoriaHerramienta> oLista = new List<EN_CategoriaHerramienta >();
            oLista = new RN_CategoriaHerramienta().Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
            /*El json da los datos, jala los datos de esa lista, en data*/
        }

        #endregion


    }
}