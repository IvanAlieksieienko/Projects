import { Component, OnInit, Injectable } from '@angular/core';
import { SharedService } from '../../../services/shared.service';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { AdminService } from '../../../services/admin.service';
import { UserModel } from '../../../models/user.model';
import { AdminModel } from '../../../models/admin.model';
import { AccountService } from '../../../services/account.service';
import { BookModel } from '../../../models/book.model';
import { BookService } from '../../../services/book.service';

@Component({
	selector: 'account-component',
	templateUrl: './account.component.html',
	styleUrls: ['./account.component.css']
})
@Injectable()
export class AccountComponent implements OnInit {

	private _service_user: UserService;
	private _service_admin: AdminService;
	private _service_account: AccountService;
	private _service_book: BookService;


	constructor(private _shared_service: SharedService, private router: Router, service_user: UserService, service_admin: AdminService, service_account: AccountService, service_book: BookService) {
		this._service_user = service_user;
		this._service_admin = service_admin;
		this._service_account = service_account;
		this._service_book = service_book;
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

	private delete_user_profile() {
		if (this._shared_service._authorized_role == "user") {
			this._service_user.delete(this._shared_service._user_id).subscribe();
		}
		if (this._shared_service._authorized_role == "admin") {
			this._service_admin.delete(this._shared_service._user_id).subscribe();
		}
		this.logout();
		this.router.navigateByUrl("books")
	}

	private logout() {
		this._service_account.logout().subscribe();
		this._shared_service._is_authorized = false;
		this._shared_service._user_email = "";
		this._shared_service._user_id = 0;
		this._shared_service._authorized_role = "";
		this._shared_service._is_admin = false;
	}

	private check_book(bookID: number) {
		this.router.navigateByUrl("books");
		this._service_book.get_by_id(bookID).subscribe((val: BookModel) => this._shared_service._selected_book = val);
		this._shared_service._is_show_selected_book = true;
	}
}