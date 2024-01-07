using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public  class RN_Area
    {
        private BD_Area objCapaDato = new BD_Area(); /*Instancia una clase de la capa datos */
        public List<EN_Area> ListarAreaParaPrestamo()
        {
            return objCapaDato.ListarAreaParaPrestamo();
        }
    }
}
