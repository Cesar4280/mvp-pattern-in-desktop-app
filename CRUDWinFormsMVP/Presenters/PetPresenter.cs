using System;
using System.Windows.Forms;
using System.Collections.Generic;
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
        
        private void AddNewPet(object sender, EventArgs e) => _view.IsEdit = false;
        
        private void LoadSelectedPetToEdit(object sender, EventArgs e)
        {
            /* Cuando el evento editar se ejecute cargaremos los datos de la
            ** mascota seleccionada en los cuadros de texto del formulario
            ** para poder editarlo, para ello debemos obtener el modelo
            ** mascota entonces accedemos a la fuente vinculante de la lista
            ** de mascotas y obtenemos el objeto de la fila seleccionada
            ** actualmente, esta propiedad obtiene el elemento actual de la
            ** lista subyacente
            */
            var pet = (PetModel) _petsBindingSource.Current;
            _view.PetId = pet.Id.ToString();
            _view.PetName = pet.Name;
            _view.PetType = pet.Type;
            _view.PetColour = pet.Colour;
            _view.IsEdit = true;
        }
        
        private void SavePet(object sender, EventArgs e)
        {
            /* El valor del estado editar de la vista cambia de acuerdo
            ** a la acción del usuario ya sea que los datos se agreguen
            ** o se editen y el evento guardar se ocupa del trabajo final
            */
            int length = _view.PetId.Length;
            var model = new PetModel
            {
                Id = length == 0 ? length : Convert.ToInt32(_view.PetId),
                Name = _view.PetName,
                Type = _view.PetType,
                Colour = _view.PetColour
            };
            try
            {
                /* Bueno antes de agregar o editar el modelo debemos
                ** validar que los datos de la vista sean correctos
                ** ya que en el modelo hicimos algunas validaciones
                ** por ejemplo no permitir datos vacios y la
                ** longitud mínima y máxima sin embargo esos atributos
                ** validadores necesitan ser procesados para llevar
                ** a cabo las validaciones correspondientes
                */
                new Common.ModelDataValidation().Validate(model);

                if (_view.IsEdit) // Edit model
                {
                    _repository.Edit(model);
                    _view.Message = "Pet edited successfully";
                }
                else // Add new model
                {
                    _repository.Add(model);
                    _view.Message = "Pet added successfully";
                }
                _view.IsSuccessful = true;
                LoadAllPetList();
                CleanViewFields();
            }
            catch (Exception ex)
            {
                /* Si se produce algún error le indicamos a la vista que
                ** la operación bo tuvo éxito y también establecemos  el
                ** mensaje de error
                */
                _view.IsSuccessful = false;
                _view.Message = ex.Message;
            }
        }
        
        private void CleanViewFields()
        {
            _view.PetId = "0";
            _view.PetName = string.Empty;
            _view.PetType = string.Empty;
            _view.PetColour = string.Empty;
        }

        private void CancelAction(object sender, EventArgs e) => CleanViewFields();

        private void DeleteSelectedPet(object sender, EventArgs e)
        {
            try
            {
                var pet = (PetModel) _petsBindingSource.Current;
                _repository.Delete(pet.Id);
                _view.Message = "Pet deleted successfully";
                LoadAllPetList();
            }
            catch (Exception)
            {
                _view.IsSuccessful = false;
                _view.Message = "An error ocurred, could not delete pet";
            }
        }
    }
}
