import { Component } from '@angular/core';
import { NotificationService, ColumnItem, ActDatatableCellButton } from 'actio-components-mdbs/dist';

@Component({
    selector: 'app-order-historico',
    templateUrl: './historico.component.html',
    styleUrls: ['./historico.component.scss']
})
export class HistoricoComponent {
    constructor(public nSV: NotificationService) {
    }

    columnsOrder: ColumnItem[] = [
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
                new ActDatatableCellButton('', 'fa fa-pencil', null, 'btn btn-sm btn-albie', true, '/order/detalle/{no}')
                    .setTooltip('Editar pedido', 'left')
            ]
        })
    ];
}
