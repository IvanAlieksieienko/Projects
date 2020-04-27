import { Component } from '@angular/core';
import { SharedService } from './services/shared.service';
import { BehaviorSubject } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { RouterOutlet } from '@angular/router';

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.css']
})
export class AppComponent {
	title = 'app';

	constructor(public _sharedService: SharedService, private spinner: NgxSpinnerService) {
		_sharedService.loadingChange.subscribe(value => {
			if (value == true) {
				this.spinner.show();
			}
			else {
				setTimeout(() => {
					this.spinner.hide();
				}, 1500);				
			}
		})
	}

	ngOnInit() {
	}
}
