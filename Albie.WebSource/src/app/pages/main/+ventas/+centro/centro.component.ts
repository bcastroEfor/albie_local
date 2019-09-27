import { Component, ViewChild, OnInit } from '@angular/core';
import { NotificationService, ColumnItem, ActDataselectComponent } from 'actio-components-mdbs/dist';
import { ModalDirective } from 'angular-bootstrap-md';
import { VentasService } from '../../../../shared/services/webapi/ventas/ventas.service';
import * as moment from 'moment';
import { SalesCenterService } from '../../../../shared/services/webapi/salesCenter/salesCenter.service';

@Component({
    selector: 'app-ventas-centro',
    templateUrl: './centro.component.html',
    styleUrls: ['./centro.component.scss']
})
export class CentroComponent implements OnInit {
    private hasPermission = false;

    @ViewChild('popUpDeleteResourcePickDate') private deleteResourcePickDateModal: ModalDirective;
    private resourceToDelete: any = {};
    private deleteResourceInfo: {
        Start_Date?: Date,
        Start_Date_String?: string,
        End_Date?: Date,
        End_Date_String?: string
    } = {};
    @ViewChild('popUpMoveResourcePickDate') private moveResourcePickDateModal: ModalDirective;
    private resourceToMove: any = {};
    private moveResourceInfo: {
        Start_Date?: Date,
        Start_Date_String?: string,
        End_Date?: Date,
        End_Date_String?: string
    } = {};

    private loading: Boolean = false;
    private loadingWeeks: Boolean = false;

    private department: string;
    private area: string;
    private job_Items: string;

    job_no: string;
    job_name: string;
    job_task_no: string;
    job_task_name: string;
    opportunity_id: string;
    opportunity_name: string;
    opportunity_company: string;
    edition_description: string;
    responsable_name: string;
    bill_to_name: string;
    bill_to_code: string;

    public resources: any;
    private dates: string[] = [];
    private planification_cod: number;

    newItem: ResourcePlanItem;
    private editedItems: ResourcePlanItem[];
    isSaving: Boolean = false;

    private copyToOtherWeeksDate: Date;
    private copyToOtherWeeksDateString: string;
    private copyToOtherWeeksResources: any[];
    private copyToOtherWeeksSW_Description: Boolean = false;

    private years: number[];
    private pickedYear: number;
    private pickedWeek: number;
    private pickedMonth: number;
    private weeks: { Label: string, Value: number }[] = [];
    private months: string[] = [];

    private nextToPickYear: number;
    private nextToPickWeek: number;
    private prevToPickYear: number;
    private prevToPickWeek: number;

    enabledCopyToOtherWeeks: Boolean = false;
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

    private getCurrentViewDate(): Date {
        const firstDateString = this.dates[0];
        const firstDateOfView = new Date(firstDateString);

        return firstDateOfView;
    }

    private getMonthCode(): string {
        const currentViewDate = this.getCurrentViewDate();
        const monthNo = currentViewDate.getMonth();

        return this.months[monthNo];
    }

    private getWeeks() {
        this.loadingWeeks = true;
        this.vSV.getWeeks(this.pickedYear).subscribe(
            resp => {
                let wasFirstWeek = false;
                let launchGetList = false;
                if (this.planification_cod === 1) {
                    if (this.pickedWeek === 0) {
                        wasFirstWeek = true;
                        launchGetList = true;
                    } else if (this.weeks.length > 0 && this.pickedWeek === this.weeks[this.weeks.length - 1].Value) {
                        this.pickedWeek = 1;
                        launchGetList = true;
                    }
                }
                this.weeks = resp;
                this.loadingWeeks = false;
            },
            error => { });
    }

    private setNewItemFormatData(dates1: any[]) {
        this.newItem.dates = [];
        for (let i = 0; i < dates1.length; i++) {
            this.newItem.dates[i] = new ResourcePlanDateItem();
            this.newItem.dates[i].planning_date = dates1[i];
        }
    }

    setPrev() {
        this.pickedWeek = this.prevToPickWeek;
        if (this.pickedYear !== this.prevToPickYear) {
            this.pickedYear = this.prevToPickYear;
            this.getWeeks();
        }
        this.search();
    }
    setNext() {
        this.pickedWeek = this.nextToPickWeek;
        if (this.pickedYear !== this.nextToPickYear) {
            this.pickedWeek = 0;
            this.pickedYear = this.nextToPickYear;
            this.getWeeks();
        }
        this.search();
    }

    search() {
        this.editedItems = [];
        this.getList();
    }

    getList() {
        this.resources = [];
        this.vSV.getSalesCenter(this.year, this.month, this.modeSelected).subscribe(
            result => {
                console.log(result);
                this.resources = result;
            });
    }

    InputQtyChanged(it: any, resource_no: string, rDate: any) {
        let existResource = false;
        let existDay = false;

        this.editedItems.forEach(o => {
            if (o.resource_no === resource_no) {
                o.dates.forEach(d => {
                    if (d.planning_date === it.Planning_Date) {
                        d.quantity = it.Quantity;
                        existDay = true;
                    }
                });
                if (!existDay) {
                    const n = new ResourcePlanDateItem();
                    n.planning_date = it.Planning_Date;
                    n.quantity = it.Quantity;
                    n.line_no = it.Line_No;
                    o.dates.push(n);
                }
                existResource = true;
            }
        });
        if (!existResource) {
            const n = new ResourcePlanDateItem();
            n.planning_date = it.Planning_Date;
            n.quantity = it.Quantity;
            n.line_no = it.Line_No;
            // Si se quiere añadir linea en vez de editar la que está, descomentar esto  (y cambiar en server)
            // n.quantity = it.Quantity - rDate.QuantityAll;
            const rn = new ResourcePlanItem();
            rn.resource_no = resource_no;
            rn.dates.push(n);
            this.editedItems.push(rn);
        }
    }
}

class ResourcePlanItem {
    public resource_no: string;
    public resource_name: string;
    public dates: ResourcePlanDateItem[];

    constructor() {
        this.resource_no = '';
        this.resource_name = '';
        this.dates = [];
    }
}

class ResourcePlanDateItem {
    public planning_date: string;
    public quantity: number;
    public description: string;
    public line_no?: number;

    constructor() {
        this.planning_date = '';
        this.quantity = 0;
        this.description = '';
    }
}
