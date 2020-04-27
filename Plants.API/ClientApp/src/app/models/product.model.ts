import { Guid } from "guid-typescript";

export class ProductModel {
    public id: Guid;
    public categoryID: Guid;
    public isAvailable: boolean;
    public name: string;
    public description: string;
    public imagePath: string;
    public price: number;
}