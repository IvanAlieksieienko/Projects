import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BookInputModel } from "../inputModels/book.inputModel";


@Injectable({ providedIn: 'root' })

export class BookService {
        private url = "";

        constructor(private http: HttpClient) {

        }

        public get_all() {
                return this.http.get(this.url + "Book");
        }

        public get_by_id(id: number) {
                return this.http.get(this.url + "Book/" + id);
        }

        public delete(id: number) {
                return this.http.delete(this.url + "Book/" + id);
        }

        public add_book(inputModel: BookInputModel) {
                console.log("post works!!!");
                return this.http.post(this.url + "Book", inputModel);
        }

        public update_book(inputModel: BookInputModel, id: number) {
                return this.http.put(this.url + "Book/" + id, inputModel);
        }

        public order_book(id: number) {
                return this.http.get(this.url + "Book/Order/BookID/" + id);
        }

        public buy_book(BookId: number) {
                return this.http.get(this.url + "Book/Buy/BookID/" + BookId.toString());
        }

        public delete_ordered_book(BookId: number) {
                return this.http.delete(this.url + "Book/Order/BookID/" + BookId);
        }
}