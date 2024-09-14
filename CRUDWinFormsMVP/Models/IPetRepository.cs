using System.Collections.Generic;

namespace CRUDWinFormsMVP.Models
{
    public interface IPetRepository
    {
        // PetModel Get(int id);
        void Add(PetModel petModel);
        void Edit(PetModel petModel);
        void Delete(int id);
        IEnumerable<PetModel> GetAll();
        IEnumerable<PetModel> GetByValue(string value); // Searchs
    }
}
