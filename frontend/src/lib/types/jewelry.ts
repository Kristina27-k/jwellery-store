export interface JewelryItem {
    id: number;
    name: string;
    description: string;
    price: number;
    imageUrl: string;
    categoryId: number;
    categoryName?: string;
}
