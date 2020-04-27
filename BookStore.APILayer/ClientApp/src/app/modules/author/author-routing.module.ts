import { Routes, RouterModule } from "@angular/router";
import { AuthorGetAllComponent } from "./authorGetAll/authorGetAll.component";
import { AuthorGetAllResolveService } from "./authorGetAll/authorGetAllResolveService";
import { AuthorUpdateComponent } from "./authorUpdate/authorUpdate.component";
import { CanDeactivateGuard } from "../CanDeactiveGuard/can-deactive.guard";
import { NgModule } from "@angular/core";

const routes: Routes = [
        { path: '', component: AuthorGetAllComponent, resolve: {authorsList: AuthorGetAllResolveService}},
        { path: 'update', component: AuthorUpdateComponent, canDeactivate: [CanDeactivateGuard] }
];

@NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule],
})
export class AuthorRoutingModule { }