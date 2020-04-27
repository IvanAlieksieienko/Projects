import { Component, ɵ_sanitizeHtml } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { switchMap } from "rxjs/operators";
import { SharedService } from "src/app/services/shared.service";
import { Guid } from "guid-typescript";
import { ProductService } from "src/app/services/product.service";
import { OrderService } from "src/app/services/order.service";
import { BasketService } from "src/app/services/basket.service";
import { ImageProductService } from "src/app/services/image-product.service";
import { ProductBasketModel } from "src/app/models/product-basket.model";
import { BasketModel } from "src/app/models/basket.model";
import { ProductDefaulImageModel } from "src/app/models/product-def-img.model";
import { faArrowLeft, faArrowCircleLeft, faArrowCircleRight } from '@fortawesome/free-solid-svg-icons';
import { DeliveryModel } from "src/app/models/delivery.model";
import { OrderModel } from "src/app/models/order.model";
import { DeliveryService } from "src/app/services/delivery.service";
import { DatePipe } from "@angular/common";

@Component({
    selector: "order",
    templateUrl: "./order.component.html",
    styleUrls: ["./order.component.css"]
})
export class OrderComponent {

    backArrow = faArrowLeft;
    arrowLeft = faArrowCircleLeft;
    arrowRight = faArrowCircleRight;
    public basketID: Guid;
    public _productService: ProductService;
    public _orderService: OrderService;
    public _basketService: BasketService;
    public _deliveryService: DeliveryService;
    public _imageProductService: ImageProductService;
    public products: ProductBasketModel[] = Array();
    public _baskets: BasketModel[];
    public _delivery: DeliveryModel;
    public _order: OrderModel;
    public _isBasketsEmpty: boolean = true;
    public _basketsCount: number = 0;

    public postName: string = "Тип почты";
    public deliveryType: string = "Тип доставки";
    public payType: string = "Способ оплаты";

    constructor(public activateRoute: ActivatedRoute, public sharedService: SharedService, imageProductService: ImageProductService,
        orderService: OrderService, productService: ProductService, basketService: BasketService, deliveryService: DeliveryService,
        public router: Router) {
        this._imageProductService = imageProductService;
        this._orderService = orderService;
        this._productService = productService;
        this._basketService = basketService;
        this._deliveryService = deliveryService;
    }

    ngOnInit() {

        var that = this;
        this.activateRoute.paramMap.pipe(
            switchMap(params => params.getAll('id'))
        ).subscribe(response => {
            that.sharedService.loadingTurn(true);
            that._delivery = new DeliveryModel();
            that._order = new OrderModel();
            that._order.totalPrice = 0;
            that.basketID = Guid.parse(response);
            that._basketService.getByBasketID(that.basketID).subscribe(response => {
                that._baskets = response;
                if (response.length > 0) {
                    that._isBasketsEmpty = false;
                    that._basketsCount = response.length;
                    that.products = [];
                    response.forEach(function (basket) {
                        that._productService.getByID(basket.productID).subscribe(response1 => {
                            that._imageProductService.getByProductID(response1.id).subscribe(response2 => {
                                var imageProduct = new ProductBasketModel();
                                imageProduct.basket = basket;
                                imageProduct.productImage = new ProductDefaulImageModel();
                                imageProduct.productImage.image = response2.find(p => p.isFirst == true);
                                imageProduct.productImage.product = response1;
                                that._order.totalPrice += (basket.count * response1.price);
                                that.products.push(imageProduct);
                            });
                        })
                    });
                    that.sharedService.loadingTurn(false);
                }
                else {
                    this._basketsCount = 0;
                    this._isBasketsEmpty = true;
                }
            });
        });
    }

    pickPostName(type: number) {
        switch (type) {
            case 1: {
                this._delivery.postName = 1;
                this.postName = "Новая почта";
                break;
            }
            case 2: {
                this._delivery.postName = 2;
                this.postName = "Meest Express";
                break;
            }
            case 3: {
                this._delivery.postName = 3;
                this.postName = "Укрпочта";
                break;
            }
            case 0: {
                this._delivery.postName = 0;
                this.postName = "Самовывоз";
                break;
            }
            default: {
                this.postName = "Тип почты";
                break;
            }
        }
    }

    pickDeliveryType(type: number) {
        switch (type) {
            case 0: {
                this._delivery.deliveryName = 0;
                this.deliveryType = "Курьер";
                break;
            }
            case 1: {
                this._delivery.deliveryName = 1;
                this.deliveryType = "Отделение";
                break;
            }
            default: {
                this.deliveryType = "Тип доставки";
                break;
            }
        }
    }

    pickPayType(type: number) {
        switch (type) {
            case 0: {
                this._delivery.payType = 0;
                this.payType = "Наличные";
                break;
            }
            case 1: {
                this._delivery.payType = 1;
                this.payType = "Безналичный";
                break;
            }
            default: {
                this.payType = "Способ оплаты";
                break;
            }
        }
    }

    order() {
        var elem = document.getElementById("dropdownBasic1");
        debugger
        elem.style.border = "1px solid red !important";
        var errors = false;
        var errorMessage = "";
        if (this._delivery.postName == undefined) {
            errorMessage += "Не выбрана почта!\n";
            errors = true;
        }
        else {
            if (this._delivery.payType == undefined) {
                errorMessage += "Не выбран способ оплаты!\n";
                errors = true;
            }
            if (this._delivery.postName != 0) {
                if (this._delivery.deliveryName == undefined) {
                    errorMessage += "Не выбран тип доставки!\n";
                    errors = true;
                }
                if (this._delivery.region == undefined || this._delivery.region == "") {
                    errorMessage += "Не введен регион!\n";
                    errors = true;
                }
                if (this._delivery.city == undefined || this._delivery.city == "") {
                    errorMessage += "Не введен город!\n";
                    errors = true;
                }
                if (this._delivery.houseNumber == undefined || this._delivery.houseNumber == "") {
                    if (this._delivery.deliveryName == 0)
                        errorMessage += "Не введен номер дома!\n";
                    else
                        errorMessage += "Не введен номер отделения!\n";
                    errors = true;
                }
                if (this._delivery.street == undefined || this._delivery.street == "") {
                    errorMessage += "Не введена улица!\n";
                    errors = true;
                }
            }
        }
        if (this._order.name == undefined || this._order.name == "") {
            errorMessage += "Не введено имя!\n";
            errors = true;
        }
        if (this._order.surname == undefined || this._order.surname == "") {
            errorMessage += "Не введена фамилия!\n";
            errors = true;
        }
        if (this._order.patronymic == undefined || this._order.patronymic == "") {
            errorMessage += "Не введено отчество!\n";
            errors = true;
        }
        if (this._order.phoneNumber == undefined || this._order.phoneNumber == "") {
            errorMessage += "Не введен номер телефона!\n";
            errors = true;
        }
        if (this._order.totalPrice == 0) {
            errorMessage += "Сумма заказа равна нулю!\n";
            errors = true;
        }
        if (errors) alert(errorMessage);
        else {
            var that = this;
            that._order.basketID = this.sharedService._basketID;
            this._deliveryService.add(this._delivery).subscribe(response => {
                that._order.deliveryID = response.id;
                that._order.creationDate = new Date();
                that._order.orderState = 0;
                that._orderService.add(that._order).subscribe(response1 => {
                    that._basketService.delete(that.basketID).subscribe(response => {
                        that.sharedService._basketID = response;
                    });
                    this.sharedService.loadingTurn(true);
                    that.router.navigateByUrl("category");
                })
            });
        }
    }

    increase(basket: BasketModel) {
        basket.count++;
        this._basketService.changeCount(basket).subscribe();
    }

    decrease(basket: BasketModel) {
        if (basket.count == 1) {
            if (confirm("Хотите удалить продукт из заказа?")) {
                this._basketService.deleteByProduct(basket.id, basket.productID);
            }
            else {
                return;
            }
        }
        else {
            basket.count--;
            this._basketService.changeCount(basket).subscribe();
        }
    }

    goToCategory() {
        this.sharedService.loadingTurn(true);
        this.router.navigateByUrl('category');
    }
}