namespace BookStore.BusinessLogicLayer.Models
{
    public class UserBookModel
    {
        public int UserID { get; set; }
        public int ID { get; set; }
        public int BookID { get; set; }
        public decimal BookPrice { get; set; }
        public string BookTitle { get; set; }
    }
}
