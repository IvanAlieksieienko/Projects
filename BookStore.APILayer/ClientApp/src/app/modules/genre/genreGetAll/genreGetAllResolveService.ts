import { Injectable } from "@angular/core";
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { GenreModel } from "src/app/models/genre.model";
import { GenreService } from "src/app/services/genre.service";
import { Observable } from "rxjs";

@Injectable()
export class GenreGetAllResolveService implements Resolve<GenreModel[]> {

        constructor(private _service_genre: GenreService) {

        }

        public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
                console.log("genreGetAllResolveService is called!");
                return this._service_genre.get_all();
        }
}