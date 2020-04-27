using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DataAccessLayer.EntityFramework.Entities
{
    public class AuthorBook
    {
        public int ID { get; set; }
        public Book Book { get; set; }
        public Author Author { get; set; }
        public int AuthorID { get; set; }
        public int BookID { get; set; }
    }
}
