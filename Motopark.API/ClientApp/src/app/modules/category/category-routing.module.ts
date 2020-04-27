import { Routes, RouterModule } from "@angular/router";
import { CategoryGetAllComponent } from "src/app/components/category-get-all/category-get-all.component";
import { CategoryGetByIdComponent } from "src/app/components/category-get-by-id/category-get-by-id.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
    { path: '', component: CategoryGetAllComponent},
    { path: 'get/:id', component: CategoryGetByIdComponent},
    { path: '**', component: CategoryGetAllComponent}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CategoryRouterModule {}