import { Guid } from "guid-typescript";

export class OrderModel {
    public id: Guid;
    public basketID: Guid;
    public deliveryID: Guid;
    public totalPrice: number;
    public name: string;
    public surname: string;
    public patronymic: string;
    public email: string;
    public phoneNumber: string;
    public comment: string;
    public orderState: number;
    public creationDate: Date;
}