import { Injectable } from '@angular/core';
import { ApiService } from 'actio-components-mdbs/dist';
import { Router } from '@angular/router';

@Injectable()
export class RecuentoService {
    private urlBase = 'almacenZP/';
    private urlBaseRecuento = 'hojaRecuento/';
    constructor(public api: ApiService, private route: Router) { }

    addProductToAlmacen(product: any, almacen: string, zona: string) {
        return this.api.post(this.urlBase + `AddProductToAlmacen?almacen=${almacen}&zona=${zona}`, product);
    }

    recuento(center: string, almacen: string, product: string, zona: string) {
        return this.api.get(this.urlBaseRecuento + `GetRecuento?center=${center}&almacen=${almacen}&product=${product}&zona=${zona}`);
    }

    getZonasSelect(almacen: string) {
        return this.api.get(`zona/GetZonaSelectMulti?al=${almacen}`);
    }

    getHojasByCodigo() {
        return this.api.get(this.urlBaseRecuento + 'GetHojasByCodigo');
    }

    getHojasByRecuento(idRecuento: string, filter: any[], pindex: number, psize: number, sortName: string = null, isDescending: boolean = true) {
        this.api.post(this.urlBaseRecuento + `GetCollectionListHojaRecuentos?pi=${pindex}&ps=${psize}&sn=${sortName}&sd=${isDescending}&rId=${idRecuento}`, filter);
    }

    UpdMultiRecueto(hojas: any) {
        return this.api.post(this.urlBaseRecuento + `UpdHojaRecuentoMulti?insertIfNoExists=true`, hojas);
    }
}
