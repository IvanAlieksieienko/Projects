import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { AuthorRoutingModule } from "./author-routing.module";
import { AuthorGetAllComponent } from "./authorGetAll/authorGetAll.component";
import { AuthorUpdateComponent } from "./authorUpdate/authorUpdate.component";
import { SharedService } from "src/app/services/shared.service";
import { AuthorGetAllResolveService } from "./authorGetAll/authorGetAllResolveService";
import { AuthorService } from "src/app/services/author.service";
import { BookService } from "src/app/services/book.service";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { HttpConfigInterceptor } from "../Interceptor/httpconfig.interceptor";
import { FormsModule } from "@angular/forms";

@NgModule({
        imports: [
                CommonModule,
                AuthorRoutingModule,
                HttpClientModule,
                FormsModule
        ],
        declarations: [AuthorGetAllComponent, AuthorUpdateComponent],
        providers: 
        [
                AuthorGetAllResolveService,
                AuthorService,
                BookService,
                { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true }
        ]
})
export class AuthorModule {}