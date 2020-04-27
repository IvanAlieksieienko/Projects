import { BookModel } from "./book.model";
import { UserModel } from "./user.model";

export class BasketModel {
        public id: number;
        public bookPrice: number;
        public bookID: number;
        public bookTitle: string;
        public userID: number;
}
