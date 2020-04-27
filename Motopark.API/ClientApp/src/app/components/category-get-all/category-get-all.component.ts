import { Component } from "@angular/core";
import { CategoryService } from "src/app/services/category.service";
import { CategoryModel } from "src/app/models/category.model";
import { Guid } from "guid-typescript";
import { ProductModel } from "src/app/models/product.model";
import { ProductService } from "src/app/services/product.service";
import { ImageProductService } from "src/app/services/image-product.service";
import { ImageProductModel } from "src/app/models/image-product.model";
import { Router } from "@angular/router";
import { SharedService } from "src/app/services/shared.service";

@Component({
    selector: "category-get-all",
    templateUrl: "./category-get-all.component.html",
    styleUrls: ["./category-get-all.component.css"]
})
export class CategoryGetAllComponent {

    public _categoryService: CategoryService;
    public _productService: ProductService;
    public _allCategories: CategoryModel[] = new Array();
    public _categories: CategoryModel[] = new Array();
    public _products: ProductModel[] = new Array();
    public _isCategoriesEmpty: boolean = false;
    public _isProductsEmpty: boolean = false;


    constructor(categoryService: CategoryService, productService: ProductService,public sharedService:SharedService, public router: Router) {
        this._categoryService = categoryService;
        this._productService = productService;
    }

    ngOnInit() {
        this.sharedService.loadingTurn(true);
        this._categoryService.getAll().subscribe(response => {
            this._allCategories = response;
            if (response.length == 0) this._isCategoriesEmpty = true;
            this.getParentCategories(this._allCategories);
            this._productService.getAll().subscribe(response => {
                this._products = response;
                if (response.length == 0) this._isProductsEmpty = true; 
                this.sharedService.loadingTurn(false);
            });
        });
    }

    getParentCategories(categories: CategoryModel[]) {
        this._categories = categories.filter(function(category) {
            return category.categoryID == Guid.createEmpty();
        });
    }

    showCategory(id: Guid) {
        this.sharedService.loadingTurn(true);
        this.router.navigate(['category/get', id]);
    }
}