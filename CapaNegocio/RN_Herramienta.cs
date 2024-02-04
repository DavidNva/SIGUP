using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;
namespace CapaNegocio
{
    public class RN_Herramienta
    {
        BD_Herramienta herramientaDatos = new BD_Herramienta();

        public List<EN_Herramienta> listarHerramientas()
        {
            return herramientaDatos.listarHerramientas();
        }

        public List<EN_Herramienta> ListarHerramientaParaPrestamo()
        {
            return herramientaDatos.ListarHerramientaParaPrestamo();
        }

        public string Registrar(EN_Herramienta obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
           
            if (obj.idHerramienta == "0")
            {
                Mensaje = "El identificador de la herramienta no puede ser 0";
            }
            else if (string.IsNullOrEmpty(obj.idHerramienta) || string.IsNullOrWhiteSpace(obj.idHerramienta))
            {
                Mensaje = "El id de la herramienta no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre de la herramienta no puede ser vacio";
            }
            else if (obj.cantidad <= 0)
            {
                Mensaje = "La cantidad debe ser mayor a 0 para poder registrar una herramienta";
            }
            else if (obj.id_marcaHerramienta.idMarca == 0)/*Si no ha seleccionado ningun lector*/
            {
                Mensaje = "Debes seleccionar una marca";
            }
            else if (obj.id_categoHerramienta.idCategoria == 0)/*Si no ha seleccionado ningun lector*/
            {
                Mensaje = "Debes seleccionar una categoria";
            }
            else if (string.IsNullOrEmpty(obj.observaciones) || string.IsNullOrWhiteSpace(obj.observaciones))
            {
                Mensaje = "El campo observaciones no puede ser vacío";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/

                return herramientaDatos.Registrar(obj, out Mensaje);
            }
            else
            {
                return "0";/*No se ha creado un Administrador*/
            }
        }
        
        public bool editarHerramienta(EN_Herramienta obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            //Validaciones para que la caja de texto no este vacio o con espacios

            if (obj.idHerramienta == "0")
            {
                Mensaje = "El identificador de la herramienta no puede ser 0";
            }
            else if (string.IsNullOrEmpty(obj.idHerramienta) || string.IsNullOrWhiteSpace(obj.idHerramienta))
            {
                Mensaje = "El id de la herramienta no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.nombre) || string.IsNullOrWhiteSpace(obj.nombre))
            {
                Mensaje = "El nombre de la herramienta no puede ser vacio";
            }
            else if (obj.cantidad <= 0)
            {
                Mensaje = "La cantidad debe ser mayor a 0 para poder editar la herramienta";
            }
            else if (obj.id_marcaHerramienta.idMarca == 0)/*Si no ha seleccionado ningun lector*/
            {
                Mensaje = "Debes seleccionar una marca";
            }
            else if (obj.id_categoHerramienta.idCategoria == 0)/*Si no ha seleccionado ningun lector*/
            {
                Mensaje = "Debes seleccionar una categoria";
            }
            else if (string.IsNullOrEmpty(obj.observaciones) || string.IsNullOrWhiteSpace(obj.observaciones))
            {
                Mensaje = "El campo observaciones no puede ser vacío";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/
                return herramientaDatos.modificar_herramienta(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }
        
        public bool Eliminar(string id, out string Mensaje)
        {
            return herramientaDatos.Eliminar(id, out Mensaje);
        } 
    }
}
