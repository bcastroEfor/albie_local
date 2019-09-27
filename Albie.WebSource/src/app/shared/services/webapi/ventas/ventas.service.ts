import { Injectable } from '@angular/core';
import { ApiService } from 'actio-components-mdbs/dist';
import { Router } from '@angular/router';

@Injectable()
export class VentasService {
    constructor(public api: ApiService, private route: Router) { }

    getWeeks(year: number) {
        return this.api.get(`hojaRecuento/GetWeeks?y=${year}`);
    }

    getSalesCenter(year: string, month: string, mode: string) {
        return this.api.get(`salesCenter/GetSalesCenterList?y=${year}&m=${month}&mode=${mode}`);
    }
}
