import { Component } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { switchMap } from 'rxjs/operators';
import { CategoryModel } from "src/app/models/category.model";
import { ProductService } from "src/app/services/product.service";
import { CategoryService } from "src/app/services/category.service";
import { ProductModel } from "src/app/models/product.model";
import { SharedService } from "src/app/services/shared.service";
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { Guid } from "guid-typescript";

@Component({
    selector: "category-get-by-id",
    templateUrl: "./category-get-by-id.component.html",
    styleUrls: ["./category-get-by-id.component.css"]
})
export class CategoryGetByIdComponent {

    backArrow = faArrowLeft;
    public parentCategoryID: Guid;
    public categoryID: Guid;
    public _category: CategoryModel;
    public _categoryService: CategoryService;
    public _productService: ProductService;
    public _products: ProductModel[] = new Array();
    public _testProducts: ProductModel[] = new Array();
    public _ids: Guid[] = new Array();
    public _allSubCategories: CategoryModel[] = new Array();
    public _subCategories: CategoryModel[] = new Array();
    public _isSubCategoriesEmpty: boolean = false;
    public _isProductsEmpty: boolean = false;

    public _isShowFullImage: boolean = false;
    public _fullImagePath: string = "";

    constructor(public activateRoute: ActivatedRoute, categoryService: CategoryService, productService: ProductService, public router: Router, private sharedService: SharedService) {
        this._categoryService = categoryService;
        this._productService = productService;
    }

    ngOnInit() {

        this.activateRoute.paramMap.pipe(
            switchMap(params => params.getAll('id'))
        ).subscribe(response => {
            this.sharedService.loadingTurn(true);
            this._category = new CategoryModel();
            this._products = [];
            this._ids = [];
            this.categoryID = Guid.parse(response);
            this._categoryService.getByID(this.categoryID).subscribe(response => {
                this.parentCategoryID = response.categoryID;
                this._category = response;
                this._categoryService.getSubCategories(this._category.id).subscribe(response => {
                    this._allSubCategories = response;
                    this.getProducts(response);
                    if (response.length == 0) {
                        this._isSubCategoriesEmpty = true;
                    }
                    else {
                        this._isSubCategoriesEmpty = false;
                        this.getParentCategories(response);
                    }
                    this.sharedService.loadingTurn(false);
                });
            });
        });
    }

    getParentCategories(categories: CategoryModel[]) {
        var that = this;
        this._subCategories = categories.filter(function (category) {
            return category.categoryID == that.categoryID;
        });
    }

    getProducts(categories: CategoryModel[]) {
        var that = this;
        that._ids.push(this._category.id);
        categories.forEach(function (category) {
            that._ids.push(category.id);
        });
        this._productService.getAllByCategoryID(this._ids).subscribe(response => {
            that._products = response;
            if (response.length == 0) this._isProductsEmpty = true;
            else this._isProductsEmpty = false;
        });
    }

    showCategory(id: Guid) {
        this.sharedService.loadingTurn(true);
        this.router.navigate(['category/get', id]);
    }

    showImage(path: string) {
        this._isShowFullImage = true;
        this._fullImagePath = path;
    }

    closeImageView() {
        this._isShowFullImage = false;
        this._fullImagePath = "";
    }

    backToCategory() {
        if (this.parentCategoryID == Guid.createEmpty()) {
            this.sharedService.loadingTurn(true);
            this.router.navigateByUrl('category');
        }
        else {
            this.sharedService.loadingTurn(true);
            this.router.navigate(['category/get', this.parentCategoryID]);
        }
    }
}