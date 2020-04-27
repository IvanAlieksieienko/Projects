import { Guid } from "guid-typescript";

export class BasketModel {
    public id: Guid;
    public productID: Guid;
    public count: number;
}