import { ManagementComponent } from "./management/management.component";
import { Routes, RouterModule } from "@angular/router";
import { BookAddComponent } from "./bookAdd/bookAdd.component";
import { CanDeactivateGuard } from "../CanDeactiveGuard/can-deactive.guard";
import { AuthorAddComponent } from "./authorAdd/authorAdd.component";
import { GenreAddComponent } from "./genreAdd/genreAdd.component";
import { NgModule } from "@angular/core";


const routes: Routes = [
        { path: '', component: ManagementComponent },
        { path: 'book/add', component: BookAddComponent, canDeactivate: [CanDeactivateGuard] },
        { path: 'author/add', component: AuthorAddComponent, canDeactivate: [CanDeactivateGuard] },
        { path: 'genre/add', component: GenreAddComponent, canDeactivate: [CanDeactivateGuard] }
];

@NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule]
})
export class ManagementRoutingModule { }