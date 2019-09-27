import { Injectable } from '@angular/core';
import { ApiService } from 'actio-components-mdbs/dist';
import { Router } from '@angular/router';

@Injectable()
export class ProveedorService {
    private urlBase = 'provider/';
    constructor(public api: ApiService, private route: Router) { }

    UpdProvider(provider: any, insertIfNoExists: boolean) {
        return this.api.post(this.urlBase + `UpdProvider?insertIfNoExists=${insertIfNoExists}`, provider);
    }
}
