import { ActDatatableModule, LoadingboxModule, ActDataselectModule, AutocompleteModule, DecimalToStringPipeModule } from 'actio-components-mdbs/dist';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';
import { MatNativeDateModule, MatTooltipModule, MatExpansionModule, MatFormFieldModule, MatInputModule, MatProgressSpinnerModule, MatDatepickerModule, MatCheckboxModule, MatSelectModule, MatOptionModule } from '@angular/material';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { DndModule } from 'ngx-drag-drop';
import { ModalModule } from 'angular-bootstrap-md';
import { TemplateButtonModule } from '../../../../shared/components/_template/form/button/button.module';
import { Routes, RouterModule } from '@angular/router';
import { VentasService } from '../../../../shared/services/webapi/ventas/ventas.service';
import { ContadoComponent } from './contado.component';
import { SalesCountedService } from '../../../../shared/services/webapi/salesCounted/salesCounted.service';

export const routes: Routes = [
    {
        path: '',
        component: ContadoComponent
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
        ActDatatableModule,
        ActDataselectModule,
        AutocompleteModule,
        LoadingboxModule,
        CommonModule,
        HttpModule,
        TranslateModule,
        MatNativeDateModule,
        FormsModule,
        MatTooltipModule,
        DndModule,
        FormsModule,
        MatExpansionModule,
        MatFormFieldModule,
        MatInputModule,
        MatProgressSpinnerModule,
        ModalModule,
        TemplateButtonModule,
        MatDatepickerModule,
        MatCheckboxModule,
        MatSelectModule,
        MatOptionModule,
        DecimalToStringPipeModule
    ],
    declarations: [ContadoComponent],
    exports: [ContadoComponent],
    providers: [VentasService, SalesCountedService]
})
export class ContadoModule { }
