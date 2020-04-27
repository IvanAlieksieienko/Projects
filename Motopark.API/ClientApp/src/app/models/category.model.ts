import { Guid } from "guid-typescript";

export class CategoryModel {
    public id: Guid;
    public categoryID: Guid;
    public name: string;
    public description: string;
    public imagePath: string;
}