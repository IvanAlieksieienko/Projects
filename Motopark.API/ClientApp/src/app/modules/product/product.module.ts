import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { ProductRouterModule } from "./product-routing.module";
import { ProductGetByIdComponent } from "src/app/components/product-get-by-id/product-get-by-id.component";
import { ProductGetAllComponent } from "src/app/components/product-get-all/product-get-all.component";
import { NotFoundComponent } from "src/app/components/not-found/not-found.component";

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        FontAwesomeModule,
        ProductRouterModule
    ],
    declarations: [
        ProductGetByIdComponent,
        NotFoundComponent
    ]
})
export class ProductModule {}