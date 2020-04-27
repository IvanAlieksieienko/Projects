using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DataAccessLayer.Dapper.Entities
{
    public class GenreBook
    {
        public int ID { get; set; }
        public Book Book { get; set; }
        public Genre Genre { get; set; }

        [Key, ForeignKey("Book")]
        public int BookID { get; set; }
        [Key, ForeignKey("Genre")]
        public int GenreID { get; set; }
    }

}
