import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { UserInputModel } from "../inputModels/user.inputModel";

@Injectable({ providedIn: 'root' })

export class UserService {
        private url = "";

        constructor(private http: HttpClient) {

        }

        public get_all() {
                return this.http.get(this.url + "User");
        }

        public get_by_id(id: number) {
                return this.http.get(this.url + "User/" + id);
        }

        public delete(id: number) {
                return this.http.delete(this.url + "User/" + id);
        }

        public add_user(inputModel: UserInputModel) {
                return this.http.post(this.url + "User/", inputModel);
        }

        public update_user(inputModel: UserInputModel, id: number) {
                return this.http.put(this.url + "User/" + id, inputModel);
        }

        public get_user_books(id: number) {
                return this.http.get(this.url + "User/" + id + "/Book");
        }

        public get_user_basket(id: number) {
                return this.http.get(this.url + "User/" + id + "/Basket");
        }
}