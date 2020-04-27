import { Component, OnInit, Injectable } from '@angular/core';
import { BookModel } from '../../../models/book.model';
import { BookService } from '../../../services/book.service';
import { SharedService } from '../../../services/shared.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthorModel } from '../../../models/author.model';
import { AuthorService } from '../../../services/author.service';
import { GenreService } from '../../../services/genre.service';
import { GenreModel } from '../../../models/genre.model';

@Component({
	selector: 'book-get-all-component',
	templateUrl: './bookGetAll.component.html',
	styleUrls: ['./bookGetAll.component.css']
})
@Injectable()
export class BookGetAllComponent implements OnInit {

	private _service_book: BookService;
	private _service_author: AuthorService;
	private _service_genre: GenreService;

	constructor(service_book: BookService, private _shared_service: SharedService, private router: Router, service_author: AuthorService, service_genre: GenreService,
		private route: ActivatedRoute) {
		this._service_book = service_book;
		this._service_author = service_author;
		this._service_genre = service_genre;
	}


	ngOnInit() {
		this.load_data();
	}

	private load_data() {
		this._shared_service._all_books = this.route.snapshot.data['booksList'];
	}

	private show_book(book: BookModel) {
		if (this._shared_service._selected_book == book) {
			this._shared_service._is_show_selected_book = false;
		}
		else {
			this._shared_service._is_show_selected_book = true;
		}
		this._shared_service._selected_book = book;
	}

	private check_author(authorID: number) {
		this.router.navigateByUrl("authors");
		this._service_author.get_by_id(authorID).subscribe((val: AuthorModel) => this._shared_service._selected_author = val);
		this._shared_service._is_show_selected_author = true;
	}

	private check_genre(genreID: number) {
		this.router.navigateByUrl("genres");
		this._service_genre.get_by_id(genreID).subscribe((val: GenreModel) => this._shared_service._selected_genre = val);
		this._shared_service._is_show_selected_genre = true;
	}

	private order_book() {
		this._service_book.order_book(this._shared_service._selected_book.id).subscribe();
	}

	private turn_book_delete_page(book: BookModel) {
		this._service_book.delete(book.id).subscribe();
		this._service_book.get_all().subscribe((val: BookModel[]) => this._shared_service._all_books = val);
	}
}