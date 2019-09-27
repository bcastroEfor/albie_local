import { Injectable } from '@angular/core';
import { ApiService } from 'actio-components-mdbs/dist';
import { Router } from '@angular/router';

@Injectable()
export class AlbaranService {
    private urlBase = 'albaranCompra/';
    constructor(public api: ApiService, private route: Router) { }

    createAlbaran(order: any, albaranDate: string, nonConform: boolean) {
        return this.api.post(this.urlBase + `GenerateAlbaran?da=${albaranDate}&nc=${nonConform}`, order);
    }

    anularAlbaran(albaran: any) {
        return this.api.post(this.urlBase + 'AnularAlbaran', albaran);
    }
}
