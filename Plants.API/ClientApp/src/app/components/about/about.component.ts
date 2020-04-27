import { Component } from "@angular/core";
import { faCalendarAlt, faInfinity, faPercent, faCheck, faChevronDown } from '@fortawesome/free-solid-svg-icons';
 
@Component({
    selector: 'about',
    templateUrl: './about.component.html',
    styleUrls: ['./about.component.css']
})
export class AboutComponent {

    pic = "Resources//Images//about.jpg";
    calendar = faCalendarAlt;
    infinity = faInfinity;
    percent = faPercent;
    check = faCheck;
    down = faChevronDown;

    constructor() {

    }

    ngOnInit() {
        
    }
}