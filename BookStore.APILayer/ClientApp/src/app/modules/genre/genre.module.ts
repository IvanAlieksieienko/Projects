import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { GenreRoutingModule } from "./genre-routing.module";
import { GenreGetAllComponent } from "./genreGetAll/genreGetAll.component";
import { GenreUpdateComponent } from "./genreUpdate/genreUpdate.component";
import { SharedService } from "src/app/services/shared.service";
import { GenreGetAllResolveService } from "./genreGetAll/genreGetAllResolveService";
import { GenreService } from "src/app/services/genre.service";
import { BookService } from "src/app/services/book.service";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { HttpConfigInterceptor } from "../Interceptor/httpconfig.interceptor";
import { FormsModule } from "@angular/forms";

@NgModule({
        imports: [
                CommonModule,
                GenreRoutingModule,
                HttpClientModule,
                FormsModule
        ],
        declarations: [GenreGetAllComponent, GenreUpdateComponent],
        providers:
                [
                        GenreGetAllResolveService,
                        GenreService,
                        BookService,
                        { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true }
                ],
})
export class GenreModule { }