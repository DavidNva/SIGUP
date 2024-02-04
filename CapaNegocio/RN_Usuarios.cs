using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class RN_Usuarios
    {
        BD_Usuario bd_usuario = new BD_Usuario();
        public List<EN_Usuario> ListarUsuarios()
        {
            return bd_usuario.ListarUsuarios();
        }

        public string AñadirUsuario(EN_Usuario usuario)
        {
            return bd_usuario.AñadirUsuario(usuario);
        }

        public string EditarUsuario(EN_Usuario usuario)
        {
            return bd_usuario.EditarUsuario(usuario);
        }

        public string EliminarUsuario(int idUsuario)
        {
            return bd_usuario.EliminarUsuario(idUsuario);
        }

        public List<EN_Usuario> ListarUsuarioParaPrestamo()
        {
            return bd_usuario.ListarUsuarioParaPrestamo();
        }

    }
}
