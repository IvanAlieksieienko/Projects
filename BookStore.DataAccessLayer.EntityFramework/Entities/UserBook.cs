using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.DataAccessLayer.EntityFramework.Entities
{
    public class UserBook
    {
        public int ID { get; set; }
        public Book Book { get; set; }
        public int BookID { get; set; }
        public User User { get; set; }
        public int UserID { get; set; }
    }
}
