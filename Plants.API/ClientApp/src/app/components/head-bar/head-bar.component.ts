import { Component } from "@angular/core";
import { faSeedling } from '@fortawesome/free-solid-svg-icons';
import { SharedService } from "src/app/services/shared.service";
import { LoginService } from "src/app/services/login.service";
import { Router } from "@angular/router";

@Component({
    selector: "head-bar",
    templateUrl: "./head-bar.component.html",
    styleUrls: ["./head-bar.component.css"]
})
export class HeadBarComponent {
    mainIcon = faSeedling;
    public _serviceLogin: LoginService;

    constructor(public _sharedService: SharedService, serviceLogin: LoginService, public router: Router) {
        this._serviceLogin = serviceLogin;
    }
    
    ngOnInit() {
        this._serviceLogin.logined().subscribe(b=> {
            console.log(b);
            if (b == true) {
                this._sharedService._isAuthenticated = true;

            }
        })
    }

    logout() {
        this._serviceLogin.logout().subscribe(b=> {
            this._sharedService._isAuthenticated = false;
            this.router.navigateByUrl("");
        })
    }

    sideBar() {
        this._sharedService._isSideBarHidden = !this._sharedService._isSideBarHidden;
    }
}