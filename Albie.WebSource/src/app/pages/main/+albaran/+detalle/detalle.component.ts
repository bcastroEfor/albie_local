import { Component, ViewChild } from '@angular/core';
import { NotificationService, ColumnItem, ActDatatableCellButton } from 'actio-components-mdbs/dist';
import { ModalDirective } from 'angular-bootstrap-md';
import { AlbaranService } from '../../../../shared/services/webapi/albaran/albaran.service';
import * as moment from 'moment';
import { HistoricoService } from '../../../../shared/services/webapi/historico/historico.service';

@Component({
    selector: 'app-albaran-detalle',
    templateUrl: './detalle.component.html',
    styleUrls: ['./detalle.component.scss']
})
export class DetalleComponent {
    products: any;
    fechaAlbaran: Date;
    order: any;
    nonConform: boolean;
    constructor(public nSV: NotificationService,
        public hSV: HistoricoService,
        public aSV: AlbaranService
    ) {
    }

    @ViewChild('orderModal') public orderModal: ModalDirective;
    @ViewChild('inventarioModal') public inventarioModal: ModalDirective;
    columnsList: ColumnItem[] = [
        new ColumnItem().setData({
            columnName: 'Proveedor',
            field: 'buyFromVendorName',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            columnName: 'Fecha pedido',
            field: 'orderDate',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            fieldDisplayType: 'date',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Fecha recepción',
            field: 'expectedReceiptDate',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            fieldDisplayType: 'date',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Centro',
            field: 'centro.name',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Zona',
            field: 'zona.Name',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Nombre proveedor',
            field: 'buyFromVendorName2',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: false
        }), new ColumnItem().setData({
            columnName: 'Direccion proveedor',
            field: 'buyFromAddress',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: false
        }), new ColumnItem().setData({
            columnName: 'Direccion proveedor 2',
            field: 'buyFromAddress2',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: false
        }), new ColumnItem().setData({
            columnName: 'Proveedor ciudad',
            field: 'buyFromCity',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: false
        }), new ColumnItem().setData({
            columnName: 'Proveedor codigo postal',
            field: 'buyFromPostCode',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: false
        }), new ColumnItem().setData({
            columnName: 'Provincia',
            field: 'buyFromCounty',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Nº Pedido',
            field: 'vendorOrderNo',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Estado',
            field: 'estado',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Origen pedido',
            field: 'origenPedido',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Importe IVA',
            field: 'amountIncludingVAT',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            buttons: [
                new ActDatatableCellButton('', 'fa fa-star-o', null, 'btn btn-sm btn-albie', true)
                    .setTooltip('Recepción', 'left')
                    .setOnClickCallback((rowItem) => {
                        this.products = rowItem.lines;
                        this.order = rowItem;
                        this.orderModal.show();
                    })
            ]
        })
    ];

    saveAlbaran() {
        if (this.fechaAlbaran) {
            this.aSV.createAlbaran(this.order, moment(this.fechaAlbaran).format('YYYY-MM-DD'), this.nonConform).subscribe(result => {
                this.orderModal.hide();
                this.inventarioModal.show();
            });
        } else {
            this.nSV.smallToast('Albaran', 'Debes seleccionar una fecha para poder crear el albaran', 'error');
        }
    }

    closeOrder() {
        this.hSV.closeOrder(this.order).subscribe(result => {
            this.inventarioModal.hide();
            if (result.hasErrors) {
                this.nSV.smallToast('Pedido', 'Ocurrió un error al cerrar el pedido', 'error');
            } else {
                this.nSV.smallToast('Pedido', 'El pedido se cerro correctamente', 'success');
            }
        });
    }
}
