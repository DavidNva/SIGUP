using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_Prestamo
    {
        public int idPrestamo { get; set; }
        public EN_Usuario id_Usuario { get; set; }
        public float cantidad { get; set; }
        public string unidad { get; set; }
        public int cantidadPorUnidad { get; set; }
        public string areaDeUso { get; set; }
        public EN_Area id_Area { get; set; }
        public bool activo { get; set; }
        public string fechaPrestamo { get; set; }
        public string fechaDevolucion { get; set; }
        public int diasPrestamo { get; set; }
        public string notas { get; set; }
        public EN_Herramienta id_Herramienta { get; set; }
        public int calificacionEntrega { get; set; }


    }
}
