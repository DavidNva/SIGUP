﻿using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class RN_Prestamo
    {
        private BD_Prestamo objCapaDato = new BD_Prestamo();

        public List<EN_Prestamo> ListarPrestamosCompleto()
        {
            return objCapaDato.ListarPrestamosCompleto();
        }

        public bool Registrar(EN_Prestamo obj, DataTable DetallePrestamo,/* DataTable EjemplarActivo,*/ out string Mensaje)
        {
            Mensaje = string.Empty;
            //int cantidadInicial;
            //float cantidadIngresada;

            //cantidadInicial = obj.id_Herramienta.cantidad;
            //cantidadIngresada = obj.cantidad;

            //Validaciones para que la caja de texto no este vacio o con espacios
            if (string.IsNullOrEmpty(obj.fechaPrestamo) || string.IsNullOrWhiteSpace(obj.fechaPrestamo))
            {
                Mensaje = "La fecha del préstamo no puede ser vacio";
            }
            //if (string.IsNullOrEmpty(obj.FechaDevolucion) || string.IsNullOrWhiteSpace(obj.FechaDevolucion))
            //{
            //    Mensaje = "La fecha de devolucion del préstamo no puede ser vacio";
            //}
            else if (obj.diasPrestamo == 0)
            {
                Mensaje = "Debe ingresar los dias de préstamo del libro -> El valor debe ser mayor a 0";
            }

            else if (obj.id_Herramienta.idHerramienta == 0)/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debes seleccionar una herramienta";
            }
            else if (obj.id_Area.idArea == 0)/*Si no ha seleccionado ninguna marca*/
            {
                Mensaje = "Debes seleccionar una area";
            }
            //else if (obj.oId_Ejemplar.IdEjemplarLibro == 0)/*Si no ha seleccionado ninguna marca*/
            //{
            //    Mensaje = "Debes seleccionar un ejemplar disponible para el libro seleccionado. Verifica que el libro cuente con al menos un ejemplar disponible.";
            //}
            else if (obj.id_Usuario.idUsuario == 0)/*Si no ha seleccionado ningun lector*/
            {
                Mensaje = "Debes seleccionar un usuario";
            }
            else if (obj.cantidad == 0)
            {
                Mensaje = "Debes ingresar la cantidad de libros a prestar -> El valor debe ser mayor a 0";
            }


            //else if (cantidadInicial - cantidadIngresada <0)
            //{
            //    Mensaje = "No hay suficiente stock para realizar el prestamo con esa cantidad, prueba con un número menor";
            //}

            else if (string.IsNullOrEmpty(obj.notas) || string.IsNullOrWhiteSpace(obj.notas))
            {
                Mensaje = "El campo observaciones no puede ser vacio";

            }


            if (string.IsNullOrEmpty(Mensaje))
            {/*Si no hay ningun mensaje, significa que no ha habido ningun error*/

                //return objCapaDato.Registrar(obj, out Mensaje);
                return objCapaDato.Registrar(obj, DetallePrestamo, /*EjemplarActivo, */ out Mensaje);
            }
            else
            {
                return false;/*No se ha creado la Libro*/
            }
            //return objCapaDato.Registrar(obj, DetallePrestamo, /*EjemplarActivo, */ out Mensaje);
        }
    }
}
