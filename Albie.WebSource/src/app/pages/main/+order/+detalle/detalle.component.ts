import { Component } from '@angular/core';
import { NotificationService } from 'actio-components-mdbs/dist';
import { ActivatedRoute } from '@angular/router';
import { OrderService } from '../../../../shared/services/webapi/order/order.service';

@Component({
    selector: 'app-order-detalle',
    templateUrl: './detalle.component.html',
    styleUrls: ['./detalle.component.scss']
})
export class DetalleComponent {
    public item: any = {};

    constructor(private aRoute: ActivatedRoute, public oSV: OrderService, public nSV: NotificationService) {
        this.aRoute.params.subscribe((newParams) => {
            if (newParams != null) {
                this.getById(newParams.id);
            }
        });
    }

    providerSelect(event: any) {
        this.item.provider = event;
    }

    centroSelect(event: any) {
        this.item.centro = event;
    }

    zonaSelect(event: any) {
        this.item.zona = event;
    }

    estadoSelect(event: any) {
        this.item.estado = event;
    }

    getById(id: string) {
        this.oSV.getOrderById(id).subscribe(
            result => {
                this.item = result;
            });
    }

    saveOrder() {
        this.oSV.updOrder(this.item).subscribe(result => {
            if (!result.hasErrors) {
                this.nSV.smallToast('Editar pedido', 'El pedido se ha editado correctamente', 'success');
            } else {
                this.nSV.smallToast('Editar pedido', result.errors.toString(), 'error');
            }
            console.log(result);
        });
    }
}
