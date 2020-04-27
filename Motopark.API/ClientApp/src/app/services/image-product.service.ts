import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Guid } from "guid-typescript";
import { ImageProductModel } from "../models/image-product.model";

@Injectable({providedIn: 'root'})
export class ImageProductService {
    public url = "";

    constructor(public http: HttpClient) {}

    public getByProductID(id: Guid) : Observable<ImageProductModel[]> {
        return this.http.get<ImageProductModel[]>(this.url + "image/" + id);
    }
}