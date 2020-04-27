import { Component, OnInit, Injectable } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { SharedService } from '../../services/shared.service';
import { Router } from '@angular/router';
import { RegisterModel } from '../../models/register.model';
import { CanComponentDeactivate } from '../CanDeactiveGuard/can-deactive.guard';
import { Observable } from 'rxjs';

@Component({
	selector: 'register-component',
	templateUrl: './register.component.html',
	styleUrls: ['./register.component.css']
})
@Injectable()
export class RegisterComponent implements OnInit, CanComponentDeactivate {

	private _register_model: RegisterModel = new RegisterModel();
	private _service_account: AccountService;

	constructor(service_account: AccountService, private _shared_service: SharedService, private router: Router) {
		this._service_account = service_account;
	}


	ngOnInit() {
		this._register_model.email = "";
		this._register_model.password = "";
	}

	private register() {
		if (this._register_model.email == "") {

		}
		else if (this._register_model.password == "") {

		}
		else if (this._register_model.email != "" && this._register_model.password != "") {
			this._service_account.register_with_model(this._register_model).subscribe(data => this.router.navigateByUrl("/"));
		}
		this.router.navigateByUrl('books');
	}

	public canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
		if (this._register_model.email == "" || this._register_model.password == "") {
			return confirm('Your changes are unsaved! Do you like to exit?');
		}
		return true;
	}
}