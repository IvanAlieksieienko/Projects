import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { ProductAddComponent } from "src/app/components/product-add/product-add.component";
import { ProductRouterModule } from "./product-routing.module";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { NgbModule, NgbCarousel } from '@ng-bootstrap/ng-bootstrap';
import { ProductGetByIDComponent } from "src/app/components/product-get-by-id/product-get-by-id.component";
import { ProductUpdateComponent } from "src/app/components/product-update/product-update.component";

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ProductRouterModule,
        FontAwesomeModule,
        NgbModule
    ],
    declarations: [
        ProductAddComponent,
        ProductGetByIDComponent,
        ProductUpdateComponent
    ]
})
export class ProductModule {}