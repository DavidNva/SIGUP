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

        

        public int añadirHerramienta(EN_Herramienta herramienta, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(herramienta.nombre) || string.IsNullOrWhiteSpace(herramienta.nombre))
            {
                Mensaje = "El nombre no puede ser vacío";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/

                return herramientaDatos.añadir_herramienta(herramienta, out Mensaje);
            }
            else
            {
                return 0;/*No se ha creado la categoria*/
            }

        }

        public bool editarHerramienta(EN_Herramienta herramienta, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(herramienta.nombre) || string.IsNullOrWhiteSpace(herramienta.nombre))
            {
                Mensaje = "La descripción de la categoria no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/
                return herramientaDatos.modificar_herramienta(herramienta, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return herramientaDatos.eliminar_herramienta(id, out Mensaje);
        }
    }
}
