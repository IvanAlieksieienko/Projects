import { Component, OnInit, Injectable } from '@angular/core';
import { SharedService } from '../../../services/shared.service';
import { AuthorService } from '../../../services/author.service';
import { AuthorModel } from '../../../models/author.model';
import { BookModel } from '../../../models/book.model';
import { Router, ActivatedRoute } from '@angular/router';
import { BookService } from '../../../services/book.service';

@Component({
	selector: 'author-get-all-component',
	templateUrl: './authorGetAll.component.html',
	styleUrls: ['./authorGetAll.component.css']
})
@Injectable()
export class AuthorGetAllComponent implements OnInit {

	private _service_author: AuthorService;
	private _service_book: BookService;


	constructor(service_author: AuthorService, private _shared_service: SharedService, private router: Router, service_book: BookService,
		private route: ActivatedRoute) {
		this._service_author = service_author;
		this._service_book = service_book;
	}


	ngOnInit() {
		this.load_data();
	}

	private load_data() {
		this._shared_service._all_authors = this.route.snapshot.data['authorsList'];
	}

	private show_author(author: AuthorModel) {
		if (this._shared_service._selected_author == author) {
			this._shared_service._is_show_selected_author = false;
		}
		else {
			this._shared_service._is_show_selected_author = true;
		}
		this._shared_service._selected_author = author;
	}

	private check_book(bookID: number) {
		this.router.navigateByUrl("books");
		this._service_book.get_by_id(bookID).subscribe((val: BookModel) => this._shared_service._selected_book = val);
		this._shared_service._is_show_selected_book = true;
	}

	private turn_author_delete_page(author: AuthorModel) {
		this._service_author.delete(author.id).subscribe();
		this._service_author.get_all().subscribe((val: AuthorModel[]) => this._shared_service._all_authors = val);
		this.router.navigateByUrl("authors");
	}
}