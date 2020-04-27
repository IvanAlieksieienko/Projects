import { Component, OnInit, Injectable } from '@angular/core';
import { SharedService } from '../../../services/shared.service';
import { GenreService } from '../../../services/genre.service';
import { GenreModel } from '../../../models/genre.model';
import { BookModel } from '../../../models/book.model';
import { Router, ActivatedRoute } from '@angular/router';
import { BookService } from '../../../services/book.service';

@Component({
	selector: 'genre-get-all-component',
	templateUrl: './genreGetAll.component.html',
	styleUrls: ['./genreGetAll.component.css']
})
@Injectable()
export class GenreGetAllComponent implements OnInit {

	private _service_genre: GenreService;
	private _service_book: BookService;


	constructor(service_genre: GenreService, private _shared_service: SharedService, private router: Router, service_book: BookService,
		private route: ActivatedRoute) {
		this._service_genre = service_genre;
		this._service_book = service_book;
	}


	ngOnInit() {
		this.load_data();
	}

	private load_data() {
		this._shared_service._all_genres = this.route.snapshot.data['genresList'];
	}

	private show_genre(genre: GenreModel) {
		if (this._shared_service._selected_genre == genre) {
			this._shared_service._is_show_selected_genre = false;
		}
		else {
			this._shared_service._is_show_selected_genre = true;
		}
		this._shared_service._selected_genre = genre;
	}

	private check_book(bookID: number) {
		this.router.navigateByUrl("books");
		this._service_book.get_by_id(bookID).subscribe((val: BookModel) => this._shared_service._selected_book = val);
		this._shared_service._is_show_selected_book = true;
	}

	private turn_genre_delete_page(genre: GenreModel) {
		this._service_genre.delete(genre.id).subscribe();
		this.load_data();
		this.router.navigateByUrl('genres');
	}
}