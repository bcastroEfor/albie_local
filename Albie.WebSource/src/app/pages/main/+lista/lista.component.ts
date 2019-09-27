import { Component, ViewChild } from '@angular/core';
import { ColumnItem, ActDatatableComponent, ActDatatableCellButton, NotificationService, CallbackButton } from 'actio-components-mdbs/dist';
import { IDatatableRowClickEvent } from 'actio-components-mdbs/dist/app/components/datatable/common/models/events';
import { ModalDirective } from 'angular-bootstrap-md';
import { ListaService } from '../../../shared/services/webapi/list/list.service';
import { OrderService } from '../../../shared/services/webapi/order/order.service';

@Component({
    selector: 'app-lista',
    templateUrl: './lista.component.html',
    styleUrls: ['./lista.component.scss']
})
export class ListaComponent {
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
                new ActDatatableCellButton('', 'fa fa-star-o', null, 'btn btn-sm btn-albie', true)
                    .setTooltip('Pedido habitual', 'left')
                    .setOnClickCallback((rowItem) => {
                        this.lSV.setAsUsualOrder(rowItem.id).subscribe(result => {
                            if (result) {
                                this.dtList.refreshData();
                                this.nSV.smallToast('Pedido habitual', 'Pedido marcado como habitual', 'success');
                            }
                        });
                    }),
                new ActDatatableCellButton('', 'fa fa-cart-plus', null, 'btn btn-sm btn-albie', true)
                    .setTooltip('Crear pedido al proveedor', 'left')
                    .setOnClickCallback((rowItem) => {
                        console.log('Click', rowItem);
                        this.oSV.createOrderByProvider(rowItem).subscribe(result => {
                            console.log(result);
                        });
                    }),
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
                    .setOnClickCallback((rowItem) => {
                        this.productId = rowItem.productId;
                        this.cartListId = rowItem.id;
                        this.providersModal.show();
                        setTimeout(() => {
                            this.dtProvidersRate.refreshData();
                        }, 200);
                })
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
    constructor(public lSV: ListaService,
        public nSV: NotificationService,
        public oSV: OrderService
    ) {
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
}
