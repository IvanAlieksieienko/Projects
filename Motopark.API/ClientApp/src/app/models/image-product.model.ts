import { Guid } from "guid-typescript";

export class ImageProductModel {
    public id: Guid;
    public productID: Guid;
    public imagePath: string;
    public isFirst: boolean;
}