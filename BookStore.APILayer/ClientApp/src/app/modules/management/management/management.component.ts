import { Component, OnInit, Injectable } from '@angular/core';
import { SharedService } from '../../../services/shared.service';
import { Router } from '@angular/router';

@Component({
	selector: 'management-component',
	templateUrl: './management.component.html',
	styleUrls: ['./management.component.css']
})
@Injectable()
export class ManagementComponent implements OnInit {


	constructor(private _shared_service: SharedService, private router: Router) {

	}


	ngOnInit() {

	}

}