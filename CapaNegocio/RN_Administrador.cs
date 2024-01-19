using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public  class RN_Administrador
    {
        BD_Administrador bd_administrador = new BD_Administrador();
        public List<EN_Administrador> ListarAdministrador()
        {
            return bd_administrador.ListarAdministrador();
        }

        public string Registrar(EN_Administrador obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.nombres) || string.IsNullOrWhiteSpace(obj.nombres))
            {
                Mensaje = "El nombre del administrador no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.apellidos) || string.IsNullOrWhiteSpace(obj.apellidos))
            {
                Mensaje = "El apellido del administrador no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.telefono) || string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El teléfono del administrador no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.correo) || string.IsNullOrWhiteSpace(obj.correo))
            {
                Mensaje = "El correo del administrador no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/
                string clave = "test123";
                obj.clave = RN_Recursos.ConvertirSha256(clave);

                return bd_administrador.Registrar(obj, out Mensaje);


            }
            else
            {
                return "0";/*No se ha creado un Administrador*/
            }
            //if (string.IsNullOrEmpty(Mensaje))
            //{/*Si no hay ningun mensaje, significa que no ha habido ningun error*/

            //    /*Antes de registrar, se debe enviar un correo al Administrador donde
            //     * estará la contraseña para que acceda al sistema, entonces:*/
            //    string clave = RN_Recursos.GenerarClave();//Va a encripar este valor
            //    //Aqui ira la logica de enviar un correo al Administrador
            //    string asunto = "Creacion de Cuenta"; /*En los signos de excalamcion de la linea ed abajo, se trae la variable clave*/
            //    string mensajeCorreo = "<h3>Su cuenta fue creada correctamente</h3> <br> <p>Su contraseña para acceder es: !clave!</p>";
            //    mensajeCorreo = mensajeCorreo.Replace("!clave!", clave);/*Aqui solo trae la clave creada*/

            //    bool respuesta = RN_Recursos.EnviarCorreo(obj.Correo, asunto, mensajeCorreo);

            //    if (respuesta)
            //    {
            //        obj.Clave = RN_Recursos.ConvertirSha256(clave);/*Encripta la clave generada*/
            //        return objCapaDato.Registrar(obj, out Mensaje);
            //    }
            //    else
            //    {
            //        Mensaje = "No se puede enviar el correo";
            //        return 0;
            //    }

            //}


        }

        public bool Editar(EN_Administrador obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.nombres) || string.IsNullOrWhiteSpace(obj.nombres))
            {
                Mensaje = "El nombre del administrador no puede ser vacio";
            }
            if (string.IsNullOrEmpty(obj.apellidos) || string.IsNullOrWhiteSpace(obj.apellidos))
            {
                Mensaje = "El apellido del administrador no puede ser vacio";
            }
            if (string.IsNullOrEmpty(obj.telefono) || string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El teléfono del administrador no puede ser vacio";
            }
            if (string.IsNullOrEmpty(obj.correo) || string.IsNullOrWhiteSpace(obj.correo))
            {
                Mensaje = "El correo del administrador no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/
                return bd_administrador.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(string id, out string Mensaje)
        {
            return bd_administrador.Eliminar(id, out Mensaje);
        }
    }
}
