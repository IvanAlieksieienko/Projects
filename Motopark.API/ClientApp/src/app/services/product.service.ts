import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ImagePath } from "../models/image-path.model";
import { ProductModel } from "../models/product.model";
import { Guid } from "guid-typescript";

@Injectable({providedIn: 'root'})
export class ProductService {
    public url = ""

    constructor(public http: HttpClient) {}
 
    public getAll() : Observable<ProductModel[]> {
        return this.http.get<ProductModel[]>(this.url + "product/all");
    }

    public getByCategoryID(id: Guid) : Observable<ProductModel[]> {
        return this.http.get<ProductModel[]>(this.url + "product/category/" + id);
    }

    public getByID(id: Guid) : Observable<ProductModel> {
        return this.http.get<ProductModel>(this.url + "product/" + id);
    }

    public getAllByCategoryID(ids: Guid[]) : Observable<ProductModel[]> {
        return this.http.post<ProductModel[]>(this.url + "product/category/all", ids);
    }

    public uploadImage(file: File) : Observable<ImagePath> {
        var formData = new FormData();
        formData.append('file', file, file.name);
        var responseData = this.http.post<ImagePath>(this.url + "product/upload", formData);
        formData.delete('file');
        return responseData;
    }
}