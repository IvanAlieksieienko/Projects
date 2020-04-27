import { Injectable } from "@angular/core";
import { Resolve, ActivatedRoute, ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { BookModel } from "src/app/models/book.model";
import { BookService } from "src/app/services/book.service";
import { Observable } from "rxjs";

@Injectable()
export class BookGetAllResolveService implements Resolve<BookModel[]> {
        
        constructor(private _service_book: BookService) {

        }

        public resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<any> {
                console.log("BookGetAllResolveService is called!");
                return this._service_book.get_all();
        }
}