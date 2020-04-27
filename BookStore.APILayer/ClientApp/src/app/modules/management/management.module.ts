import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ManagementComponent } from "./management/management.component";
import { AuthorAddComponent } from "./authorAdd/authorAdd.component";
import { GenreAddComponent } from "./genreAdd/genreAdd.component";
import { BookAddComponent } from "./bookAdd/bookAdd.component";
import { ManagementRoutingModule } from "./management-routing.module";
import { SharedService } from "src/app/services/shared.service";
import { AuthorService } from "src/app/services/author.service";
import { BookService } from "src/app/services/book.service";
import { GenreService } from "src/app/services/genre.service";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { HttpConfigInterceptor } from "../Interceptor/httpconfig.interceptor";
import { FormsModule } from "@angular/forms";

@NgModule({
        imports: [
                CommonModule,
                ManagementRoutingModule,
                HttpClientModule,
                FormsModule,
        ],
        declarations: [ManagementComponent, AuthorAddComponent, GenreAddComponent, BookAddComponent],
        providers:
                [
                        AuthorService,
                        BookService,
                        GenreService,
                        { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true }
                ],
})
export class ManagementModule {}