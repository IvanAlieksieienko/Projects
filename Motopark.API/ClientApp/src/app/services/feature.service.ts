import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { FeatureModel } from "../models/feature.model";
import { Guid } from "guid-typescript";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root'})
export class FeatureService {
    public url = "";

    constructor(public http: HttpClient) {

    }

    public GetByID(id: Guid) : Observable<FeatureModel> {
        return this.http.get<FeatureModel>(this.url + "feature/" + id);
    }

    public GetByProductID(id: Guid) : Observable<FeatureModel[]> {
        return this.http.get<FeatureModel[]>(this.url + "feature/product/" + id);
    }
}