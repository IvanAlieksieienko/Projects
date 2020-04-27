import { Routes, RouterModule } from "@angular/router";
import { ProductAddComponent } from "src/app/components/product-add/product-add.component";
import { NgModule } from "@angular/core";
import { ProductGetByIDComponent } from "src/app/components/product-get-by-id/product-get-by-id.component";
import { ProductUpdateComponent } from "src/app/components/product-update/product-update.component";
import { CanDeactivateGuard } from "src/app/services/can-deactive.guard";

const routes: Routes = [
    { path: 'create/:id', component: ProductAddComponent, canDeactivate: [CanDeactivateGuard]},
    { path: 'get/:id', component: ProductGetByIDComponent},
    { path: 'update/:id', component: ProductUpdateComponent, canDeactivate: [CanDeactivateGuard]}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProductRouterModule {}