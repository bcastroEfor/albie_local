import { Component, ViewChild } from '@angular/core';
import { NotificationService, ColumnItem, ActDatatableCellButton, ActDatatableComponent, DatatableDragAndDropConfig, ActDataselectComponent } from 'actio-components-mdbs/dist';
import { RecuentoService } from '../../../../shared/services/webapi/recuento/recuento.service';
import { ModalDirective } from 'angular-bootstrap-md';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-recuento-lista',
    templateUrl: './lista.component.html',
    styleUrls: ['./lista.component.scss']
})
export class ListaComponent {
    location: any;
    almacen: any;
    cerrado = true;
    zona: any;
    hojas: any = {};
    idRecuento: any;
    @ViewChild('zonas') public dsZonas: ActDataselectComponent;
    @ViewChild('almacenes') public dtAlmacenes: ActDatatableComponent;
    @ViewChild('hojas') public dtProducts: ActDatatableComponent;
    @ViewChild('AddLineModal') public addLineModal: ModalDirective;
    @ViewChild('productSelectModal') public productSelectModal: ModalDirective;
    status = 0;
    products: any[];
    product: {};
    constructor(private aRoute: ActivatedRoute, public nSV: NotificationService, public rSV: RecuentoService) {
        this.aRoute.params.subscribe((newParams) => {
            if (newParams != null) {
                this.idRecuento = newParams.id;
                this.getHojas();
            }
        });
    }
    columnsList: ColumnItem[] = [
        new ColumnItem().setData({
            columnName: 'Código',
            field: 'codigo',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Fecha',
            field: 'date',
            sorted: true,
            fieldDisplayType: 'date',
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Almacen',
            field: 'location.name',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Zona',
            field: 'zona.name',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Producto',
            field: 'product.description',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Cantidad',
            field: 'quantity',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        })
    ];

    modalCantidades() {
        this.products = this.dtProducts.items;
        this.addLineModal.show();
    }

    addLineToModal() {
        this.productSelectModal.show();
    }

    selectProduct(event: any) {
        this.product = {
            center: this.products[0].center,
            centerCode: this.products[0].centerCode,
            codigo: this.products[0].codigo,
            date: this.products[0].date,
            entryNo: 0,
            location: this.products[0].location,
            locationCode: this.products[0].locationCode,
            product: {},
            productNo: event,
            quantity: 0,
            zona: this.products[0].zona,
            zone: this.products[0].zone
        };
        this.products.push(this.product);
        this.productSelectModal.hide();
    }

    saveHojas() {
        this.rSV.UpdMultiRecueto(this.products).subscribe(result => {
            console.log(result);
            this.addLineModal.hide();
        });
    }

    getHojas() {
        setTimeout(() => {
            this.dtProducts.refreshData();
        }, 0);
    }
}
