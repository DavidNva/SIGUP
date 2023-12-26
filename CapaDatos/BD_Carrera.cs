using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    class BD_Carrera
    {
        public bool añadir_carrera()
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

        public bool modificar_carrera()
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

        public bool eliminar_carrera()
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
