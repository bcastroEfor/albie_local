import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

@Injectable()
export class ProductService {
    public products: BehaviorSubject<any> = new BehaviorSubject<any>({});
    constructor(
    ) { }
    getProducts(product: any) {
        this.products.next(product);
    }
}
