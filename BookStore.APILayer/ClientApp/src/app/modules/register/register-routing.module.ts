import { Routes, RouterModule } from "@angular/router";
import { CanDeactivateGuard } from "../CanDeactiveGuard/can-deactive.guard";
import { NgModule } from "@angular/core";
import { RegisterComponent } from "./register.component";

const routes: Routes = [
        { path: '', component: RegisterComponent, canDeactivate: [CanDeactivateGuard]},
];

@NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule]
})
export class RegisterRoutingModule { }