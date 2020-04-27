import { Component, OnInit, Injectable } from '@angular/core';
import { SharedService } from '../../../services/shared.service';
import { Router } from '@angular/router';
import { GenreService } from '../../../services/genre.service';
import { GenreInputModel } from '../../../inputModels/genre.inputModel';
import { CanComponentDeactivate } from '../../CanDeactiveGuard/can-deactive.guard';
import { Observable } from 'rxjs';

@Component({
	selector: 'genre-update-component',
	templateUrl: './genreUpdate.component.html',
	styleUrls: ['./genreUpdate.component.css']
})
@Injectable()
export class GenreUpdateComponent implements OnInit, CanComponentDeactivate {

	private _service_genre: GenreService;
	private _new_genre: GenreInputModel = new GenreInputModel();
	private sended: boolean = false;

	constructor(service_genre: GenreService, private _shared_service: SharedService, private router: Router) {
		this._service_genre = service_genre;
	}


	ngOnInit() {

	}

	private update_genre() {
		var correctness = true;
		if (this._new_genre.Name == "") {
			correctness = false;
		}
		if (correctness == true) {
			this.sended = true;
			this._service_genre.update_genre(this._new_genre, this._shared_service._selected_genre.id).subscribe();
			this.router.navigateByUrl('genres');
		}
		else {

		}
	}

	public canDeactivate(): Observable<boolean> | Promise<boolean> | boolean {
		if (this._new_genre.Name == undefined || !this.sended) {
			return confirm('Your changes are unsaved! Do you like to exit?');
		}
		return true;
	}
}