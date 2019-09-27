import { Component } from '@angular/core';
import { NotificationService } from 'actio-components-mdbs/dist';
import { ProveedorService } from '../../../../shared/services/webapi/proveedor/proveedor.service';
import { ActivatedRoute } from '@angular/router';
import { OrderService } from '../../../../shared/services/webapi/order/order.service';

@Component({
    selector: 'app-proveedor-detalle',
    templateUrl: './detalle.component.html',
    styleUrls: ['./detalle.component.scss']
})
export class DetalleComponent {
    public item: any = {
        blocked: ''
    };
    constructor(private aRoute: ActivatedRoute,
        public nSV: NotificationService,
        public pSV: ProveedorService) {
        this.aRoute.params.subscribe((newParams) => {
            if (newParams.id) {
                this.item.Id = newParams.id;
            }
        });
    }

    selectZona(event: any) {
        this.item.zona = event;
    }

    selectCentro(event: any) {
        this.item.centro = event;
    }

    saveProvider() {
        this.pSV.UpdProvider(this.item, true).subscribe(result => {
            if (result) {
                this.nSV.smallToast('Proveedores', 'El proveedor ha sido creado correctamente', 'success');
            } else {
                this.nSV.smallToast('Proveedores', 'Ha ocurrido un problema al crear el proveedor', 'error');
            }
        });
    }

    
}
