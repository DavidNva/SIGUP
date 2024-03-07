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
            if (obj.idAdministrador == "0")
            {
                Mensaje = "El identificador del administrador no puede ser 0";
            }
            else if (string.IsNullOrEmpty(obj.idAdministrador) || string.IsNullOrWhiteSpace(obj.idAdministrador))
            {
                Mensaje = "El id del administrador no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.nombres) || string.IsNullOrWhiteSpace(obj.nombres))
            {
                Mensaje = "El nombre del administrador no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.apellidos) || string.IsNullOrWhiteSpace(obj.apellidos))
            {
                Mensaje = "El apellido del administrador no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.correo) || string.IsNullOrWhiteSpace(obj.correo))
            {
                Mensaje = "El correo del administrador no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.telefono) || string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El teléfono del administrador no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/
                
                string clave = RN_Recursos.GenerarClave();

                string asunto = "Creacion de Cuenta"; /*En los signos de excalamcion de la linea ed abajo, se trae la variable clave*/
                string mensajeCorreo = "<h3>Su cuenta fue creada correctamente</h3> <br> <p>Su contraseña para acceder es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", clave);/*Aqui solo trae la clave creada*/

                bool respuesta = RN_Recursos.EnviarCorreo(obj.correo, asunto, mensajeCorreo);

                if (respuesta)
                {
                    obj.clave = RN_Recursos.ConvertirSha256(clave);/*Encripta la clave generada*/
                    return bd_administrador.Registrar(obj, out Mensaje);
                }
                else
                {
                    Mensaje = "No se puede enviar el correo. Verifique su email o compruebe su conexión a internet";
                    return "0";
                }
            }
            else
            {
                return "0";/*No se ha creado un Administrador*/
            }
        }

        public bool Editar(EN_Administrador obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            //Validaciones para que la caja de texto no este vacio o con espacios
            if (obj.idAdministrador == "0")
            {
                Mensaje = "El identificador del administrador no puede ser 0";
            }
            else if (string.IsNullOrEmpty(obj.idAdministrador) || string.IsNullOrWhiteSpace(obj.idAdministrador))
            {
                Mensaje = "El id del administrador no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.nombres) || string.IsNullOrWhiteSpace(obj.nombres))
            {
                Mensaje = "El nombre del administrador no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.apellidos) || string.IsNullOrWhiteSpace(obj.apellidos))
            {
                Mensaje = "El apellido del administrador no puede ser vacio";
            }
           
            else if (string.IsNullOrEmpty(obj.correo) || string.IsNullOrWhiteSpace(obj.correo))
            {
                Mensaje = "El correo del administrador no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.telefono) || string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El teléfono del administrador no puede ser vacio";
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

        public bool CambiarClave(string idAdministrador, string nuevaClave, out string Mensaje)
        {
            return bd_administrador.CambiarClave(idAdministrador, nuevaClave, out Mensaje);
        }

        public bool ReestablecerClave(string idAdministrador, string correo, out string Mensaje)
        {
            Mensaje = string.Empty;
            string nuevaClave = RN_Recursos.GenerarClave();//Va a encripar este valor

            bool resultado = bd_administrador.ReestablecerClave(idAdministrador, RN_Recursos.ConvertirSha256(nuevaClave), out Mensaje);

            if (resultado)//si resultado es verdadero
            {
                string asunto = "Contraseña reestablecida"; /*En los signos de excalamcion de la linea ed abajo, se trae la variable clave*/
                string mensajeCorreo = "<h3>Su cuenta fue reestablecida correctamente</h3> <br> <p>Su nueva contraseña para acceder ahora es: !clave!</p>";
                mensajeCorreo = mensajeCorreo.Replace("!clave!", nuevaClave);/*Aqui solo trae la clave creada*/
                bool respuesta = RN_Recursos.EnviarCorreo(correo, asunto, mensajeCorreo);
                if (respuesta) {
                    return true;
                }
                else
                {
                    Mensaje = "No se pudo enviar el correo. Revise su conexión a internet o verifique su correo este escrito correctamente";
                    return false;
                }
            }
            else
            {
                Mensaje = "No se pudo reestablecer la contraseña";
                return false;
            }
        }


        public byte[] GenerarPDF()
        {
            return bd_administrador.GenerarPDF();
        }
    }
}
