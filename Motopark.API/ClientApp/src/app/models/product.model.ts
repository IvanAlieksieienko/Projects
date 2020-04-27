import { Guid } from "guid-typescript";

export class ProductModel {
    public id: Guid;
    public categoryID: Guid;
    public name: string;
    public description: string;
    public price: number;
    public isAvailable: boolean;
}