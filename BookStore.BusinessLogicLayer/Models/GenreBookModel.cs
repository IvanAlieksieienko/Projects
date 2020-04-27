namespace BookStore.BusinessLogicLayer.Models
{
    public class GenreBookModel
    {
        public int ID { get; set; }
        public int BookID { get; set; }
        public int GenreID { get; set; }
        public string GenreName { get; set; }
        public string BookTitle { get; set; }
    }
}
