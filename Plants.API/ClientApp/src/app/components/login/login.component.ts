import { Component } from "@angular/core";
import { AdminModel } from "src/app/models/login.model";
import { LoginService } from "src/app/services/login.service";
import { SharedService } from "src/app/services/shared.service";
import { Router } from "@angular/router";
import { CanComponentDeactivate } from "src/app/services/can-deactive.guard";
import { Observable } from "rxjs";

@Component({
    selector: 'login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements CanComponentDeactivate {

    public loginString: string;
    public passwordString: string;
    public _serviceLogin: LoginService;
    public sended: boolean = false;

    constructor(serviceLogin: LoginService, public _sharedService: SharedService, public router: Router) {
        this._serviceLogin = serviceLogin;
    }

    ngOnInit() {

    }

    login() {
        var model = new AdminModel();
        model.login = "";
        model.password = "";
        model.login = this.loginString;
        model.password = this.passwordString;
        console.log(model);
        console.log(this.loginString + this.passwordString);
        this.sended = true;
        if ((model.login != "" && model.login != undefined) && (model.password != "" && model.password != undefined)) {
            this._serviceLogin.login(model).subscribe(response => {
                if (response != null) {
                    this._sharedService._isAuthenticated = true;
                    this.router.navigateByUrl("");
                }
                else {
                    alert("Логин и пароль были введены неверно!");
                }
            })
        }
        else {
            alert("Поля пустые!");
        }
    }

    public canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
        if (!this.sended) {
            return confirm('Не сохраненные изменения! Уйти?');
        }
        return true;
	}
}