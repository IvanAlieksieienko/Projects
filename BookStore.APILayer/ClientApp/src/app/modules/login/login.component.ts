import { Component, OnInit, Injectable } from '@angular/core';
import { LoginModel } from '../../models/login.model';
import { AccountService } from '../../services/account.service';
import { SharedService } from '../../services/shared.service';
import { Router } from '@angular/router';
import { CanComponentDeactivate } from '../CanDeactiveGuard/can-deactive.guard';
import { Observable } from 'rxjs';

@Component({
	selector: 'login-component',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
@Injectable()
export class LoginComponent implements OnInit, CanComponentDeactivate {

	private _login_model: LoginModel = new LoginModel();
	private _service_account: AccountService;

	constructor(service_account: AccountService, private _shared_service: SharedService, private router: Router) {
		this._service_account = service_account;
	}


	ngOnInit() {
		this._login_model.email = "";
		this._login_model.password = "";
	}

	private login() {
		if (this._login_model.email == "") {

		}
		else if (this._login_model.password == "") {

		}
		else if (this._login_model.email != "" && this._login_model.password != "") {
			this._service_account.login_with_model(this._login_model).subscribe((val: LoginModel) => {
				if (val != null) {
					if (val.role != "unauthorized") {
						this._shared_service._authorized_role = val.role;
						this._shared_service._user_email = val.email;
						this._shared_service._is_authorized = true;
						if (val.role == "admin") {
							this._shared_service._is_admin = true;
						}
						if (val.role == "user") {
							this._shared_service._is_admin = false;
						}
					}
				}
			});
		}
		this.router.navigateByUrl('books');
	}

	public canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
		if (this._login_model.email == "" || this._login_model.password == "") {
			return confirm('Your changes are unsaved! Do you like to exit?');
		}
		return true;
	}
}