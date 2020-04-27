import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { CategoryModel } from "../models/category.model";
import { Guid } from "guid-typescript";
import { Observable } from "rxjs";
import { ImagePath } from "../models/image-path-model";

@Injectable({ providedIn: 'root' })
export class CategoryService {
    public url = "";

    constructor(public http: HttpClient) {}

    public add(model: CategoryModel) : Observable<CategoryModel> {
        return this.http.post<CategoryModel>(this.url + "category/add", model);
    }

    public getAll() : Observable<CategoryModel[]> {
        return this.http.get<CategoryModel[]>(this.url + "category/all");
    }

    public getByID(id: Guid) : Observable<CategoryModel> {
        return this.http.get<CategoryModel>(this.url + "category/" + id);
    }

    public update(model: CategoryModel) : Observable<CategoryModel>{
        return this.http.put<CategoryModel>(this.url + "category/update", model);
    }

    public delete(id: Guid) {
        return this.http.delete(this.url + "category/" + id);
    }

    public uploadImage(file: File) : Observable<ImagePath> {
        var formData = new FormData();
        formData.append('file', file, file.name);
        
        return this.http.post<ImagePath>(this.url + "category/upload", formData);
        formData.delete('file');
    }
}