import { faChevronCircleDown, faShoppingBasket, faBicycle } from '@fortawesome/free-solid-svg-icons';
import { Component } from '@angular/core';
import { SharedService } from 'src/app/services/shared.service';
import { BasketService } from 'src/app/services/basket.service';
import { BasketModel } from 'src/app/models/basket.model';
import { ProductService } from 'src/app/services/product.service';
import { ImageProductService } from 'src/app/services/image-product.service';
import { ProductDefaulImageModel } from 'src/app/models/product-def-img.model';
import { ProductBasketModel } from 'src/app/models/product-basket.model';
import { Router } from '@angular/router';

@Component({
    selector: 'app-nav-menu',
    templateUrl: './nav-menu.component.html',
    styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
    mainIcon = faBicycle;
    icon = faShoppingBasket;
    toggleIcon = faChevronCircleDown;
    isExpanded = false;
    public _basketService: BasketService;
    public _productService: ProductService;
    public _imageProductService: ImageProductService;
    public products: ProductBasketModel[] = Array();
    public _baskets: BasketModel[];
    public _isBasketsEmpty: boolean = true;
    public _basketsCount: number = 0;

    constructor(public _sharedService: SharedService, basketService: BasketService, productService: ProductService, imageProductService: ImageProductService,
        public router: Router) {
        this._basketService = basketService;
        this._imageProductService = imageProductService;
        this._productService = productService;
     }

    collapse() {
        this.isExpanded = false;
    }

    toggle() {
        this.isExpanded = !this.isExpanded;
    }

    showSideBar() {
        this._sharedService._isShowSideBar = !this._sharedService._isShowSideBar;
    }

    getBaskets() {
        var that = this;
        this._basketService.getByBasketID(this._sharedService._basketID).subscribe(response => {
            this._baskets = response;
            if (response.length > 0) {
                this._isBasketsEmpty = false;
                this._basketsCount = response.length;
                that.products = [];
                response.forEach(function(basket) {
                    that._productService.getByID(basket.productID).subscribe(response1 => {
                        that._imageProductService.getByProductID(response1.id).subscribe(response2 => {
                            var imageProduct = new ProductBasketModel();
                            imageProduct.basket = basket;
                            imageProduct.productImage = new ProductDefaulImageModel();
                            imageProduct.productImage.image = response2.find(p => p.isFirst == true);
                            imageProduct.productImage.product = response1;
                            that.products.push(imageProduct);
                        });
                    })
                });
            }
            else {
                this._basketsCount = 0;
                this._isBasketsEmpty = true;
            }
        });
    }

    goToOrder() {
        this.router.navigate(['order/', this._sharedService._basketID]);
    }

    uncollapse() {
        this.isExpanded = false;
    }
}
