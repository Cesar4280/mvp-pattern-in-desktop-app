using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CRUDWinFormsMVP.Models;
using CRUDWinFormsMVP.Views;

namespace CRUDWinFormsMVP.Presenters
{
    /* El presentador es responsable de interpretar los
    ** eventos del usuario, la comunicación con los
    ** objetos del modelo y la vista, mueve las cosas
    ** de un componente a otro según sea necesario.
    ** Eso es básicamente lo que estamos haciendo
    ** ahora mismo en esta clase eso se logra porque
    ** el presentador admite como parametro las
    ** interfaces que implementan la vista y el
    ** repositorio de tal forma que el presentador
    ** puede operar y manipularlas, sin embargo, el
    ** presentador no toca directamente la vista o el
    ** repositorio sino que lo hace a través de las
    ** abstracciones. La idea es que una clase no debe
    ** depender ni crear instancias concretas de otra
    ** clase más bien debería depender de una interfaz
    ** abstracta que esa clase implemente y en la que
    ** se inyecte una instancia concreta básicamente
    ** eso es el principio de inversión de dependencia
    */
    public class PetPresenter
    {
        // Fields
        private IPetView _view;
        private IPetRepository _repository;
        private BindingSource _petsBindingSource;
        private IEnumerable<PetModel> _petList;

        // Constructor
        public PetPresenter(IPetView view, IPetRepository repository)
        {
            _petsBindingSource = new BindingSource();
            _view = view;
            _repository = repository;

            // Subscribe Event Handler to View Events
            _view.SearchEvent += SearchPet;
            _view.AddNewEvent += AddNewPet;
            _view.EditEvent += LoadSelectedPetToEdit;
            _view.DeleteEvent += DeleteSelectedPet;
            _view.SaveEvent += SavePet;
            _view.CancelEvent += CancelAction;

            // Set pets binding source
            _view.SePetListBindingSource(_petsBindingSource);

            // Load pet list view
            LoadAllPetList();

            // Show view
            _view.Show();
        }

        // Methods
        private void LoadAllPetList()
        {
            _petList = _repository.GetAll();
            /* Al hacer esto, el DataGridView ya puede
            ** mostrar los datos de la lista de mascotas
            ** y se actualiza de manera automática cada
            ** vez que cambia el enlace de datos
            */
            _petsBindingSource.DataSource = _petList; // Set data source.
        }
        private void SearchPet(object sender, EventArgs e)
        {
            bool emptyValue = string.IsNullOrWhiteSpace(_view.SearchValue);
            _petList = emptyValue 
                ? _repository.GetAll()
                : _repository.GetByValue(_view.SearchValue);
            _petsBindingSource.DataSource = _petList;
        }
        private void CancelAction(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void SavePet(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void DeleteSelectedPet(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void LoadSelectedPetToEdit(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void AddNewPet(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
