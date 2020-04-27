using System.Collections.Generic;

namespace BookStore.BusinessLogicLayer.Models
{
    public class AuthorModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<AuthorBookModel> AuthorBook { get; set; }
    }
}
