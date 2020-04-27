import { Component, OnInit, Injectable } from '@angular/core';
import { SharedService } from '../../services/shared.service';
import { Router } from '@angular/router';
import { BookModel } from '../../models/book.model';
import { BookService } from '../../services/book.service';
import { AdminModel } from '../../models/admin.model';
import { AccountService } from '../../services/account.service';
import { UserService } from '../../services/user.service';
import { UserModel } from '../../models/user.model';
import { PayInputModel } from '../../inputModels/pay.inputModel';
import { PayService } from '../../services/pay.service';

declare var Stripe;

@Component({
	selector: 'basket-get-all-component',
	templateUrl: './basketGetAll.component.html',
	styleUrls: ['./basketGetAll.component.css']
})
@Injectable()
export class BasketGetAllComponent implements OnInit {

	private stripe;
	private _current_user_basket_book: BookModel;
	private _service_book: BookService;
	private _service_account: AccountService;
	private _service_user: UserService;
	private _service_pay: PayService;

	_is_show_basket_book: boolean = false;

	constructor(private _shared_service: SharedService, private router: Router, service_book: BookService,
		service_account: AccountService, service_user: UserService, service_pay: PayService) {
		this._service_book = service_book;
		this._service_user = service_user;
		this._service_account = service_account;
		this._service_pay = service_pay;
	}


	ngOnInit() {
		this.get_current_user();
	}

	private get_current_user() {
		this._service_account.get_current_user_info().subscribe((val: AdminModel) => {
			if (val != null) {
				this._shared_service._user_email = val.email;
				this._shared_service._user_id = val.id;
				this._shared_service._user_password = val.password;
				this._service_user.get_by_id(val.id).subscribe((value: UserModel) => this._shared_service._current_user = value);
			}
		})
	}

	private check_basket_book(bookID: number) {
		this._service_book.get_by_id(bookID).subscribe((val: BookModel) => this._current_user_basket_book = val);
		this._is_show_basket_book = !this._is_show_basket_book;
	}

	private pay() {
		var fields = new PayInputModel();
		fields.Email = this._shared_service._user_email;
		fields.Title = this._current_user_basket_book.title;
		fields.Price = this._current_user_basket_book.price;
		fields.BookID = this._current_user_basket_book.id;
		var flag = true;
		this._service_pay.pay(fields).subscribe((val: string) => {
			console.log(val);
			this.stripe = Stripe("pk_test_HW1BdSa54NYVuasN4ojAkRfx00JQvskVES");
			this.stripe.redirectToCheckout({
				sessionId: val
			}).then(function (result) {
				// If `redirectToCheckout` fails due to a browser or network
				// error, display the localized error message to your customer
				// using `result.error.message`.
				console.log(result.error.message);
			});
		});
	}

	private delete_ordered_book(book: BookModel) {
		this._service_book.delete_ordered_book(book.id).subscribe();
	}
}