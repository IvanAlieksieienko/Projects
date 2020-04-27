import { DatePipe } from "@angular/common";
import { AuthorBookModel } from "./authorbook.model";
import { GenreBookModel } from "./genrebook.model";

export class BookModel {
    public id: number;
    public title: string;
    public authorBook: Array<AuthorBookModel>;
    public genreBook: Array<GenreBookModel>;
    public releaseDate: DatePipe;
    public price: number;
}