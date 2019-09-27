import { Injectable } from '@angular/core';
import { ApiService } from 'actio-components-mdbs/dist';
import { Router } from '@angular/router';

@Injectable()
export class SalesCenterService {
    private urlBase = 'salesCenter/';
    constructor(public api: ApiService, private route: Router) { }

    addSalesCenter(salesCenter: any) {
        return this.api.post(this.urlBase + `UpdSalesCenter?insertIfNoExists=true`, salesCenter);
    }
}
