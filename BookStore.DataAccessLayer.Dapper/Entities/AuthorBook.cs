using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DataAccessLayer.Dapper.Entities
{
    public class AuthorBook
    {
        public int ID { get; set; }
        public Book Book { get; set; }
        public Author Author { get; set; }

        [Key, ForeignKey("Author")]
        public int AuthorID { get; set; }
        [Key, ForeignKey("Book")]
        public int BookID { get; set; }
    }
}
