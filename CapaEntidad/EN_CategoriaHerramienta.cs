using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_CategoriaHerramienta
    {
        public int idCategoria { get; set; }
        public string descripcion { get; set; }
        public bool activo { get; set; }
        public string fechaRegistro { get; set; }
    }
}
