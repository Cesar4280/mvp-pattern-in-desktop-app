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
            btnClose.Click += delegate { this.Close(); };
        }

        private void AssociateAndRaiseViewEvents()
        {
            // Search
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
            // Add new
            btnAddNew.Click += delegate 
            {
                AddNewEvent?.Invoke(this, EventArgs.Empty);
                tabControl1.TabPages.Remove(tabPagePetList);
                tabControl1.TabPages.Add(tabPagePetDetail);
                tabPagePetDetail.Text = "Add new Pet";
            };
            // Edit
            btnEdit.Click += delegate
            {
                EditEvent?.Invoke(this, EventArgs.Empty);
                tabControl1.TabPages.Remove(tabPagePetList);
                tabControl1.TabPages.Add(tabPagePetDetail);
                tabPagePetDetail.Text = "Edit Pet";
            };
            // Save changes
            btnSave.Click += delegate
            {
                SaveEvent?.Invoke(this, EventArgs.Empty);
                if (_isSuccessful)
                {
                    tabControl1.TabPages.Remove(tabPagePetDetail);
                    tabControl1.TabPages.Add(tabPagePetList);
                }
                MessageBox.Show(_message);
            };
            // Cancel
            btnCancel.Click += delegate
            {
                CancelEvent?.Invoke(this, EventArgs.Empty);
                tabControl1.TabPages.Remove(tabPagePetDetail);
                tabControl1.TabPages.Add(tabPagePetList);
            };
            // Delete
            btnDelete.Click += delegate
            {
                var result = MessageBox.Show
                (
                    "Are you sure want to delete the selected pet?",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );
                if (result == DialogResult.Yes)
                {
                    DeleteEvent?.Invoke(this, EventArgs.Empty);
                    MessageBox.Show(_message);
                }
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

        // Singleton pattern (Open a single form instance)
        private static PetView _instance;

        public static PetView GetInstance(Form parentContainer)
        {
            // Evaluamos si la instancia del formulario no existe o esta desechado
            if (_instance == null || _instance.IsDisposed)
            {
                /* Ahora, cuando se crea una instancia del formulario secundario
                ** establecemos el parámetro del contenedor principal en la
                ** propiedad Mdi padre del formulario secundario. Quitamos el
                ** estilo de borde y establecemos la propiedad acople en llenar
                */
                _instance = new PetView()
                {
                    MdiParent = parentContainer,
                    FormBorderStyle = FormBorderStyle.None,
                    Dock = DockStyle.Fill
                };
            }
            else
            {
                // si ya existe entonces traemos el formulario al frente para mostrarlo
                // otra condición para restaurar el formulario en caso de estar minimizado
                if (_instance.WindowState == FormWindowState.Minimized)
                    _instance.WindowState = FormWindowState.Normal;
                _instance.BringToFront();
            }
            // finalmente retornamos la instancia del formulario
            return _instance;
        }
    }
}
