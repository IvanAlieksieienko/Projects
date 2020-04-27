import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { BookRoutingModule } from "./book-routing.module";
import { BookGetAllComponent } from "./bookGetAll/bookGetAll.component";
import { BookUpdateComponent } from "./bookUpdate/bookUpdate.component";
import { SharedService } from "src/app/services/shared.service";
import { BookGetAllResolveService } from "./bookGetAll/bookGetAllResolveService";
import { AuthorService } from "src/app/services/author.service";
import { GenreService } from "src/app/services/genre.service";
import { BookService } from "src/app/services/book.service";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { HttpConfigInterceptor } from "../Interceptor/httpconfig.interceptor";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";

@NgModule({
        imports: [
                CommonModule,
                BookRoutingModule,
                HttpClientModule,
                FormsModule,
        ],
        declarations: [BookGetAllComponent, BookUpdateComponent],
        providers:
                [
                        BookGetAllResolveService,
                        AuthorService,
                        GenreService,
                        BookService,
                        { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true }
                ]
})
export class BookModule { }