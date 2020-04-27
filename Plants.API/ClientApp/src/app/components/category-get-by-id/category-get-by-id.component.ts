import { Component } from "@angular/core";
import { Guid } from "guid-typescript";
import { ActivatedRoute, Router } from "@angular/router";
import { Subscription } from "rxjs";
import { CategoryModel } from "src/app/models/category.model";
import { CategoryService } from "src/app/services/category.service";
import { switchMap } from 'rxjs/operators';
import { ProductService } from "src/app/services/product.service";
import { ProductModel } from "src/app/models/product.model";
import { SharedService } from "src/app/services/shared.service";
import { faArrowLeft } from '@fortawesome/free-solid-svg-icons';

@Component({
    selector: 'category-get-by-id',
    templateUrl: './category-get-by-id.component.html',
    styleUrls: ['./category-get-by-id.component.css']
})
export class CategoryGetByIDComponent {

    icon = faArrowLeft;
    public categoryID: Guid;
    public category: CategoryModel;
    public products: ProductModel[];
    public subscription: Subscription;
    public _serviceCategory: CategoryService;
    public _serviceProduct: ProductService;
    public _isShowFullImage: boolean = false;
    public _fullImagePath: string = "";
    public _deleteMode: boolean = false;
    public _deleteList: boolean[] = new Array();

    constructor(public activateRoute: ActivatedRoute, serviceCategory: CategoryService, serviceProduct: ProductService, public _sharedService: SharedService, public router: Router) {
        this._serviceCategory = serviceCategory;
        this._serviceProduct = serviceProduct;
    }

    ngOnInit() {
        this.activateRoute.paramMap.pipe(
            switchMap(params => params.getAll('id'))
        ).subscribe(response => {
            this.categoryID = Guid.parse(response);
            console.log(this.categoryID);
            this._serviceCategory.getByID(this.categoryID).subscribe(response => {
                this.category = response;
                console.log(this.category);
                if (this.category.imagePath == "") {
                    this.category.imagePath = "Resources\\Images\\default-tree.png";
                }
                this.getCategoryProducts(response);
            })
        });
    }

    getCategoryProducts(category: CategoryModel) {
        this._serviceProduct.getByCategoryID(this.category.id).subscribe(response => {
            this.products = response;
            console.log(this.products);
        })
    }

    createProduct() {
        this.router.navigate(['product/create', this.category.id]);
    }

    deleteCategory() {
        if (confirm("Уверены, что хотите удалить эту категорию?")) {
            this._serviceCategory.delete(this.categoryID).subscribe();
            this.router.navigateByUrl("");
        }
    }

    updateCategory() {
        this.router.navigate(['category/update', this.category.id]);
    }

    showImage(path: string) {
        this._isShowFullImage = true;
        this._fullImagePath = path;
    }

    closeImageView() {
        this._isShowFullImage = false;
        this._fullImagePath = "";
    }

    deleteProductsMode() {
        this._deleteMode = !this._deleteMode;
    }

    checkProduct(product: ProductModel) {
        this.router.navigate(['product/get', product.id]);
    }

    addProductToDeleteList(product: ProductModel) {
        var index = this.products.findIndex(p => p.id == product.id);
        this._deleteList[index] = true;
    }

    removeProductFromDeleteList(product: ProductModel) {
        var index = this.products.findIndex(p => p.id == product.id);
        this._deleteList[index] = false;
    }

    deleteList() {
        if (confirm("Уверены, что хотите удалить?")) {
            if (this._deleteList.length > 0) {
                for (var i = 0; i < this._deleteList.length; i++) {
                    if (this._deleteList[i] == true) {
                        this._serviceProduct.delete(this.products[i].id).subscribe();
                    }

                }
                window.location.reload();
            }
        }
    }

    backToCategories() {
        this.router.navigateByUrl('category');
    }
}