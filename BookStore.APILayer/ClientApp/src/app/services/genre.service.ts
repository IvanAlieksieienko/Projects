import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { GenreInputModel } from "../inputModels/genre.inputModel";

@Injectable({ providedIn: 'root' })

export class GenreService {
        private url = "";

        constructor(private http: HttpClient) {

        }

        public get_all() {
                return this.http.get(this.url + "Genre");
        }

        public get_by_id(id: number) {
                return this.http.get(this.url + "Genre/" + id);
        }

        public delete(id: number) {
                return this.http.delete(this.url + "Genre/" + id);
        }

        public add_genre(inputModel: GenreInputModel) {
                return this.http.post(this.url + "Genre/", inputModel);
        }

        public update_genre(inputModel: GenreInputModel, id: number) {
                return this.http.put(this.url + "Genre/" + id, inputModel);
        }
}