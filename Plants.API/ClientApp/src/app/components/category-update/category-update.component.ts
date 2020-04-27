import { Component } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { Guid } from "guid-typescript";
import { CategoryModel } from "src/app/models/category.model";
import { CategoryService } from "src/app/services/category.service";
import { ProductService } from "src/app/services/product.service";
import { SharedService } from "src/app/services/shared.service";
import { switchMap } from "rxjs/operators";
import { faPaperclip } from '@fortawesome/free-solid-svg-icons';
import { CanComponentDeactivate } from "src/app/services/can-deactive.guard";
import { Observable } from "rxjs";

@Component({
    selector: 'category-update',
    templateUrl: 'category-update.component.html',
    styleUrls: ['category-update.component.css']
})
export class CategoryUpdateComponent implements CanComponentDeactivate {

    icon = faPaperclip;
    public categoryID: Guid;
    public category: CategoryModel;
    public _isShowFullImage: boolean = false;
    public _fullImagePath: string = "";
    public _serviceCategory: CategoryService;
    public _serviceProduct: ProductService;
    public sended: boolean = false;

    constructor(public activateRoute: ActivatedRoute, serviceCategory: CategoryService, serviceProduct: ProductService, public _sharedService: SharedService, public router: Router) {
        this._serviceCategory = serviceCategory;
        this._serviceProduct = serviceProduct;
    }

    ngOnInit() {
        this.activateRoute.paramMap.pipe(
            switchMap(params => params.getAll('id'))
        ).subscribe(response => {
            this.categoryID = Guid.parse(response);
            console.log(this.categoryID);
            this._serviceCategory.getByID(this.categoryID).subscribe(response => {
                this.category = response;
                console.log(this.category);
                if (this.category.imagePath == "") {
                    this.category.imagePath = "Resources\\Images\\default-tree.png";
                }
            })
        });
    }

    onSelectFile(event) { // called each time file input changes
        if (event.target.files && event.target.files[0]) {
            var fileToUpload = event.target.files[0];
            this._serviceCategory.uploadImage(fileToUpload).subscribe(response => {
                if (response != null && response.dbPath != "") {
                    console.log(response);
                    this.category.imagePath = "";
                    this.category.imagePath += response.dbPath;
                }
            });
        }
    }

    update() {
        if (this.category.name != "") {
            this.sended = true;
            this._serviceCategory.update(this.category).subscribe(response => {
                
                this.router.navigate(['category/get', response.id]);
            });
            console.log(this.category.description);
        }
        else {
            alert("Введите имя!");
        }
    }

    showImage(path: string) {
        this._isShowFullImage = true;
        this._fullImagePath = path;
    }

    closeImageView() {
        this._isShowFullImage = false;
        this._fullImagePath = "";
    }

    public canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
        if (!this.sended) {
            return confirm('Не сохраненные изменения! Уйти?');
        }
        return true;
    }
}