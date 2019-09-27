import { Component, ViewChild, OnInit } from '@angular/core';
import { NotificationService, ActDataselectComponent } from 'actio-components-mdbs/dist';
import { ModalDirective } from 'angular-bootstrap-md';
import { VentasService } from '../../../../shared/services/webapi/ventas/ventas.service';
import * as moment from 'moment';
import { SalesCountedService } from '../../../../shared/services/webapi/salesCounted/salesCounted.service';

@Component({
    selector: 'app-ventas-contado',
    templateUrl: './contado.component.html',
    styleUrls: ['./contado.component.scss']
})
export class ContadoComponent implements OnInit {
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
    resources: any;

    modeSelect = [
        { value: '1', viewValue: 'Diario' },
        { value: '7', viewValue: 'Semanal' },
        { value: '12', viewValue: 'Mensual' }
    ];

    modeSelected = '1';
    date: string;
    constructor(public nSV: NotificationService, public sSV: SalesCountedService) {
    }

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
        // this.sSV.addSalesCenter(this.item).subscribe(result => {
        //     console.log(result);
        //     this.nSV.smallToast('venta añadida', 'Venta de maquina añadida', 'success');
        // });
        this.addLineModal.hide();
    }

    getList() {
        this.resources = [];
        this.sSV.getSalesCenter(this.year, this.month, this.modeSelected).subscribe(result => {
            console.log(result);
            this.resources = result;
        });
    }
}
