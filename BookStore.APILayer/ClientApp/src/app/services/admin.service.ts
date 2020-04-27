import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AdminInputModel } from "../inputModels/admin.inputModel";

@Injectable({ providedIn: 'root' })

export class AdminService {
        private url = "";

        constructor(private http: HttpClient) {

        }

        public get_all() {
                return this.http.get(this.url + "Admin");
        }

        public get_by_id(id: number) {
                return this.http.get(this.url + "Admin/" + id);
        }

        public delete(id: number) {
                return this.http.delete(this.url + "Admin/" + id);
        }

        public add_admin(inputModel: AdminInputModel) {
                return this.http.post(this.url + "Admin/", inputModel);
        }

        public update_admin(inputModel: AdminInputModel, id: number) {
                return this.http.put(this.url + "Admin/" + id, inputModel);
        }
}