using System;
using System.Windows.Forms;

namespace CRUDWinFormsMVP.Views
{
    public interface IPetView
    {
        // Properties - Fields
        string PetId { get; set; }
        string PetName { get; set; }
        string PetType { get; set; }
        string PetColour { get; set; }

        // Another Properties
        string SearchValue { get; set; }
        bool IsEdit { get; set; }
        bool IsSuccessful { get; set; }
        string Message { get; set; }

        // Events
        event EventHandler SearchEvent;
        event EventHandler AddNewEvent;
        event EventHandler EditEvent;
        event EventHandler DeleteEvent;
        event EventHandler SaveEvent;
        event EventHandler CancelEvent;

        // Methods 
        void SePetListBindingSource(BindingSource petList);
        void Show(); // Optional
    }
}
