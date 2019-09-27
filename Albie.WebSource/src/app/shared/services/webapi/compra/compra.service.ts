import { Injectable } from '@angular/core';
import { ApiService } from 'actio-components-mdbs/dist';
import { Router } from '@angular/router';

@Injectable()
export class CompraService {
    private urlBase = 'compra/';
    constructor(public api: ApiService, private route: Router) { }

    getProductos(parentCategory: string) {
        return this.api.get(`product/GetProductosList?ParentCategory=${parentCategory}`);
    }

    postList(products: any[], nombreLista: string) {
        return this.api.post(this.urlBase + `PostList?nameList=${nombreLista}`, products);
    }

    changeProductRate(product: any, productId: string) {
        return this.api.post(`cartlist/ChangeProductRate/${productId}`, product);
    }
}
