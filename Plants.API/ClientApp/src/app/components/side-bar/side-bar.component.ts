import { Component } from "@angular/core";
import { CategoryService } from "src/app/services/category.service";
import { CategoryModel } from "src/app/models/category.model";
import { SharedService } from "src/app/services/shared.service";
import { LoginService } from "src/app/services/login.service";
import { Router } from "@angular/router";

@Component({
    selector: "side-bar",
    templateUrl: "./side-bar.component.html",
    styleUrls: ["./side-bar.component.css"]
})
export class SideBarComponent {

    public _serviceCategory: CategoryService;
    public _serviceLogin: LoginService;
    public _categories: CategoryModel[];
    public _isShowCategories: boolean = true;

    constructor(serviceCategory: CategoryService, public _sharedService: SharedService, serviceLogin: LoginService, public router: Router) {
        this._serviceCategory = serviceCategory;
        this._serviceLogin = serviceLogin;
    }

    ngOnInit() {
        this._serviceLogin.logined().subscribe(response => {
            console.log(response);
            if (response != null) {
                this._sharedService._isAuthenticated = true;
            }
            else {
            }
        })
        this._serviceCategory.getAll().subscribe(response => {
            if (response != null) {
                this._categories = response;
                if (response.length == 0) {
                    this._isShowCategories = false;
                }
                else this._isShowCategories = true;
                console.log(response);
            }
        })
    }

    checkCategory(category: CategoryModel) {
        console.log(category);
        this.router.navigate(['category/get', category.id ]);
    }
}