import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AuthorInputModel } from "../inputModels/author.inputModel";

@Injectable({ providedIn: 'root' })

export class AuthorService {
        private url = "";

        constructor(private http: HttpClient) {

        }

        public get_all() {
                return this.http.get(this.url + "Author");
        }

        public get_by_id(id: number) {
                return this.http.get(this.url + "Author/" + id);
        }

        public delete(id: number) {
                return this.http.delete(this.url + "Author/" + id);
        }

        public add_author(inputModel: AuthorInputModel) {
                return this.http.post(this.url + "Author/", inputModel);
        }

        public update_author(inputModel: AuthorInputModel, id: number) {
                return this.http.put(this.url + "Author/" + id, inputModel);
        }
}