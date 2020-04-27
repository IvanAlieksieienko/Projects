import { Component } from "@angular/core";
import { CategoryModel } from "src/app/models/category.model";
import { faPaperclip } from '@fortawesome/free-solid-svg-icons';
import { CategoryService } from "src/app/services/category.service";
import { Router } from "@angular/router";
import { CanComponentDeactivate } from "src/app/services/can-deactive.guard";
import { Observable } from "rxjs";

@Component({
    selector: 'category-add',
    templateUrl: './category-add.component.html',
    styleUrls: ['./category-add.component.css']
})
export class CategoryAddComponent implements CanComponentDeactivate {

    icon = faPaperclip;
    public model: CategoryModel = new CategoryModel();
    public _serviceCategory: CategoryService;
    public _isShowFullImage: boolean = false;
    public _fullImagePath: string = "";
    public sended: boolean = false;

    constructor(public serviceCategory: CategoryService, public router: Router) {
        this._serviceCategory = serviceCategory;
    }

    ngOnInit() { 
        this.model.imagePath = "Resources\\Images\\default-tree.png";
    }

    onSelectFile(event) { // called each time file input changes
        if (event.target.files && event.target.files[0]) {
            var fileToUpload = event.target.files[0];
            this._serviceCategory.uploadImage(fileToUpload).subscribe(response => {
                if (response != null && response.dbPath != "") {
                    console.log(response);
                    this.model.imagePath = "";
                    this.model.imagePath += response.dbPath;
                    console.log(this.model.imagePath);
                }
            });
        }
    }

    add() {
        if (this.model.name != "") {
            this.sended = true;
            this._serviceCategory.add(this.model).subscribe(response => {
                this.router.navigate(['category/get', response.id ])
            });
            console.log(this.model.description);
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