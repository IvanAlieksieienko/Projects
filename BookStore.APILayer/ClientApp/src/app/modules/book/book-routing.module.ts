import { Routes, RouterModule } from "@angular/router";
import { BookGetAllComponent } from "./bookGetAll/bookGetAll.component";
import { BookGetAllResolveService } from "./bookGetAll/bookGetAllResolveService";
import { BookUpdateComponent } from "./bookUpdate/bookUpdate.component";
import { CanDeactivateGuard } from "../CanDeactiveGuard/can-deactive.guard";
import { NgModule } from "@angular/core";

const routes: Routes = [
        { path: '', component: BookGetAllComponent, resolve: {booksList: BookGetAllResolveService} },
        { path: 'update', component: BookUpdateComponent, canDeactivate: [CanDeactivateGuard] }
];

@NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule]
})
export class BookRoutingModule { }