export interface Product {
  id: string
  code: string
  name: string
  description: string
  image: string
  price: number
  category: string
  quantity: number
  inventoryStatus: string
  rating: number
}

const products: Product[] = [
  {
    id: '1',
    code: 'f230fh0g3',
    name: 'Bamboo Watch',
    description: 'Product Description',
    image: 'bamboo-watch.jpg',
    price: 65,
    category: 'Accessories',
    quantity: 24,
    inventoryStatus: 'INSTOCK',
    rating: 5,
  },
]

export class ProductService {
  static getProducts(): Promise<Product[]> {
    return Promise.resolve(products)
  }
  static getProductById(id: string): Promise<Product | undefined> {
    return Promise.resolve(products.find((p) => p.id === id))
  }
}
