using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_DetallePrestamo
    {
        public int idDetallePrestamo { get; set; }
        public EN_Prestamo prestamo { get; set; }
        public EN_Herramienta herramienta { get; set; }
        public int cantidad { get; set; }
    }
}
