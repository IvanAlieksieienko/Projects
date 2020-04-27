import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { AuthorRoutingModule } from "../author/author-routing.module";
import { BasketGetAllComponent } from "./basketGetAll.component";
import { SharedService } from "src/app/services/shared.service";
import { BookService } from "src/app/services/book.service";
import { AccountService } from "src/app/services/account.service";
import { UserService } from "src/app/services/user.service";
import { PayService } from "src/app/services/pay.service";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { HttpConfigInterceptor } from "../Interceptor/httpconfig.interceptor";
import { FormsModule } from "@angular/forms";

@NgModule({
        imports: [
                CommonModule,
                AuthorRoutingModule,
                HttpClientModule,
                FormsModule,
        ],
        declarations: [BasketGetAllComponent],
        providers:
        [
                BasketGetAllComponent,
                BookService,
                AccountService,
                UserService,
                PayService,
                { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true }
        ]
})
export class BasketGetAllModule {}