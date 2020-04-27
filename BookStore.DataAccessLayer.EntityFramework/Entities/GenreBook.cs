using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DataAccessLayer.EntityFramework.Entities
{
    public class GenreBook
    {
        public int ID { get; set; }
        public Book Book { get; set; }
        public Genre Genre { get; set; }
        public int BookID { get; set; }
        public int GenreID { get; set; }
    }

}
