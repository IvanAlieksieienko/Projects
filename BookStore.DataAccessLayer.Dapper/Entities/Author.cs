using System.Collections.Generic;

namespace BookStore.DataAccessLayer.Dapper.Entities
{
    public class Author
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<AuthorBook> AuthorBook { get; set; }
    }
}
