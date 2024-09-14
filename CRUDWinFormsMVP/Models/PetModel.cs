using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CRUDWinFormsMVP.Models
{
    public class PetModel
    {
        // Fields
        private int _id;
        private string _name;
        private string _type;
        private string _colour;

        // Properties - Validations
        [DisplayName("Pet ID")]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [DisplayName("Pet Name")]
        [Required(ErrorMessage = "Pet name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Pet name must be between 3 and 50 characters")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        
        [DisplayName("Pet Type")]
        [Required(ErrorMessage = "Pet type is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Pet type must be between 3 and 50 characters")]
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
        
        [DisplayName("Pet Colour")]
        [Required(ErrorMessage = "Pet colour is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Pet colour must be between 3 and 50 characters")]
        public string Colour
        {
            get { return _colour; }
            set { _colour = value; }
        }
    }
}
