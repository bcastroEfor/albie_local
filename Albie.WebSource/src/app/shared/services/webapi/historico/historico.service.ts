import { Injectable } from '@angular/core';
import { ApiService } from 'actio-components-mdbs/dist';
import { Router } from '@angular/router';

@Injectable()
export class HistoricoService {
    private urlBase = 'historicoPedido/';
    constructor(public api: ApiService, private route: Router) { }

    closeOrder(product: any) {
        return this.api.post(this.urlBase + 'CloseOrder', product);
    }
}
