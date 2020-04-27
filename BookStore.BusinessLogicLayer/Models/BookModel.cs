using System;
using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Models
{
    public class BookModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public ICollection<AuthorBookModel> AuthorBook { get; set; }
        public ICollection<GenreBookModel> GenreBook { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
    }
}