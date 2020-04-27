import { BookModel } from "./book.model";
import { GenreModel } from "./genre.model";

export class GenreBookModel {
    public id: number;
    public bookTitle: string;
    public genreName: string;
    public genreID: number;
    public bookID: number;
}