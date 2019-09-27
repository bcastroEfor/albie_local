import { Injectable } from '@angular/core';
import { ApiService } from 'actio-components-mdbs/dist';
import { Router } from '@angular/router';

@Injectable()
export class ListaService {
    private urlBase = 'cartlist/';
    constructor(public api: ApiService, private route: Router) { }

    changeProductRate(product: any, productId: string) {
        return this.api.post(`providerrate/ChangeProductRate/${productId}`, product);
    }

    setAsUsualOrder(productId: string) {
        return this.api.post(this.urlBase + `SetAsUsualOrder/${productId}`, {});
    }
}
