import { Routes, RouterModule } from "@angular/router";
import { CanDeactivateGuard } from "../CanDeactiveGuard/can-deactive.guard";
import { NgModule } from "@angular/core";
import { UserGetAllComponent } from "./userGetAll/userGetAll.component";
import { UserGetAllResolveService } from "./userGetAll/userGetAllResolveService";
import { UserUpdateComponent } from "./userUpdate/userUpdate.component";

const routes: Routes = [
        { path: '', component: UserGetAllComponent, resolve: {usersList: UserGetAllResolveService} },
        { path: 'update', component: UserUpdateComponent, canDeactivate: [CanDeactivateGuard] }
];

@NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule]
})
export class UserRoutingModule { }