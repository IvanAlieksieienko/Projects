using System.Collections.Generic;

namespace BookStore.DataAccessLayer.EntityFramework.Entities
{
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Confirmed { get; set; }
        public ICollection<UserBook> UserBook { get; set; }
        public ICollection<Basket> UserBasket { get; set; }
    }
}