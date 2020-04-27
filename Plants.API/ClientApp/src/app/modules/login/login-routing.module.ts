import { LoginComponent } from "src/app/components/login/login.component";
import { NgModule } from "@angular/core";
import { Router, RouterModule, Routes } from "@angular/router";
import { CanDeactivateGuard } from "src/app/services/can-deactive.guard";

const routes: Routes = [
    {path: '', component: LoginComponent, canDeactivate: [CanDeactivateGuard]},
    {path: '**', component: LoginComponent, canDeactivate: [CanDeactivateGuard] }
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LoginRouterModule {}