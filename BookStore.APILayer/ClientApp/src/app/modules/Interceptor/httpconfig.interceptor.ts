import {Injectable, Injector} from '@angular/core';
import {HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest} from '@angular/common/http'
import {Observable} from 'rxjs';
import { tap } from "rxjs/internal/operators";

@Injectable()
export class HttpConfigInterceptor implements HttpInterceptor {

        constructor(private _injector: Injector) { }

        public intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
                const logFormat = 'background: maroon; color: white';

                return next.handle(req).pipe(tap(event => {
                        }, exception => {
                                if (exception instanceof HttpErrorResponse) {
                                        switch (exception.status) {

                                                case HttpError.BadRequest:
                                                        // alert('Bad Request 400');
                                                        break;

                                                case HttpError.Unauthorized:
                                                        //alert('Unauthorized 401');
                                                        break;

                                                case HttpError.NotFound:
                                                        //alert('Not Found 404');
                                                        break;

                                                case HttpError.TimeOut:
                                                        //alert('TimeOut 408');
                                                        break;

                                                case HttpError.Forbidden:
                                                        //alert('Forbidden 403');
                                                        break;

                                                case HttpError.InternalServerError:
                                                       // alert('Bad 500');
                                                        break;
                                        }
                                }
                        }));
        }
}

export class HttpError{
        static BadRequest = 400;
        static Unauthorized = 401;
        static Forbidden = 403;
        static NotFound = 404;
        static TimeOut = 408;
        static Conflict = 409;
        static InternalServerError = 500;
    }