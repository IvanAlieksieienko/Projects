using System;
using System.Collections.Generic;

namespace BookStore.DataAccessLayer.Dapper.Entities
{
    public class Book
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public ICollection<AuthorBook> AuthorBook { get; set; }
        public ICollection<GenreBook> GenreBook { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
    }
}
