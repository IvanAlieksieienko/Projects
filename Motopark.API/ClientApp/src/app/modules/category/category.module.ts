import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { CategoryRouterModule } from "./category-routing.module";
import { CategoryGetByIdComponent } from "src/app/components/category-get-by-id/category-get-by-id.component";
import { CategoryGetAllComponent } from "src/app/components/category-get-all/category-get-all.component";
import { ProductGetAllComponent } from "src/app/components/product-get-all/product-get-all.component";
import { NgbPaginationModule, NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { NgxPaginationModule } from "ngx-pagination";

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        FontAwesomeModule,
        CategoryRouterModule,
        NgbModule,
        NgxPaginationModule 
    ],
    declarations: [
        CategoryGetByIdComponent,
        CategoryGetAllComponent,
        ProductGetAllComponent
    ]
})
export class CategoryModule {}