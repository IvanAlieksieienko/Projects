import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { PayInputModel } from "../inputModels/pay.inputModel";

@Injectable({ providedIn: 'root' })

export class PayService {
        private url = "";

        constructor(private http: HttpClient) {

        }

        public pay(inputModel: PayInputModel) {
                return this.http.post(this.url + "Pay/Payment", inputModel, { responseType: 'text' });
        }
}