import { Routes, RouterModule } from "@angular/router";
import { NgModule } from "@angular/core";
import { HomeComponent } from "./components/home/home.component";
import { AboutComponent } from "./components/about/about.component";
import { BasketComponent } from "./components/basket/basket.component";
import { ContactComponent } from "./components/contact/contact.component";
import { OrderComponent } from "./components/order/order.component";

const routes: Routes = [
    { path: "", component: HomeComponent},
    { path: "product", loadChildren: "./modules/product/product.module#ProductModule"},
    { path: "category", loadChildren: "./modules/category/category.module#CategoryModule"},
    { path: "about", component: AboutComponent},
    { path: "basket", component: BasketComponent},
    { path: "contact", component: ContactComponent},
    { path: "order/:id", component: OrderComponent},
    { path: "**", component: HomeComponent}
]

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: []
})
export class AppRoutingModule {}