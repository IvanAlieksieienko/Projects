import { Component } from "@angular/core";
import { FeatureService } from "src/app/services/feature.service";
import { ActivatedRoute, Router } from "@angular/router";
import { CategoryService } from "src/app/services/category.service";
import { ProductService } from "src/app/services/product.service";
import { SharedService } from "src/app/services/shared.service";
import { switchMap } from "rxjs/operators";
import { CategoryModel } from "src/app/models/category.model";
import { Guid } from "guid-typescript";
import { ProductModel } from "src/app/models/product.model";
import { ImageProductModel } from "src/app/models/image-product.model";
import { FeatureModel } from "src/app/models/feature.model";
import { ImageProductService } from "src/app/services/image-product.service";
import { faArrowLeft, faShoppingBasket } from '@fortawesome/free-solid-svg-icons';
import { BasketService } from "src/app/services/basket.service";
import { BasketModel } from "src/app/models/basket.model";

@Component({
    selector: "product-get-by-id",
    templateUrl: "./product-get-by-id.component.html",
    styleUrls: ["./product-get-by-id.component.css"]
})
export class ProductGetByIdComponent {

    backArrow = faArrowLeft;
    icon = faShoppingBasket;
    public categoryID: Guid;
    public productID: Guid;
    public product: ProductModel = new ProductModel();
    public images: ImageProductModel[];
    public mainImage: ImageProductModel;
    public features: FeatureModel[];
    public category: CategoryModel;
    public _categoryService: CategoryService;
    public _productService: ProductService;
    public _featureService: FeatureService;
    public _imageProductService: ImageProductService;
    public _basketService: BasketService;
    public _isFeaturesEmpty: boolean = true;

    public _isShowFullImage: boolean = false;
    public _fullImagePath: string = "";

    constructor(public activateRoute: ActivatedRoute, categoryService: CategoryService, productService: ProductService, basketService: BasketService,
        public router: Router, private sharedService: SharedService, featureService: FeatureService, imageProductService: ImageProductService) {
            this._basketService = basketService;
            this._categoryService = categoryService;
            this._productService = productService;
            this._featureService = featureService;
            this._imageProductService = imageProductService;
        }

    ngOnInit() {
        this.activateRoute.paramMap.pipe(
            switchMap(params => params.getAll('id'))
        ).subscribe(response => {
            this.sharedService.loadingTurn(true);
            this.productID = Guid.parse(response);
            this._productService.getByID(this.productID).subscribe(response => {
                this.product = response;
                this.getFeatures();
                this.getImages();
                this._categoryService.getByID(response.categoryID).subscribe(response => {
                    this.categoryID = response.id;
                    this.category = response;
                    this.sharedService.loadingTurn(false);
                });
            });
        });
    }

    getFeatures() {
        this._featureService.GetByProductID(this.product.id).subscribe(response => {
            this.features = response;
            if (response.length > 0) this._isFeaturesEmpty = false;
            else this._isFeaturesEmpty = true;
        });
    }

    getImages() {
        this._imageProductService.getByProductID(this.product.id).subscribe(response => {
            this.images = response;
            this.mainImage = this.images.find(p => p.isFirst == true);
            this.images = this.images.filter(p => p.isFirst == false);
        });
    }

    goToCategory() {
        this.sharedService.loadingTurn(true);
        this.router.navigate(['category/get', this.categoryID]);
    }

    showImage(path: string) {
        this._isShowFullImage = true;
        this._fullImagePath = path;
    }

    closeImageView() {
        this._isShowFullImage = false;
        this._fullImagePath = "";
    }

    addToBasket() {
        var newBasket = new BasketModel();
        newBasket.productID = this.product.id;
        newBasket.count = 1;
        newBasket.id = this.sharedService._basketID;
        this._basketService.add(newBasket).subscribe();
    }
}