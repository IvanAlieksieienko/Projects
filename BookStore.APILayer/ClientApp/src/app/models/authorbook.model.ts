import { BookModel } from "./book.model";
import { AuthorModel } from "./author.model";

export class AuthorBookModel {
    public id: number;
    public bookTitle: string;
    public authorName: string;
    public authorID: number;
    public bookID: number;
}