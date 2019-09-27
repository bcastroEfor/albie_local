import { Injectable } from '@angular/core';
import { ApiService } from 'actio-components-mdbs/dist';
import { Router } from '@angular/router';

@Injectable()
export class SalesCountedService {
    constructor(public api: ApiService, private route: Router) { }

    getSalesCenter(year: string, month: string, mode: string) {
        return this.api.get(`salesCountedCenter/GetSalesCountedCenterList?y=${year}&m=${month}&mode=${mode}`);
    }
}
