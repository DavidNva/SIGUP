using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class RN_MarcaHerramienta
    {
        BD_MarcaHerramienta marcaHerramienta = new BD_MarcaHerramienta();
        public List<EN_MarcaHerramienta> Listar()
        {
            return marcaHerramienta.Listar();
        }

        public int registrar(EN_MarcaHerramienta marca, out string mensaje)
        {
            mensaje = string.Empty;
            Console.WriteLine(marca.descripcion);

            //if (string.IsNullOrEmpty(marca.descripcion) || string.IsNullOrWhiteSpace(marca.descripcion))
            //{
            //    mensaje = "La descripción es obligatoria";
            //}

            if (string.IsNullOrEmpty(mensaje))
            {
                Console.WriteLine(marca.descripcion);
                return marcaHerramienta.añadir_marca(marca, out mensaje);
            }
            else
            {
                return 0;
            }


        }
    }
}
