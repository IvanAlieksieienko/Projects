import { LoginComponent } from "./login.component";
import { Routes, RouterModule } from "@angular/router";
import { CanDeactivateGuard } from "../CanDeactiveGuard/can-deactive.guard";
import { NgModule } from "@angular/core";

const routes: Routes = [
        { path: '', component: LoginComponent, canDeactivate: [CanDeactivateGuard]},
];

@NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule]
})
export class LoginRoutingModule { }