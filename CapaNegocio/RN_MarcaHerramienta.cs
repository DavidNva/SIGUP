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

        public List<EN_MarcaHerramienta> ListarMarcaEnHerramienta()
        {
            return marcaHerramienta.ListarMarcaEnHerramienta();
        }

        public int registrar(EN_MarcaHerramienta marca, out string mensaje)
        {
            mensaje = string.Empty;
            Console.WriteLine(marca.descripcion);

            if (string.IsNullOrEmpty(marca.descripcion) || string.IsNullOrWhiteSpace(marca.descripcion))
            {
                mensaje = "La descripción es obligatoria";
            }

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

        public bool editarMarca(EN_MarcaHerramienta marca, out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(marca.descripcion) || string.IsNullOrWhiteSpace(marca.descripcion))
            {
                mensaje = "La descripción es obligatoria";
            }

            if (string.IsNullOrEmpty(mensaje))
            {
                return marcaHerramienta.modificar_marca(marca, out mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool eliminarMarca(int id, out string mensaje)
        {
            mensaje = string.Empty;
            return marcaHerramienta.eliminar_marca(id, out mensaje);
        } 
    }
}
