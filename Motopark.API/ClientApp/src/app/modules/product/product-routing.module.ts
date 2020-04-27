import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { ProductGetAllComponent } from "src/app/components/product-get-all/product-get-all.component";
import { ProductGetByIdComponent } from "src/app/components/product-get-by-id/product-get-by-id.component";
import { HomeComponent } from "src/app/components/home/home.component";
import { NotFoundComponent } from "src/app/components/not-found/not-found.component";

const routes: Routes = [    
    { path: '', component: NotFoundComponent},
    { path: 'get/:id', component: ProductGetByIdComponent}
]

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ProductRouterModule {}