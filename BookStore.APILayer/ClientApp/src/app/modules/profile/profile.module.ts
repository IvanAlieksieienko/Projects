import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { ProfileRoutingModule } from "./profile-routing.module";
import { AccountComponent } from "./account/account.component";
import { AccountUpdateComponent } from "./accountUpdate/accountUpdate.component";
import { SharedService } from "src/app/services/shared.service";
import { UserService } from "src/app/services/user.service";
import { AdminModel } from "src/app/models/admin.model";
import { AccountService } from "src/app/services/account.service";
import { BookService } from "src/app/services/book.service";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { HttpConfigInterceptor } from "../Interceptor/httpconfig.interceptor";
import { FormsModule } from "@angular/forms";

@NgModule({
        imports: [
                CommonModule,
                ProfileRoutingModule,
                HttpClientModule,
                FormsModule,
        ],
        declarations: [AccountComponent, AccountUpdateComponent],
        providers: 
        [
                UserService,
                AdminModel,
                AccountService,
                BookService,
                { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true }
        ]
})
export class ProfileModule {}