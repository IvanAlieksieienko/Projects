import { Component } from "@angular/core";
import { CategoryService } from "src/app/services/category.service";
import { CategoryModel } from "src/app/models/category.model";
import { Guid } from "guid-typescript";
import { Router } from "@angular/router";
import { SharedService } from "src/app/services/shared.service";

@Component({
    selector: "side-bar",
    templateUrl: "./side-bar.component.html",
    styleUrls: ["./side-bar.component.css"]
})
export class SideBarComponent {

    public _categoryService: CategoryService;
    public _allCategories: CategoryModel[] = new Array();
    public _categories: CategoryModel[] = new Array();
    public _isCategoriesEmpty: boolean = false;

    constructor(categoryService: CategoryService, public router: Router, public _sharedService: SharedService) {
        this._categoryService = categoryService;
    }

    ngOnInit() {
        this._categoryService.getAll().subscribe(response => {
            this._allCategories = response;
            if (response.length == 0) this._isCategoriesEmpty = true;
            this.getParentCategories(this._allCategories);
        });
    }

    getParentCategories(categories: CategoryModel[]) {
        this._categories = categories.filter(function(category) {
            return category.categoryID == Guid.createEmpty();
        });
    }

    goToAll() {
        this._sharedService._isShowSideBar = false;
        this._sharedService.loadingTurn(true);
        this.router.navigateByUrl('category');
    }

    showCategory(id: Guid) {
        this._sharedService._isShowSideBar = false;
        this._sharedService.loadingTurn(true);
        this.router.navigate(['category/get', id]);
    }
}