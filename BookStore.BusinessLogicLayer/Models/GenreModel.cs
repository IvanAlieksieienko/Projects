using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Models
{
    public class GenreModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<GenreBookModel> GenreBook { get; set; }
    }
}
