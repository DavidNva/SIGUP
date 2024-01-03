using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class RN_TipoUsuario
    {
        BD_TipoUsuario tp_Datos = new BD_TipoUsuario();
        public List<EN_TipoUsuario> ListarTiposUsuario()
        {
            return tp_Datos.ListarTiposUsuarios();
        }
    }
}
