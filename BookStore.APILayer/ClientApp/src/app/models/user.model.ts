import { UserBookModel } from "./userbook.model";
import { BasketModel } from "./basket.model";

export class UserModel {
        public id: number;
        public email: string;
        public password: string;
        public role: string;
        public confirmed: boolean;
        public userBook: Array<UserBookModel>;
        public userBasket: Array<BasketModel>;
}