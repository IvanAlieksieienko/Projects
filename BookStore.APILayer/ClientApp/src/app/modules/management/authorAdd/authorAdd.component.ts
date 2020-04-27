import { Component, OnInit, Injectable } from '@angular/core';
import { SharedService } from '../../../services/shared.service';
import { Router } from '@angular/router';
import { AuthorService } from '../../../services/author.service';
import { AuthorInputModel } from '../../../inputModels/author.inputModel';
import { CanComponentDeactivate } from '../../CanDeactiveGuard/can-deactive.guard';
import { Observable } from 'rxjs';

@Component({
	selector: 'author-add-component',
	templateUrl: './authorAdd.component.html',
	styleUrls: ['./authorAdd.component.css']
})
@Injectable()
export class AuthorAddComponent implements OnInit, CanComponentDeactivate {

	private _service_author: AuthorService;
	private _new_author: AuthorInputModel = new AuthorInputModel();
	private sended: boolean = false;


	constructor(service_author: AuthorService, private _shared_service: SharedService, private router: Router) {
		this._service_author = service_author;
	}


	ngOnInit() {

	}

	private add_author() {
		var correctness = true;
		if (this._new_author.Name == "") {
			correctness = false;
		}
		if (correctness == true) {
			this.sended = true;
			this._service_author.add_author(this._new_author).subscribe();
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