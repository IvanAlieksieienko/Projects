import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Guid } from "guid-typescript";
import { Observable } from "rxjs";
import { BasketModel } from "../models/basket.model";

@Injectable({providedIn: 'root'})
export class BasketService {
    public url = "";

    constructor(public http: HttpClient) {}

    public getByBasketID(id: Guid) : Observable<BasketModel[]> {
        return this.http.get<BasketModel[]>(this.url + "basket/get/" + id);
    }

    public getBasketID() : Observable<Guid> {
        return this.http.get<Guid>(this.url + "basket/getID");
    }

    public add(model: BasketModel) : Observable<BasketModel> {
        return this.http.post<BasketModel>(this.url + "basket", model);
    }

    public changeCount(model: BasketModel) : Observable<BasketModel> {
        return this.http.post<BasketModel>(this.url + "basket/update", model);
    }

    public delete(id: Guid) : Observable<Guid> {
        return this.http.delete<Guid>(this.url + "basket/" + id);
    } 

    public deleteByProduct(id: Guid, basketId: Guid) {
        return this.http.delete(this.url + "basket/product/" + id + "/" + basketId);
    }
}