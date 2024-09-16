using System;
using System.Windows.Forms;
using System.Configuration;
using CRUDWinFormsMVP.Views;
using CRUDWinFormsMVP._Repositories;
using CRUDWinFormsMVP.Presenters;
using CRUDWinFormsMVP.Models;

namespace CRUDWinFormsMVP
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /* Establecemos la cadena de conexión desde las configuraciones
            ** de la aplicación para ello es necesario agregar la referencia
            ** al ensamblado de configuración. Seleccionamos las cadenas de
            ** conexión del administrador de configuraciones, especificamos
            ** el nombre de la cadena de conexión a la que queremos acceder
            ** y obtenemos la cadena de conexión
            */
            string sqlConnectionString = ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString;

            IMainView view = new MainView();
            
            // Instanciamos el presentador y le inyectamos los objetos vista y repositorio
            new MainPresenter(view, sqlConnectionString);
       
            Application.Run((Form) view);
        }
    }
}
