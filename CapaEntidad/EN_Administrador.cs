using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class EN_Administrador
    {
        public string idValidacionAdmin { get; set; }
        public string idAdministrador { get; set; }
        public string nombres { get; set; }  
        public string apellidos { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }
        public bool reestablecer { get; set; }
        public bool activo { get; set; }
        public string fechaRegistro { get; set; }


    }
}
