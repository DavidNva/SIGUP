using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;/*Se agrego una nueva referencia desde la capa datos*/

namespace CapaDatos
{
    public class BD_Conexion
    {
        /*El valor se guarda en cn, desde la variable cadena indicada en web.config*/
        public static string cn = ConfigurationManager.ConnectionStrings["cadena"].ToString();
    }
}
