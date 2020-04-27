using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool Confirmed { get; set; }
        public ICollection<UserBookModel> UserBook { get; set; }
        public ICollection<BasketModel> UserBasket { get; set; }
    }
}
