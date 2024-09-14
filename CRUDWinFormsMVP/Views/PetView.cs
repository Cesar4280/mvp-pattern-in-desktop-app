using System;
using System.Windows.Forms;

namespace CRUDWinFormsMVP.Views
{
    public partial class PetView : Form, IPetView
    {
        // Fields
        private string _message;
        private bool _isSuccessful;
        private bool _isEdit;

        // Constructor
        public PetView()
        {
            InitializeComponent();
            AssociateAndRaiseViewEvents();
            tabControl1.TabPages.Remove(tabPagePetDetail);
        }

        private void AssociateAndRaiseViewEvents()
        { 
            /* Nos suscribimos al evento Click del botón de busqueda
            ** y generamos el evento buscar, podemos hacerlo mediante
            ** un metodo de controlador de eventos o podemos hacerlo
            ** directamente en una sola linea o bloque usando 
            ** expresión lambda o usando un delegado.
            */
            btnSearch.Click += delegate
            {
                /* Entonces accedemos al evento buscar, verificamos
                ** que sea diferente a Nulo y finalmente invocamos
                ** el evento, enviamos el parametro de objeto remitente
                ** y argumento, en este caso este formulario y un
                ** argumento de evento vacio.
                */
                SearchEvent?.Invoke(this, EventArgs.Empty);
            };
            /* Tambien generaremos el evento buscar cuando se presione
            ** la Tecla Enter ya que es mucho más interactivo y no
            ** tener la molestia de hacer click en el botón de
            ** búsqueda entonces de manera similar suscribimos el
            ** Evento KeyDown del Cuadro de Texto de Búsqueda en este
            ** caso usamos Expresión Lambda ya que será necesario
            ** obtener el argumento del evento
            */
            txtSearch.KeyDown += (sender, e) =>
            {
                /* Definimos una condición, si el Código de Tecla del
                ** argumento del evento es la Tecla Enter generamos el
                ** evento buscar
                **/
                if (e.KeyCode == Keys.Enter)
                    SearchEvent?.Invoke(this, EventArgs.Empty);

            };
        }

        // Properties
        public string PetId {
            get { return txtId.Text; }
            set { txtId.Text = value; }
        }
        public string PetName
        {
            get { return txtName.Text; }
            set => txtName.Text = value;
        }
        public string PetType {
            get { return txtType.Text; }
            set { txtType.Text = value; }
        }
        public string PetColour {
            get { return txtColour.Text; }
            set { txtColour.Text = value; }
        }

        // Another Properties
        public string SearchValue {
            get { return txtSearch.Text; }
            set { txtSearch.Text = value; }
        }
        public bool IsEdit {
            get { return _isEdit; }
            set { _isEdit = value; }
        }
        public bool IsSuccessful {
            get { return _isSuccessful; }
            set { _isSuccessful = value; }
        }
        public string Message {
            get { return _message; }
            set { _message = value; }
        }

        // Events
        public event EventHandler SearchEvent;
        public event EventHandler AddNewEvent;
        public event EventHandler EditEvent;
        public event EventHandler DeleteEvent;
        public event EventHandler SaveEvent;
        public event EventHandler CancelEvent;

        // Methods
        public void SePetListBindingSource(BindingSource petList) => dataGridView.DataSource = petList;
    }
}
