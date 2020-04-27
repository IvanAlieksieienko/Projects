import { Component, OnInit, Injectable } from '@angular/core';
import { SharedService } from '../../../services/shared.service';
import { Router } from '@angular/router';
import { AuthorService } from '../../../services/author.service';
import { AuthorInputModel } from '../../../inputModels/author.inputModel';
import { Observable } from 'rxjs';
import { CanComponentDeactivate } from '../../CanDeactiveGuard/can-deactive.guard';

@Component({
	selector: 'author-update-component',
	templateUrl: './authorUpdate.component.html',
	styleUrls: ['./authorUpdate.component.css']
})
@Injectable()
export class AuthorUpdateComponent implements OnInit, CanComponentDeactivate {

	private _service_author: AuthorService;
	private _new_author: AuthorInputModel = new AuthorInputModel();
	private sended: boolean = false;


	constructor(service_book: AuthorService, private _shared_service: SharedService, private router: Router) {
		this._service_author = service_book;
	}


	ngOnInit() {
		
	}

	private update_author() {
		var correctness = true;
		if (this._new_author.Name == "") {
			correctness = false;
		}
		if (correctness == true) {
			this.sended = true;
			this._service_author.update_author(this._new_author, this._shared_service._selected_author.id).subscribe();
			this.router.navigateByUrl('authors');
		}
		else {

		}
	}

	public canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
		if (this._new_author.Name == undefined || !this.sended) {
			return confirm('Your changes are unsaved! Do you like to exit?');
		}
		return true;
	}
}