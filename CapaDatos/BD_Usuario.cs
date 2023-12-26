using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    class BD_Usuario
    {
        public bool añadir_usuario()
        {
            try
            {

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool modificar_usuario()
        {
            try
            {

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool eliminar_usuario()
        {
            try
            {

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
