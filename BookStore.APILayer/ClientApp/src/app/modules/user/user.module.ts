import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { UserRoutingModule } from "./user-routing.module";
import { UserGetAllComponent } from "./userGetAll/userGetAll.component";
import { UserUpdateComponent } from "./userUpdate/userUpdate.component";
import { SharedService } from "src/app/services/shared.service";
import { UserGetAllResolveService } from "./userGetAll/userGetAllResolveService";
import { UserService } from "src/app/services/user.service";
import { BookService } from "src/app/services/book.service";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { HttpConfigInterceptor } from "../Interceptor/httpconfig.interceptor";
import { FormsModule } from "@angular/forms";

@NgModule({
        imports: [
                CommonModule,
                UserRoutingModule,
                HttpClientModule,
                FormsModule,
        ],
        declarations: [UserGetAllComponent, UserUpdateComponent],
        providers: 
        [
                UserGetAllResolveService,
                UserService,
                BookService,
                { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true }
        ],
})
export class UserModule {}