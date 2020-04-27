import { Component } from "@angular/core";
import { CategoryService } from "src/app/services/category.service";
import { CategoryModel } from "src/app/models/category.model";
import { SharedService } from "src/app/services/shared.service";
import { Router } from "@angular/router";

@Component({
    selector: 'category-get-all',
    templateUrl: './category-get-all.component.html',
    styleUrls: ['./category-get-all.component.css']
})
export class CategoryGetAllComponent {

    public _serviceCategory: CategoryService;
    public _categories: CategoryModel[] = new Array();
    public _deleteList: boolean[] = new Array();
    public _isCategories: boolean = false;
    public _deleteMode: boolean = false;

    constructor(serviceCategory: CategoryService, public _sharedService: SharedService, public router: Router) {
        this._serviceCategory = serviceCategory;
    }

    ngOnInit() {
        if (this._categories.length == 0) this._isCategories = false;
        else this._isCategories = true;
        this._serviceCategory.getAll().subscribe(response => {
            if (response != null) {
                this._categories = response;
                this._isCategories = true;
                this._deleteList = new Array<boolean>(this._categories.length);
                for (var i = 0; i < this._deleteList.length; i++) {
                    this._deleteList[i] = false;
                }
            }
            else {
                this._isCategories = false;
            }
        })
    }

    createCategory() {
        this.router.navigateByUrl('category/create');
    }

    deleteCategoriesMode() {
        this._deleteMode = !this._deleteMode;
    }

    checkCategory(category: CategoryModel) {
        this.router.navigate(['category/get', category.id]);
    }

    addCategoryToDeleteList(category: CategoryModel) {
        var index = this._categories.findIndex(p => p.id == category.id);
        this._deleteList[index] = true;
    }

    removeCategoryFromDeleteList(category: CategoryModel) {
        var index = this._categories.findIndex(p => p.id == category.id);
        this._deleteList[index] = false;
    }

    deleteList() {
        if (confirm("Уверены, что хотите удалить?")) {
            if (this._deleteList.length > 0) {
                for (var i = 0; i < this._deleteList.length; i++) {
                    if (this._deleteList[i] == true) {
                        this._serviceCategory.delete(this._categories[i].id).subscribe();
                    }
                }
                window.location.reload();
            }
        }
    }
}