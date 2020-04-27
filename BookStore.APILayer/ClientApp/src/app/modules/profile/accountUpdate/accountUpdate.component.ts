import { Component, OnInit, Injectable } from '@angular/core';
import { SharedService } from '../../../services/shared.service';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { AdminService } from '../../../services/admin.service';
import { UserInputModel } from '../../../inputModels/user.inputModel';
import { AdminInputModel } from '../../../inputModels/admin.inputModel';
import { AccountService } from '../../../services/account.service';
import { CanComponentDeactivate } from '../../CanDeactiveGuard/can-deactive.guard';
import { Observable } from 'rxjs';

@Component({
        selector: 'account-update-component',
        templateUrl: './accountUpdate.component.html',
        styleUrls: ['./accountUpdate.component.css']
})
@Injectable()
export class AccountUpdateComponent implements OnInit, CanComponentDeactivate {

        private _service_user: UserService;
        private _service_admin: AdminService;
        private _new_user: UserInputModel = new UserInputModel();
        private _new_admin: AdminInputModel = new AdminInputModel();
        private _service_account: AccountService;
        private sended: boolean = false;

        constructor(private _shared_service: SharedService, private router: Router, service_user: UserService, service_admin: AdminService, service_account: AccountService) {
                this._service_user = service_user;
                this._service_admin = service_admin;
                this._service_account = service_account;
        }


        ngOnInit() {
                this._new_admin.Email = "";
                this._new_admin.Password = "";
                this._new_user.Password = "";
                this._new_user.Email = "";
        }

        private update_user() {
                var correctness = true;
                if (this._new_user.Email == "" || this._new_user.Password == "") {
                        correctness = false;
                }
                if (correctness == true) {
                        this.sended = true;
                        this._service_user.update_user(this._new_user, this._shared_service._user_id).subscribe();
                }
                this.logout();
                this.router.navigateByUrl("books");
        }

        private update_admin() {
                var correctness = true;
                if (this._new_admin.Email == "" || this._new_admin.Password == "") {
                        this.sended = true;
                        correctness = false;
                }
                if (correctness == true) {
                        this._service_admin.update_admin(this._new_admin, this._shared_service._user_id).subscribe();
                }
                this.logout();
                this.router.navigateByUrl("books");
        }

        private logout() {
                this._service_account.logout().subscribe();
                this._shared_service._is_authorized = false;
                this._shared_service._user_email = "";
                this._shared_service._user_id = 0;
                this._shared_service._authorized_role = "";
                this._shared_service._is_admin = false;
        }

        public canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
		if (!this.sended) {
			return confirm('Your changes are unsaved! Do you like to exit?');
		}
		return true;
	}
}