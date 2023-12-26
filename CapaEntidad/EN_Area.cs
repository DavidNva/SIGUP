using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    class EN_Area
    {
        public int idArea { get; set; }
        public string nombreArea { get; set; }
        public EN_Edificio edificio { get; set; }
    }
}
