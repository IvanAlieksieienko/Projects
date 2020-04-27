import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Guid } from "guid-typescript";
import { Observable } from "rxjs";
import { DeliveryModel } from "../models/delivery.model";

@Injectable({providedIn: 'root'})
export class DeliveryService {
    public url = "";

    constructor(public http: HttpClient) {}

    public getByOrderID(id: Guid) : Observable<DeliveryModel> {
        return this.http.get<DeliveryModel>(this.url + "delivery/" + id);
    }

    public add(model: DeliveryModel) : Observable<DeliveryModel> {
        return this.http.post<DeliveryModel>(this.url + "delivery/add", model);
    }
}