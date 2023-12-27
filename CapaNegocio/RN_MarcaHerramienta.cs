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
    }
}
