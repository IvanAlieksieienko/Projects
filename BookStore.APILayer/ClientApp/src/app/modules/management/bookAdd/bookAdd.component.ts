import { Component, OnInit, Injectable } from '@angular/core';
import { SharedService } from '../../../services/shared.service';
import { Router } from '@angular/router';
import { BookService } from '../../../services/book.service';
import { BookInputModel } from '../../../inputModels/book.inputModel';
import { CanComponentDeactivate } from '../../CanDeactiveGuard/can-deactive.guard';
import { Observable } from 'rxjs';

@Component({
	selector: 'book-add-component',
	templateUrl: './bookAdd.component.html',
	styleUrls: ['./bookAdd.component.css']
})
@Injectable()
export class BookAddComponent implements OnInit, CanComponentDeactivate {

	private _service_book: BookService;
	private _new_book: BookInputModel = new BookInputModel();
	private _number_of_authors: number;
	private _number_of_genres: number;
	private _authors_of_book: string[] = new Array(10);
	private _genres_of_book: string[] = new Array(10);
	private _release_date: string;
	private sended: boolean = false;

	constructor(service_book: BookService, private _shared_service: SharedService, private router: Router) {
		this._service_book = service_book;
	}


	ngOnInit() {
		this._number_of_authors = 1;
		this._number_of_genres = 1;
		this._release_date = "2005-05-27";
		this._new_book.ReleaseDate = new Date(this._release_date);
	}

	private add_author_fields() {
		this._number_of_authors += 1;
	}

	private sub_author_fields() {
		if (this._number_of_authors > 1) {
			this._number_of_authors -= 1;
		}
	}

	private add_genre_fields() {
		this._number_of_genres += 1;
	}

	private sub_genre_fields() {
		if (this._number_of_genres > 1) {
			this._number_of_genres -= 1;
		}
	}

	private create_array_authors(): any[] {
		return Array(this._number_of_authors);
	}

	private create_array_genres(): any[] {
		return Array(this._number_of_genres);
	}

	private add_book() {
		var correctness = true;
		this._new_book.Authors = new Array(this._number_of_authors);
		for (let i = 0; i < this._number_of_authors; i++) {
			if (this._authors_of_book[i] != "") {
				this._new_book.Authors[i] = this._authors_of_book[i];
			}
			else {
				correctness = false;
			}
		}
		this._new_book.Genres = new Array(this._number_of_genres);
		for (let i = 0; i < this._number_of_genres; i++) {
			if (this._genres_of_book[i] != "") {
				this._new_book.Genres[i] = this._genres_of_book[i];
			}
			else {
				correctness = false;
			}
		}
		if (this._new_book.Title == "" || this._new_book.Price == 0 || this._new_book.ReleaseDate == new Date("01012019")) {
			correctness = false;
		}

		if (correctness == true) {
			this.sended = true;
			this._new_book.ReleaseDate = new Date(this._release_date);
			this._service_book.add_book(this._new_book).subscribe();
		}
		else {

		}
		this.router.navigateByUrl('books');
	}

	public canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
		if ((this._new_book.Title == undefined || this._new_book.Price == undefined || this._new_book.Authors == undefined || this._new_book.Genres == undefined) || !this.sended) {
			return confirm('Your changes are unsaved! Do you like to exit?');
		}
		return true;
	}
}