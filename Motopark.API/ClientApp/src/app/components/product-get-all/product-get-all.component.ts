import { Component, Input } from "@angular/core";
import { ProductService } from "src/app/services/product.service";
import { ImageProductService } from "src/app/services/image-product.service";
import { ProductModel } from "src/app/models/product.model";
import { ImageProductModel } from "src/app/models/image-product.model";
import { ProductDefaulImageModel } from "src/app/models/product-def-img.model";
import { Router } from "@angular/router";
import { Guid } from "guid-typescript";
import { BasketService } from "src/app/services/basket.service";
import { BasketModel } from "src/app/models/basket.model";
import { SharedService } from "src/app/services/shared.service";

@Component({
    selector: "product-get-all",
    templateUrl: "./product-get-all.component.html",
    styleUrls: ["./product-get-all.component.css"]
})
export class ProductGetAllComponent {

    public _productService: ProductService;
    public _imageProductService: ImageProductService;
    public _basketService: BasketService;
    @Input() _products_all: ProductModel[] = new Array();
    public _products: ProductDefaulImageModel[] = new Array();
    public _images: ImageProductModel[] = new Array();
    public _isProductsEmpty: boolean = false;
    public _isShowWorking: boolean = true;

    public page: number = 1;
    public pageSize: number = 5;
    

    constructor(productService: ProductService, imageProductService: ImageProductService, basketService: BasketService, public router: Router,
        private sharedService: SharedService) {
        this._productService = productService;
        this._imageProductService = imageProductService;
        this._basketService = basketService;
    }

    ngOnInit() {

        if (this._products_all == undefined || this._products_all.length == 0) this._isProductsEmpty = true;
        else {
            this._isProductsEmpty = false;
            this.getDefaultImages();
        }
    }

    ngOnChanges() {
        this._products = new Array();
        if (this._products_all.length == 0) this._isProductsEmpty = true;
        else {
            this._isProductsEmpty = false;
            this.getDefaultImages();
        }
    }

    getDefaultImages() {
        var that = this;
        this._products_all.forEach(function (product) {
            that._imageProductService.getByProductID(product.id).subscribe(response => {
                var productModel = new ProductDefaulImageModel();
                productModel.product = product;
                var image = response.find(p => p.isFirst == true);
                productModel.image = image;
                that._products.push(productModel);
            });
        });
    }

    goToProduct(id: Guid) {
        
        if (this._isShowWorking == true) {
            this.sharedService.loadingTurn(true);
            this.router.navigate(['product/get', id]);
        }
        else this._isShowWorking = true;
    }

    addToBasket(product: ProductModel) {
        this._isShowWorking = false;
        var newBasket = new BasketModel();
        newBasket.id = this.sharedService._basketID;
        newBasket.productID = product.id;
        newBasket.count = 1;
        this._basketService.add(newBasket).subscribe(response => {
            this.sharedService._basketID = response.id;
        });
    }
}