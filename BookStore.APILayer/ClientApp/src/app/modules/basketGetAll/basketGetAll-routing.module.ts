import { Routes, RouterModule } from "@angular/router";
import { BasketGetAllComponent } from "./basketGetAll.component";
import { NgModule } from "@angular/core";

const routes: Routes = [
        { path: '', component: BasketGetAllComponent}
];

@NgModule({
        imports: [RouterModule.forChild(routes)],
        exports: [RouterModule]
})
export class BasketGetAllRoutingModule {}