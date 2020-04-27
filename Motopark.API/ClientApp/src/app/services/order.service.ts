import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { OrderModel } from "../models/order.model";
import { Observable } from "rxjs";

@Injectable({providedIn: 'root'})
export class OrderService {
    public url = "";

    constructor(public http: HttpClient) {}

    public add(model: OrderModel) : Observable<OrderModel> {
        return this.http.post<OrderModel>(this.url + "order", model);
    }
}