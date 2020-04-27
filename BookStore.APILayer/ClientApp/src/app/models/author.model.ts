import { AuthorBookModel } from "./authorbook.model";

export class AuthorModel {
    public id: number;
    public name: string;
    public authorBook: Array<AuthorBookModel>;
}