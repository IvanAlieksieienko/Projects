import { Component, OnInit, Injectable } from '@angular/core';
import { AccountService } from '../../services/account.service';
import { UserModel } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { AdminModel } from '../../models/admin.model';
import { SharedService } from '../../services/shared.service';
import { Router } from '@angular/router';

@Component({
	selector: 'head-bar-component',
	templateUrl: './headBar.component.html',
	styleUrls: ['./headBar.component.css']
})
@Injectable()
export class HeadBarComponent implements OnInit {

	private _service_account: AccountService;
	private _service_user: UserService;


	constructor(service_account: AccountService, service_user: UserService, private _shared_service: SharedService, private router: Router) {
		this._service_account = service_account;
		this._service_user = service_user;
	}


	ngOnInit() {
		this.check_role();
		this.get_current_user();
	}

	private check_role() {
		this._service_account.check_role().subscribe((val: string) => {
			if (val === "admin" || val === "user") {
				this._shared_service._is_authorized = true;
				this._shared_service._authorized_role = val;
				if (val == "admin") {
					this._shared_service._is_admin = true;
				}
				if (val == "user") {
					this._shared_service._is_admin = false;
				}
				this.get_current_user();
			}
			else {
				this._shared_service._is_authorized = false;
			}
		});
	}

	private get_current_user() {
		this._service_account.get_current_user_info().subscribe((val: AdminModel) => {
			if (val != null) {
				this._shared_service._user_email = val.email;
				this._shared_service._user_id = val.id;
				this._shared_service._user_password = val.password;
				this._service_user.get_by_id(val.id).subscribe((value: UserModel) => this._shared_service._current_user = value);
			}
		});
	}

	private logout() {
		this._service_account.logout().subscribe();
		this._shared_service._is_authorized = false;
		this._shared_service._user_email = "";
		this._shared_service._user_id = 0;
		this._shared_service._authorized_role = "";
		this._shared_service._is_admin = false;
		this.router.navigateByUrl('books');
	}
}