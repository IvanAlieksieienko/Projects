import { UserModel } from "src/app/models/user.model";
import { UserService } from "src/app/services/user.service";
import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";

@Injectable()
export class UserGetAllResolveService implements Resolve<UserModel[]> {
        
        constructor(private _service_user: UserService) {

        }

        public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
                console.log("UserGetAllResolveService is called!");
                return this._service_user.get_all();
        }
}