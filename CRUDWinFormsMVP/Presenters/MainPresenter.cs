using System;
using CRUDWinFormsMVP.Models;
using CRUDWinFormsMVP.Views;
using CRUDWinFormsMVP._Repositories;

namespace CRUDWinFormsMVP.Presenters
{
    public class MainPresenter
    {
        private IMainView _mainView;
        private readonly string _sqlConnectionString;

        public MainPresenter(IMainView mainView, string sqlConnectionString)
        {
            _mainView = mainView;
            _sqlConnectionString = sqlConnectionString;
            _mainView.ShowPetView += ShowPetsView;
        }

        private void ShowPetsView(object sender, EventArgs e)
        {
            /* Necesitaremos establecer la vista principal como el padre Mdi
            ** de la vista mascota, podemos aprovechar el metodo obtener
            ** instancia del formulario secundario
            */
            IPetView view = PetView.GetInstance((MainView) _mainView);
            IPetRepository repository = new PetRepository(_sqlConnectionString);

            // Instanciamos el presentador y le inyectamos los objetos vista y repositorio
            new PetPresenter(view, repository);
        }
    }
}
