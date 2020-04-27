using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DataAccessLayer.Dapper.Entities
{
    public class UserBook
    {
        public int UserID { get; set; }
        public User User { get; set; }
        public int ID { get; set; }
        public Book Book { get; set; }
        public int BookID { get; set; }
    }
}
