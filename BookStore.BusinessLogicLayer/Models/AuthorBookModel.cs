namespace BookStore.BusinessLogicLayer.Models
{
    public class AuthorBookModel
    {
        public int ID { get; set; }
        public int AuthorID { get; set; }
        public int BookID { get; set; }
        public string AuthorName { get; set; }
        public string BookTitle { get; set; }
    }
}
