import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { LoginModel } from "../models/login.model";
import { RegisterModel } from "../models/register.model";


@Injectable({ providedIn: 'root' })

export class AccountService {
    private url = "";

    constructor(private http: HttpClient) {
    }

    public login_with_model(model: LoginModel) {
        return this.http.post(this.url + "/Account/Login", model);
    }

    public check_role() {
        return this.http.get(this.url + "/Account/AuthorizeRole", { responseType: 'text' });
    }

    public register_with_model(model: RegisterModel) {
        return this.http.post(this.url + "/Account/Register", model);
    }

    public get_current_user_info() {
        return this.http.get(this.url + "/Account/GetCurrentUser");
    }

    public get_current_user_id() {
        return this.http.get(this.url + "/Account/GetCurrentUserID", { responseType: 'text' });
    }

    public logout() {
        return this.http.get(this.url + "/Account/Logout");
    }
}