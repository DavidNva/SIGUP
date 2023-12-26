using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class RN_CategoriaHerramienta
    {
        private BD_CategoriaHerramienta objCapaDato = new BD_CategoriaHerramienta(); /*Instancia una clase de la capa datos */

        public List<EN_CategoriaHerramienta> Listar() /*Usa una clase de la capa entidad*/
        {
            return objCapaDato.Listar();/*Retorna el metodo listar de la instancia de la capa Datos*/
        }

        //public List<EN_CategoriaHerramienta> ListarCategoriaEnLibro() /*Usa una clase de la capa entidad*/
        //{
        //    return objCapaDato.ListarCategoriaEnLibro();/*Retorna el metodo listar de la instancia de la capa Datos*/
        //}

        public int Registrar(EN_CategoriaHerramienta obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.descripcion) || string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "La descripción de la categoria no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/

                return objCapaDato.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;/*No se ha creado la categoria*/
            }

        }

        public bool Editar(EN_CategoriaHerramienta obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.descripcion) || string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "La descripción de la categoria no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/
                return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(string id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }

        //public byte[] GenerarPDF()
        //{
        //    return objCapaDato.GenerarPDF();
        //}
    }
}
