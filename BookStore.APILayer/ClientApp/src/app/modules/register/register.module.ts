import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RegisterComponent } from "./register.component";
import { RegisterRoutingModule } from "./register-routing.module";
import { SharedService } from "src/app/services/shared.service";
import { AccountService } from "src/app/services/account.service";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { HttpConfigInterceptor } from "../Interceptor/httpconfig.interceptor";
import { FormsModule } from "@angular/forms";

@NgModule({
        imports: [
                CommonModule,
                RegisterRoutingModule,
                HttpClientModule,
                FormsModule,
        ],
        declarations: [RegisterComponent],
        providers: 
        [
                AccountService,
                { provide: HTTP_INTERCEPTORS, useClass: HttpConfigInterceptor, multi: true }
        ],
})
export class RegisterModule {}