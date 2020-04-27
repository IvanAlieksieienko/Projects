using System.Collections.Generic;

namespace BookStore.DataAccessLayer.EntityFramework.Entities
{
    public class Genre
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<GenreBook> GenreBook { get; set; }
    }
}
