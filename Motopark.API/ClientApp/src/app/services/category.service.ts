import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { CategoryModel } from "../models/category.model";
import { Guid } from "guid-typescript";
import { ImagePath } from "../models/image-path.model";

@Injectable({providedIn: 'root'})
export class CategoryService {
    public url = ""

    constructor(public http: HttpClient) {}
 
    public getAll() : Observable<CategoryModel[]> {
        return this.http.get<CategoryModel[]>(this.url + "category/all");
    }

    public getSubCategories(id: Guid) : Observable<CategoryModel[]> {
        return this.http.get<CategoryModel[]>(this.url + "category/sub/" + id);
    }

    public getByID(id: Guid) : Observable<CategoryModel> {
        return this.http.get<CategoryModel>(this.url + "category/" + id);
    }

    public uploadImage(file: File) : Observable<ImagePath> {
        var formData = new FormData();
        formData.append('file', file, file.name);
        var responseData = this.http.post<ImagePath>(this.url + "category/upload", formData);
        formData.delete('file');
        return responseData;
    }
}