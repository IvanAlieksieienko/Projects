import { Injectable, Output, EventEmitter } from "@angular/core";
import { UserModel } from "../models/user.model";
import { BookModel } from "../models/book.model";
import { AuthorModel } from "../models/author.model";
import { GenreModel } from "../models/genre.model";

@Injectable()
export class SharedService {
        public _is_authorized: boolean = false;
        public _is_admin: boolean = false;
        public _authorized_role: string;
        public _user_email: string;
        public _user_password: string;
        public _user_id: number;

        public _current_user: UserModel;

        public _all_books: BookModel[];
        public _selected_book: BookModel;
        public _is_show_selected_book: boolean = false;

        public _all_authors: AuthorModel[];
        public _selected_author: AuthorModel;
        public _is_show_selected_author: boolean = false;

        public _all_genres: GenreModel[];
        public _selected_genre: GenreModel;
        public _is_show_selected_genre: boolean = false;

        public _all_users: UserModel[];
        public _selected_user: UserModel;
        public _is_show_selected_user: boolean = false;


}