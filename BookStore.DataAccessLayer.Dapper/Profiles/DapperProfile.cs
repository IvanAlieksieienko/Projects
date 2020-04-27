using AutoMapper;
using BookStore.BusinessLogicLayer.Models;
using BookStore.DataAccessLayer.Dapper.Entities;

namespace BookStore.DataAccessLayer.Dapper.Profiles
{
    public class DapperProfile : Profile
    {
        public DapperProfile()
        {
            CreateMap<Admin, AdminModel>();
            CreateMap<AdminModel, Admin>();
            CreateMap<Author, AuthorModel>().ForMember(cg => cg.AuthorBook, m => m.MapFrom(s => s.AuthorBook));
            CreateMap<AuthorModel, Author>().ForMember(cg => cg.AuthorBook, m => m.MapFrom(s => s.AuthorBook));
            CreateMap<Book, BookModel>().ForMember(cg => cg.AuthorBook, m => m.MapFrom(s => s.AuthorBook)).ForMember(cg => cg.GenreBook, m => m.MapFrom(s => s.GenreBook));
            CreateMap<BookModel, Book>().ForMember(cg => cg.AuthorBook, m => m.MapFrom(s => s.AuthorBook)).ForMember(cg => cg.GenreBook, m => m.MapFrom(s => s.GenreBook));
            CreateMap<Genre, GenreModel>().ForMember(cg => cg.GenreBook, m => m.MapFrom(s => s.GenreBook));
            CreateMap<GenreModel, Genre>().ForMember(cg => cg.GenreBook, m => m.MapFrom(s => s.GenreBook));
            CreateMap<User, UserModel>().ForMember(cg => cg.UserBasket, m => m.MapFrom(s => s.UserBasket)).ForMember(cg => cg.UserBook, m => m.MapFrom(s => s.UserBook));
            CreateMap<UserModel, User>().ForMember(cg => cg.UserBasket, m => m.MapFrom(s => s.UserBasket)).ForMember(cg => cg.UserBook, m => m.MapFrom(s => s.UserBook));

            CreateMap<AuthorBook, AuthorBookModel>().ForMember(cg => cg.AuthorID, m => m.MapFrom(s => s.AuthorID))
                .ForMember(cg => cg.ID, m => m.MapFrom(s => s.ID))
                .ForMember(cg => cg.BookID, m => m.MapFrom(s => s.BookID))
                .ForMember(cg => cg.AuthorName, m => m.MapFrom(s => s.Author.Name))
                .ForMember(cg => cg.BookTitle, m => m.MapFrom(s => s.Book.Title));

            CreateMap<AuthorBookModel, AuthorBook>().ForMember(cg => cg.AuthorID, m => m.MapFrom(s => s.AuthorID))
                .ForMember(cg => cg.ID, m => m.MapFrom(s => s.ID))
                .ForMember(cg => cg.BookID, m => m.MapFrom(s => s.BookID));

            CreateMap<GenreBook, GenreBookModel>().ForMember(cg => cg.GenreID, m => m.MapFrom(s => s.GenreID))
                .ForMember(cg => cg.ID, m => m.MapFrom(s => s.ID))
                .ForMember(cg => cg.BookID, m => m.MapFrom(s => s.BookID))
                .ForMember(cg => cg.GenreName, m => m.MapFrom(s => s.Genre.Name))
                .ForMember(cg => cg.BookTitle, m => m.MapFrom(s => s.Book.Title));

            CreateMap<GenreBookModel, GenreBook>().ForMember(cg => cg.GenreID, m => m.MapFrom(s => s.GenreID))
                .ForMember(cg => cg.ID, m => m.MapFrom(s => s.ID))
                .ForMember(cg => cg.BookID, m => m.MapFrom(s => s.BookID)); ;

            CreateMap<UserBook, UserBookModel>().ForMember(cg => cg.ID, m => m.MapFrom(s => s.BookID))
                .ForMember(cg => cg.UserID, m => m.MapFrom(s => s.UserID))
                .ForMember(cg => cg.BookID, m => m.MapFrom(s => s.BookID))
                .ForMember(cg => cg.BookTitle, m => m.MapFrom(s => s.Book.Title))
                .ForMember(cg => cg.BookPrice, m => m.MapFrom(s => s.Book.Price));

            CreateMap<UserBookModel, UserBook>().ForMember(cg => cg.ID, m => m.MapFrom(s => s.BookID))
                .ForMember(cg => cg.UserID, m => m.MapFrom(s => s.UserID))
                .ForMember(cg => cg.BookID, m => m.MapFrom(s => s.BookID));

            CreateMap<Basket, BasketModel>().ForMember(cg => cg.ID, m => m.MapFrom(s => s.BookID))
                .ForMember(cg => cg.UserID, m => m.MapFrom(s => s.UserID))
                .ForMember(cg => cg.BookID, m => m.MapFrom(s => s.BookID))
                .ForMember(cg => cg.BookTitle, m => m.MapFrom(s => s.Book.Title))
                .ForMember(cg => cg.BookPrice, m => m.MapFrom(s => s.Book.Price));

            CreateMap<BasketModel, Basket>().ForMember(cg => cg.ID, m => m.MapFrom(s => s.BookID))
                .ForMember(cg => cg.UserID, m => m.MapFrom(s => s.UserID))
                .ForMember(cg => cg.BookID, m => m.MapFrom(s => s.BookID));
        }
    }
}
