import { Component, ViewChild, OnInit } from '@angular/core';
import { NotificationService, ColumnItem, ActDataselectComponent } from 'actio-components-mdbs/dist';
import { ModalDirective } from 'angular-bootstrap-md';
import { VentasService } from '../../../../shared/services/webapi/ventas/ventas.service';
import * as moment from 'moment';
import { SalesCenterService } from '../../../../shared/services/webapi/salesCenter/salesCenter.service';

@Component({
    selector: 'app-ventas-caja',
    templateUrl: './caja.component.html',
    styleUrls: ['./caja.component.scss']
})
export class CajaComponent implements OnInit {
    public resources: any;
    
    @ViewChild('zonas') public dsZonas: ActDataselectComponent;
    @ViewChild('AddLineModal') public addLineModal: ModalDirective;
    month = '-1';
    year = '-1';
    item: any = {
        'centerCode': '',
        'postingStatus': 1,
        'readingDate': null,
        'amount': 0
    };
    constructor(public nSV: NotificationService, public vSV: VentasService, public sSV: SalesCenterService) {

    }

    modeSelect = [
        { value: '1', viewValue: 'Diario' },
        { value: '7', viewValue: 'Semanal' },
        { value: '12', viewValue: 'Mensual' }
    ];

    modeSelected = '1';
    date: string;
    ngOnInit() {
        this.getList();
    }
    getTdDays(event: string) {
        console.log(this.date);
        this.year = this.date.substring(0, 4);
        this.month = this.date.substring(5);
    }

    selectProduct(event: any) {
        this.item.ItemNo = event;
    }

    addSaleCenter() {
        console.log(this.item);
        this.sSV.addSalesCenter(this.item).subscribe(result => {
            console.log(result);
            this.nSV.smallToast('Venta', 'Venta añadida correctamente', 'success');
        });
        this.addLineModal.hide();
    }

    getList() {
        this.resources = [];
        this.vSV.getSalesCenter(this.year, this.month, this.modeSelected).subscribe(
            result => {
                console.log(result);
                this.resources = result;
            });
    }
}
