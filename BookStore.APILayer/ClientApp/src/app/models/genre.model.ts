import { GenreBookModel } from "./genrebook.model";

export class GenreModel {
    public id: number;
    public name: string;
    public genreBook: Array<GenreBookModel>;
}