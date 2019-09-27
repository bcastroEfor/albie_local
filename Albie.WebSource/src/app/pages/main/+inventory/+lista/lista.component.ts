import { Component, ViewChild } from '@angular/core';
import { NotificationService, ColumnItem, ActDatatableCellButton, ActDatatableComponent, DatatableDragAndDropConfig, ActDataselectComponent } from 'actio-components-mdbs/dist';
import { ModalDirective } from 'angular-bootstrap-md';
import { RecuentoService } from '../../../../shared/services/webapi/recuento/recuento.service';

@Component({
    selector: 'app-order-lista',
    templateUrl: './lista.component.html',
    styleUrls: ['./lista.component.scss']
})
export class ListaComponent {
    location: any;
    almacen: any;
    hasAlmacen = true;
    zona: any;
    constructor(public nSV: NotificationService, public rSV: RecuentoService) {
    }

    @ViewChild('almacenes') public dtAlmacenes: ActDatatableComponent;
    @ViewChild('products') public dtProducts: ActDatatableComponent;
    @ViewChild('zonas') public dsZonas: ActDataselectComponent;

    dtProductsDragAndDropConfig: DatatableDragAndDropConfig = new DatatableDragAndDropConfig().setData({
        enabledDropzone: true,
        enabledDraggable: true,
        effectAllowed: 'copy',
        datatableDropzoneTypes: ['almacenes-data'],
        rowDropzoneType: 'products-data',
        rowDraggingClass: 'dndDragging',
        rowDraggingSourceClass: 'dndDraggingSource',
        rowDraggableDisabledClass: 'dndDraggingDisabled',
        datatableDragoverClass: 'dndDragover',
        datatableDropzoneDisabledClass: 'dndDropzoneDisabled',
        datatablePlaceholderClass: 'dndPlaceholder text-center',
        datatablePlaceholderLabel: 'Arrastrar'
    });
    columnsList: ColumnItem[] = [
        new ColumnItem().setData({
            columnName: 'Descripcion',
            field: 'description',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Precio',
            field: 'unitPrice',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'almacen',
            field: 'almacenZP.location.name',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Zona',
            field: 'almacenZP.zonas.name',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        })
    ];

    dtAlmacenesDragAndDropConfig: DatatableDragAndDropConfig = new DatatableDragAndDropConfig().setData({
        enabledDropzone: true,
        enabledDraggable: true,
        effectAllowed: 'copy',
        datatableDropzoneTypes: ['products-data'],
        rowDropzoneType: 'almacenes-data',
        rowDraggingClass: 'dndDragging',
        rowDraggingSourceClass: 'dndDraggingSource',
        rowDraggableDisabledClass: 'dndDraggingDisabled',
        datatableDragoverClass: 'dndDragover',
        datatableDropzoneDisabledClass: 'dndDropzoneDisabled',
        datatablePlaceholderClass: 'dndPlaceholder text-center',
        datatablePlaceholderLabel: 'Arrastrar'
    });
    almacenColumns: ColumnItem[] = [
        new ColumnItem().setData({
            columnName: 'Almacen',
            field: 'location.name',
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
            columnName: 'Zona',
            field: 'zonas.name',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        })
    ];

    locationSelected(event: any) {
        this.almacen = event;
        setTimeout(() => {
            this.dsZonas.refreshItems();
        }, 100);
    }

    zonaSelected(event: any) {
        this.zona = event;
        setTimeout(() => {
            this.dtAlmacenes.refreshData();
        }, 0);
    }

    dropSelectProduct(input: { event: DragEvent, data: any }) {
        // hacer funcion que el producto se elimine del almacen
    }

    dropSelectAlmacen(input: { event: DragEvent, data: any }) {
        // hacer funcion que guarde el producto en el almacen
        if (!this.almacen || !this.zona) {
            this.nSV.smallToast('Añadir producto', 'Debe seleccionar almacen y zona antes de añadir un producto', 'warning');;
        } else {
            this.rSV.addProductToAlmacen(input.data, this.almacen, this.zona).subscribe(result => {
                this.nSV.smallToast('Producto', 'Producto asignado correctamente', 'success');
                this.dtAlmacenes.refreshData();
                this.dtProducts.refreshData();
            });
        }
    }

    productsSinAlmacen() {
        this.dtProducts.refreshData();
    }
}
