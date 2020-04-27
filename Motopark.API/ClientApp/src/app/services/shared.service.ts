import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { Guid } from "guid-typescript";
import { BasketService } from "./basket.service";

@Injectable()
export class SharedService {
    public _isShowSideBar:boolean = false;
    public _loading:boolean = false;
    public _basketID: Guid;

    loadingChange: Subject<boolean> = new Subject<boolean>();

    constructor(private _basketService: BasketService)  {
        this.loadingChange.subscribe((value) => {
            this._loading = value
        });
        _basketService.getBasketID().subscribe(response => {
            this._basketID = response;
        })
    }

    loadingTurn(turned: boolean) {
        this.loadingChange.next(turned);
    }
}