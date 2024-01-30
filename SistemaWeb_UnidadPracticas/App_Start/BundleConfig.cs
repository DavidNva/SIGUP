using System.Web;
using System.Web.Optimization;

namespace SistemaWeb_UnidadPracticas
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new Bundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));

            bundles.Add(new Bundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new Bundle("~/bundles/complementos").Include(
                       "~/Scripts/fontawesome/all.min.js",
                       "~/Scripts/scripts.js", /*El nuevo archivo de JavaScript copiado*/
                       "~/Scripts/DataTables/jquery.dataTables.min.js",/*Para las tablas incluida nueva*/
                       "~/Scripts/DataTables/dataTables.responsive.min.js",
                       "~/Scripts/loadingoverlay/loadingoverlay.min.js",
                       "~/Scripts/sweetalert.min.js",
                        "~/Scripts/jquery.validate.js", /*Para aplicar validaciones, como de numeros, no insercion letras, simbolos, etc*/
                       "~/Scripts/jquery-ui.js" /*Para el calendario*/
                       ));


            //bundles.Add(new Bundle("~/bundles/complementos").Include(
            //            "~/Scripts/fontawesome/all.min.js", /*Nuevo elemento agregado para iconos*/
            //            "~/Scripts/DataTables/jquery.dataTables.min.js",/*Para las tablas incluida nueva*/
            //            "~/Scripts/DataTables/dataTables.responsive.min.js",
            //            "~/Scripts/loadingoverlay/loadingoverlay.min.js",
            //            "~/Scripts/sweetalert.min.js",
            //             "~/Scripts/jquery.validate.js", /*Para aplicar validaciones, como de numeros, no insercion letras, simbolos, etc*/
            //             "~/Scripts/jquery-ui.js", /*Para el calendario*/
            //             "~/Scripts/scripts.js")); /*El nuevo archivo de JavaScript copiado*/


            /*Cambiamos a un archivo bootstrap.bundle
             al igual que le quitamos el prefijo de ScriptBundle ya que la version 5 no responde con una descripcion
            completa de ScriptBundle (como en la 3) por lo que ahora solo indicamos un Bundle*/
            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.js"));

            /*Aqui solo vamos a utilizar el site.css ubicada en la ruta especificada (dentro de la carpeta content)*/
            bundles.Add(new StyleBundle("~/Content/css").Include(/*"~/Content/bootstrap.css",*/
                "~/Content/site.css",
                "~/Content/DataTables/css/jquery.dataTables.min.css",
                "~/Content/DataTables/css/responsive.dataTables.min.css",
                "~/Content/sweetalert.css",
                 "~/Content/jquery-ui.css" /*Para el calendario*/
                 //,
                 //"~/Content/DataTables/css/jquery.dataTables.min.css",
                 //"~/Content/DataTables/css/responsive.dataTables.min.css",
                 //"~/Content/sweetalert.css",
                 //"~/Content/jquery-ui.css" /*Para el calendario*/
                ));
        }
    }
}
