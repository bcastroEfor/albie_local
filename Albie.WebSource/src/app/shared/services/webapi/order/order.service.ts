import { Injectable } from '@angular/core';
import { ApiService } from 'actio-components-mdbs/dist';
import { Router } from '@angular/router';

@Injectable()
export class OrderService {
    private urlBase = 'order/';
    constructor(public api: ApiService, private route: Router) { }

    createOrderByProvider(list: any) {
        return this.api.post(this.urlBase + 'CreateOrderByProvider', list);
    }

    getOrderById(id: string) {
        return this.api.get(this.urlBase + `GetCabeceraById?id=` + id);
    }

    updOrder(item: any) {
        return this.api.post(this.urlBase + 'UpdCabecera', item);
    }
}
