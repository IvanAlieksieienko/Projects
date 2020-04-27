import { Component, OnInit, Injectable } from '@angular/core';
import { SharedService } from '../../../services/shared.service';
import { Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { UserInputModel } from '../../../inputModels/user.inputModel';
import { UserModel } from '../../../models/user.model';
import { Observable } from 'rxjs';
import { CanComponentDeactivate } from '../../CanDeactiveGuard/can-deactive.guard';

@Component({
        selector: 'user-update-component',
        templateUrl: './userUpdate.component.html',
        styleUrls: ['./userUpdate.component.css']
})
@Injectable()
export class UserUpdateComponent implements OnInit, CanComponentDeactivate {

        private _service_user: UserService;
        private _new_user: UserInputModel = new UserInputModel();
        private sended: boolean = false;

        constructor(private _shared_service: SharedService, private router: Router, service_user: UserService) {
                this._service_user = service_user;
        }


        ngOnInit() {

        }

        private update_user(user: UserModel) {
                var correctness = true;
                if (this._new_user.Email == "" || this._new_user.Password == "") {
                        correctness = false;
                        console.log(this._new_user);
                }
                if (correctness == true) {
                        this.sended = true;
                        this._service_user.update_user(this._new_user, this._shared_service._selected_user.id).subscribe();
                        this.router.navigateByUrl('users');
                }
                else {
                }
        }

        public canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
		if (this._new_user.Email == undefined || this._new_user.Password == undefined || !this.sended) {
			return confirm('Your changes are unsaved! Do you like to exit?');
		}
		return true;
	}
}