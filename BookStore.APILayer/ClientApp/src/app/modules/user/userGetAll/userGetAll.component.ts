import { Component, OnInit, Injectable } from '@angular/core';
import { SharedService } from '../../../services/shared.service';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { UserModel } from '../../../models/user.model';
import { BookService } from '../../../services/book.service';
import { BookModel } from '../../../models/book.model';

@Component({
	selector: 'user-get-all-component',
	templateUrl: './userGetAll.component.html',
	styleUrls: ['./userGetAll.component.css']
})
@Injectable()
export class UserGetAllComponent implements OnInit {

	private _service_user: UserService;
	private _service_book: BookService;

	constructor(private _shared_service: SharedService, private router: Router, service_user: UserService, service_book: BookService,
		private route: ActivatedRoute) {
		this._service_user = service_user;
		this._service_book = service_book;
	}


	ngOnInit() {
		this.load_data();
	}

	private load_data() {
		debugger;
		this._shared_service._all_users = this.route.snapshot.data['usersList'];
	}

	private show_user(user: UserModel) {
		if (this._shared_service._selected_user == user) {
			this._shared_service._is_show_selected_user = false;
		}
		else {
			this._shared_service._is_show_selected_user = true;
		}
		this._shared_service._selected_user = user;
	}

	private check_book(bookID: number) {
		this.router.navigateByUrl("books");
		this._service_book.get_by_id(bookID).subscribe((val: BookModel) => this._shared_service._selected_book = val);
		this._shared_service._is_show_selected_book = true;
	}

	private turn_user_delete_page(user: UserModel) {
		this._service_user.delete(user.id).subscribe();
		this._service_user.get_all().subscribe((val: UserModel[]) => this._shared_service._all_users = val);
	}
}