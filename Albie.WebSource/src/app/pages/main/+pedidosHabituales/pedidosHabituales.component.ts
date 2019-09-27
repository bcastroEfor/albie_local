import { Component, ViewChild } from '@angular/core';
import { ColumnItem, ActDatatableComponent, ActDatatableCellButton, NotificationService, CallbackButton } from 'actio-components-mdbs/dist';
import { ModalDirective } from 'angular-bootstrap-md';
import { ListaService } from '../../../shared/services/webapi/list/list.service';
import { IDatatableRowClickEvent } from 'actio-components-mdbs/dist/app/components/datatable/common/models/events';

@Component({
    selector: 'app-pedidos-habituales',
    templateUrl: './pedidosHabituales.component.html',
    styleUrls: ['./pedidosHabituales.component.scss']
})
export class PedidosHabitualesComponent {
    @ViewChild('cartList') public dtList: ActDatatableComponent;
    @ViewChild('providerRate') public dtProvidersRate: ActDatatableComponent;
    @ViewChild('productCartList') public dtProductsList: ActDatatableComponent;
    @ViewChild('providersModal') public providersModal: ModalDirective;
    idList: string;
    productId: string;
    columnsList: ColumnItem[] = [
        new ColumnItem().setData({
            columnName: 'Nombre',
            field: 'nombre',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            columnName: 'Fecha',
            field: 'f_Creacion',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            fieldDisplayType: 'date',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            buttons: [
                new ActDatatableCellButton('', 'fa fa-cart-plus', null, 'btn btn-sm btn-albie', true)
                    .setTooltip('Crear pedido al proveedor', 'left')
            ]
        })
    ];

    columnsProductList: ColumnItem[] = [
        new ColumnItem().setData({
            columnName: 'Nombre',
            field: 'product.description',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            columnName: 'Cantidad',
            field: 'quantity',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            columnName: 'Unidad',
            field: 'product.unitMeasureProduct.code',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            columnName: 'Cantidad',
            field: 'providerRate.directUnitCost',
            sorted: true,
            fieldDisplayType: 'currency',
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            buttons: [
                new ActDatatableCellButton('', 'fa fa-users', null, 'btn btn-sm btn-albie', true)
                    .setTooltip('Seleccionar proveedor', 'left')
            ]
        })
    ];

    columnsProviderRate: ColumnItem[] = [
        new ColumnItem().setData({
            columnName: 'Proveedor',
            field: 'provider.name',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            columnName: 'Precio unidad',
            field: 'directUnitCost',
            sorted: true,
            filter: true,
            fieldDisplayType: 'currency',
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            columnName: 'Udad',
            field: 'product.unitMeasureProduct.code',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        })
    ];

    cartListId: any;
    constructor(public lSV: ListaService, public nSV: NotificationService) {
    }

    onRowClick(event: IDatatableRowClickEvent) {
        this.idList = event.row.id;
        this.dtProductsList.refreshData();
    }

    onProductClick(event: IDatatableRowClickEvent) {
        this.productId = event.row.productId;
        this.cartListId = event.row.id;
        this.providersModal.show();
        setTimeout(() => {
            this.dtProvidersRate.refreshData();
        }, 200);
    }

    onProductsCategoryRowClick(event: IDatatableRowClickEvent) {
        this.lSV.changeProductRate(event.row, this.cartListId).subscribe(result => {
            this.providersModal.hide();
            this.dtProductsList.refreshData();
            this.nSV.smallToast('Tarifa proveedor', 'Tarifa cambiada correctamente', 'success');
        });
    }

    selectList(dtRow: CallbackButton) {
        if (dtRow.btnIndex === 0) {
            this.lSV.setAsUsualOrder(dtRow.item.id).subscribe(result => {
                if (result) {
                    this.dtList.refreshData();
                    this.nSV.smallToast('Pedido habitual', 'Pedido marcado como habitual', 'success');
                }
            });
        }
    }

    selectProduct(dtRow: CallbackButton) {
        if (dtRow.btnIndex === 0) {
            this.productId = dtRow.item.productId;
            this.cartListId = dtRow.item.id;
            this.providersModal.show();
            setTimeout(() => {
                this.dtProvidersRate.refreshData();
            }, 200);
        }
    }
}
