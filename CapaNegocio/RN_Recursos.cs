using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography; /*Para el SHA256*/
using System.Text;

using System.IO;

namespace CapaNegocio
{
    public class RN_Recursos
    {
        public static string GenerarClave()//Genera una clave aleaotorio de 6 digitos
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);//retorna un codigo unico c#
            return clave;
        }
        /*El siguiente metodo recibe un text y devuelve un text encriptado*/
        /*Encriptacion de TEXT en SHA256*/
        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            //USAR LA REFERENCIA DE "System.Security.Criptografy"
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                {
                    Sb.Append(b.ToString("x2"));
                }
            }
            return Sb.ToString();
        }
        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("david.nava.garcia4@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;//Se trabajará en base al formato html
                //Prueba de conexion

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("david.nava.garcia4@gmail.com", "qgbgompbahinyxbh"),
                    Host = "smtp.gmail.com",//servidor que usa email para enviar los correos
                    Port = 587,//puerto que utiliza email
                    EnableSsl = true //Habilita el certificado de seguridad qgbg ompb ahin yxbh
                };

                smtp.Send(mail);//Envia el correo
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }

        /*Metodo para crear una base 64*/
        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                //Un array de bytes de la ruta
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes); /*Pasamos los valores a ese textoBase64*/
            }
            catch (Exception)
            {

                conversion = false;
            }

            return textoBase64; /*Si esta variable esta vacio, es porque no tiene valor, la conversioon fallo
                                 pero si fue exitosa, traerá un valor*/
        }
    }
}
