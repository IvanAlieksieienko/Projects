namespace BookStore.DataAccessLayer.Dapper.Entities
{
    public class Basket
    {
        public int UserID { get; set; }
        public User User { get; set; }
        public int ID { get; set; }
        public Book Book { get; set; }
        public int BookID { get; set; }
    }
}
