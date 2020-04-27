import { Routes, RouterModule, Router } from "@angular/router";
import { NgModule } from "@angular/core";
import { HomeComponent } from "./components/home/home.component";
import { SharedService } from "./services/shared.service";
import { AboutComponent } from "./components/about/about.component";
import { ContactComponent } from "./components/contact/contact.component";

const routes: Routes = [
    {path: '', loadChildren: "./modules/category/category.module#CategoryModule"},
    {path: 'login', loadChildren: "./modules/login/login.module#LoginModule"},
    {path: 'category', loadChildren: "./modules/category/category.module#CategoryModule"},
    {path: 'product', loadChildren: "./modules/product/product.module#ProductModule"},
    {path: 'about', component: AboutComponent},
    {path: 'contact', component: ContactComponent},
    {path: '**', loadChildren: "./modules/category/category.module#CategoryModule"}
    
]

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule],
    providers: [SharedService]
})
export class AppRoutingModule { }