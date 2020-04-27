import { Component } from "@angular/core";
import { Router } from "@angular/router";

@Component({
    selector: 'contact',
    templateUrl: './contact.component.html',
    styleUrls: ['./contact.component.css']
})
export class ContactComponent {

    pic = "Resources//Images//contact.jpg";
    public first: boolean = false;
    public second: boolean = false;
    public third: boolean = false;

    constructor(public router: Router) { }

    ngOnInit() {

    }

    copyMessage(val: string) {
        if (val == '+380 95 949 83 34') {
            this.first = true;
            this.second = false;
            this.third = false;
        }
        if (val == '+380 98 416 67 82') {
            this.first = false;
            this.second = true;
            this.third = false;
        }
        if (val == '+380 68 550 43 75') {
            this.first = false;
            this.second = false;
            this.third = true;
        }
        const selBox = document.createElement('textarea');
        selBox.style.position = 'fixed';
        selBox.style.left = '0';
        selBox.style.top = '0';
        selBox.style.opacity = '0';
        selBox.value = val;
        document.body.appendChild(selBox);
        selBox.focus();
        selBox.select();
        document.execCommand('copy');
        document.body.removeChild(selBox);
        alert("Номер скопирован в буфер обмена!");
    }

    disableFirst() {
        this.first = false;
    }

    disableSecond() {
        this.second = false;
    }

    disableThird() {
        this.third = false;
    }
}