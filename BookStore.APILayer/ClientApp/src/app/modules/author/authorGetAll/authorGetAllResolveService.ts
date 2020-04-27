import { Injectable } from "@angular/core";
import { AuthorModel } from "src/app/models/author.model";
import { AuthorService } from "src/app/services/author.service";
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";

@Injectable()
export class AuthorGetAllResolveService implements Resolve<AuthorModel[]> {

        constructor(private _service_author: AuthorService) {

        }

        public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
                console.log("AuthorGetAllResolveService is called!");
                return this._service_author.get_all();
        }
}