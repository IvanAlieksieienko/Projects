using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.BusinessLogicLayer.InputModels
{
    public struct BookInputModel
    {
        public string Title { get; set; }
        public string[] Authors { get; set; }
        public string[] Genres { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }

        public BookInputModel(string title, string[] authors, string[] genres, DateTime releaseDate, decimal price)
        {
            Title = title;
            Authors = authors;
            Genres = genres;
            ReleaseDate = releaseDate;
            Price = price;
        }
    }
}
