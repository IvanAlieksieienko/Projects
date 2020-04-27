import { Guid } from "guid-typescript";

export class DeliveryModel {
    public id: Guid;
    public orderID: Guid;
    public payType: number;
    public postName: number;
    public deliveryName: number;
    public city: string;
    public region: string;
    public street: string;
    public houseNumber: string;
}