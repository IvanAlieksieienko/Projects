import { Routes, RouterModule } from "@angular/router";
import { GenreGetAllComponent } from "./genreGetAll/genreGetAll.component";
import { GenreGetAllResolveService } from "./genreGetAll/genreGetAllResolveService";
import { GenreUpdateComponent } from "./genreUpdate/genreUpdate.component";
import { CanDeactivateGuard } from "../CanDeactiveGuard/can-deactive.guard";
import { NgModule } from "@angular/core";

const routes: Routes = [
        { path: '', component: GenreGetAllComponent, resolve: { genresList: GenreGetAllResolveService } },
        { path: 'update', component: GenreUpdateComponent, canDeactivate: [CanDeactivateGuard] }
];

@NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule]
})
export class GenreRoutingModule { }