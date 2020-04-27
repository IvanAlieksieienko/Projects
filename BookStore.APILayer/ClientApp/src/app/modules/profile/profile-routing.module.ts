import { Routes, RouterModule } from "@angular/router";
import { CanDeactivateGuard } from "../CanDeactiveGuard/can-deactive.guard";
import { NgModule } from "@angular/core";
import { AccountComponent } from "./account/account.component";
import { AccountUpdateComponent } from "./accountUpdate/accountUpdate.component";

const routes: Routes = [
        { path: '', component: AccountComponent},
        { path: 'update', component: AccountUpdateComponent, canDeactivate: [CanDeactivateGuard] }
];

@NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule]
})
export class ProfileRoutingModule { }