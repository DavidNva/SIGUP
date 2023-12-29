using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_Herramienta
    {
        public int idHerramienta { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public bool activo { get; set; }
        public string observaciones { get; set; }
        public string fechaRegistro { get; set; }
        public EN_MarcaHerramienta marca { get; set; }
        public EN_CategoriaHerramienta categoHerramienta { get; set; }
    }
}
