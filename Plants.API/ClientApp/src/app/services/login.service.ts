import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { AdminModel } from "../models/login.model";

@Injectable({ providedIn: 'root' })
export class LoginService {
    public url = "";

    constructor(public http: HttpClient) {}

    public login(model: AdminModel) {
        return this.http.post(this.url + "/login", model);
    }

    public logout() {
        return this.http.get(this.url + "login/logout");
    }

    public logined() {
        return this.http.get(this.url + "login/logined");
    }
}