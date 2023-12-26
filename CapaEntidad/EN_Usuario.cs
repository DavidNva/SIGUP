using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    class EN_Usuario
    {
        public int idUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public EN_TipoUsuario tipoUsuario { get; set; }
    }
}
