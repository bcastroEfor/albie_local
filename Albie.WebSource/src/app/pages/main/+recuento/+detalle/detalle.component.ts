import { Component, ViewChild, OnInit } from '@angular/core';
import { NotificationService, ActDataselectComponent, ColumnItem, ActDatatableCellButton } from 'actio-components-mdbs/dist';
import { RecuentoService } from '../../../../shared/services/webapi/recuento/recuento.service';
import { ModalDirective } from 'angular-bootstrap-md';
import { FormControl } from '@angular/forms';

@Component({
    selector: 'app-recuento-detalle',
    templateUrl: './detalle.component.html',
    styleUrls: ['./detalle.component.scss']
})
export class DetalleComponent {
    public item: any = {};
    almacen: string;
    hasAlmacen = true;
    zona = new FormControl();
    fechaRecuento: any;

    @ViewChild('zonas') public dsZonas: ActDataselectComponent;
    @ViewChild('recuentoModal') public recuentoModal: ModalDirective;
    almacenes: any;
    zonas: any;
    tabla: any = {};
    constructor(public nSV: NotificationService, public rSV: RecuentoService) {
    }

    columnsList: ColumnItem[] = [
        new ColumnItem().setData({
            columnName: 'Nombre',
            field: 'name',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }), new ColumnItem().setData({
            columnName: 'Estado',
            field: 'status',
            sorted: true,
            filter: true,
            sortedType: 'desc',
            canChangeIsShown: true,
            isShown: true
        }),
        new ColumnItem().setData({
            buttons: [
                new ActDatatableCellButton('', 'fa fa-file-o', null, 'btn btn-sm btn-albie', true, '/recuento/lista/HR{idRecuento}')
                    .setTooltip('Ver hoja', 'left')
            ]
        })
    ];

    locationSelected(event: any) {
        this.almacen = event;
        setTimeout(() => {
            this.rSV.getZonasSelect(this.almacen).subscribe(result => { 
                this.zonas = result;
            });
        }, 100);
    }

    iniciarRecuento() {
        this.rSV.recuento('', this.almacen, '', this.zona.value).subscribe(result => {
            this.recuentoModal.hide();
            if (result.hasErrors) {
                for (let i = 0; i < result.errors.length; i++) {
                    this.nSV.smallToast('Recuento', result.errors[i], 'error');
                }
            } else {
                this.tabla = result.result;
            }
        });
    }
}
