import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ProductModel } from "../models/product.model";
import { Guid } from "guid-typescript";
import { ImagePath } from "../models/image-path-model";

@Injectable({ providedIn: 'root' })
export class ProductService {

    public url = "";

    constructor(public http: HttpClient) {}

    public getAll() : Observable<ProductModel[]> {
        return this.http.get<ProductModel[]>(this.url + "product/all");
    }

    public getByID(id: Guid) : Observable<ProductModel> {
        return this.http.get<ProductModel>(this.url + "product/" + id);
    }

    public getByCategoryID(id: Guid) : Observable<ProductModel[]> {
        return this.http.get<ProductModel[]>(this.url + "product/category/" + id);
    }

    public add(model: ProductModel) : Observable<ProductModel> {
        return this.http.post<ProductModel>(this.url + "product/add", model);
    }

    public update(model: ProductModel) : Observable<ProductModel> {
        return this.http.put<ProductModel>(this.url + "product/update", model);
    }

    public delete(id: Guid) {
        return this.http.delete(this.url + "product/" + id);
    }

    public uploadImage(file: File) : Observable<ImagePath> {
        var formData = new FormData();
        formData.append('file', file, file.name);
        
        return this.http.post<ImagePath>(this.url + "product/upload", formData);
        formData.delete('file');
    }
}