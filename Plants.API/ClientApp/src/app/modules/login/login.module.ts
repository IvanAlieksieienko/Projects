import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { LoginRouterModule } from "./login-routing.module";
import { LoginComponent } from "src/app/components/login/login.component";
import { FormsModule } from "@angular/forms";

@NgModule({
    imports: [
        CommonModule,
        LoginRouterModule,
        FormsModule
    ],
    declarations: [LoginComponent],
    providers: [],
})
export class LoginModule {}