using CapaEntidad;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SistemaWeb_UnidadPracticas.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        [OutputCache(Duration = 0, NoStore = true)]//Borra la caché 
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CambiarClave()
        {
            return View();
        }
        public ActionResult Reestablecer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string correo, string clave)
        {
            EN_Administrador oAdministrador = new EN_Administrador();
            //lista el Administrador con el corre y clave dada
            oAdministrador = new RN_Administrador().ListarAdministrador().Where(u => u.correo == correo && u.clave == RN_Recursos.ConvertirSha256(clave)).FirstOrDefault();

            if (oAdministrador == null)/*Si no encontró el Administrador*/
            {
                ViewBag.Error = "Correo o contraseña incorrecta";//Variable temporal
                /*El view bag guarda informacion a compartir en la misma vista la que estamos utilizando*/
                return View();//retorna la misma vista donde se mostrará el mensaje de error
            }
            else /*Pero si lo encontró*/
            {
                if (oAdministrador.reestablecer) /*Si reestablecer es verdadera, significa que esta entrando por primera vez, entonces*/
                { /*Por lo que debe cambiar su contraseña a una personalizada*/
                    TempData["IdAdministrador"] = oAdministrador.idAdministrador;
                    /*TempData lo usamos para guardar informacion y compartirlo con otras vistas, multiples vistas dentro de un mismo controlador*/
                    return RedirectToAction("CambiarClave"); /*Como estamos en la misma carpeta, no necesitamos especificar una nueva carpeta*/
                }

                //FormsAuthentication.SetAuthCookie(oAdministrador.correo, false);/*Se crea una autentificacion del Administrador por su correo*/
                //Session["Administrador"] = oAdministrador;
                ViewBag.Error = null;
                return RedirectToAction("Index", "SIGUP"); /*Referencia al dashboard principal*/
            }
        }


        [HttpPost]
        public ActionResult CambiarClave(string idAdministrador, string claveActual, string nuevaClave, string confirmarClave)
        {
            EN_Administrador oAdministrador = new EN_Administrador();

            oAdministrador = new RN_Administrador().ListarAdministrador().Where(u => u.idAdministrador == idAdministrador).FirstOrDefault();

            if (oAdministrador.clave != RN_Recursos.ConvertirSha256(claveActual)) /*Si la clave que tiene el Administrador no es igual a la que esta poniendo*/
            {
                TempData["IdAdministrador"] = idAdministrador;/*Para mantener esta informacion temporal*/
                ViewData["vclave"] = "";/*View data permite almacenar valores mas simples, como cadenas de texto*/
                /*Como la clave actual no es correcta, la colocamos en vacio*/
                ViewBag.Error = "La contraseña actual no es correcta (Verifique la clave que se le envió al correo registrado)";
                return View();
            }
            else if (nuevaClave != confirmarClave)/*En el caso que si sea correcta, pero al confirmar nueva clave no coincida*/
            {
                TempData["IdAdministrador"] = idAdministrador;/*Para mantener esta informacion temporal*/
                ViewData["vclave"] = claveActual; /*De esta forma si nos equivovamos en la validacion de la nueva contraseña, ya no se va
                                                   a eliminar o borrar al validar de nuevo*/
                ViewBag.Error = "Las contraseñas no coinciden";
                return View();
            }

            ViewData["vclave"] = "";//Saliendo de lo anterior, esta viewdata no tendrá nada
            nuevaClave = RN_Recursos.ConvertirSha256(nuevaClave); /*Encripta la nueva clave si todo va correcto*/
            string mensaje = string.Empty;

            bool respuesta = new RN_Administrador().CambiarClave(idAdministrador, nuevaClave, out mensaje); /*Cambia la clave*/
             
            if (respuesta) /*Si el cambio ha sido correcta*/
            {
                TempData["respuesta"] = true;//Envia true para indicar que se debe mostrar un mensaje al
                                             //usuario de que la contraseña ha sido cambiada correctamente a la que el personalizó
                return RedirectToAction("Index"); /*Redirecciona al login*/
            }
            else /*En caso de que haya un error*/
            {
                TempData["respuesta"] = false;
                TempData["IdAdministrador"] = idAdministrador; /*No debemos perder este idAdministrador*/
                ViewBag.Error = mensaje;
                return View();
            }
        }

    }
}