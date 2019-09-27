import { ActDatatableModule, LoadingboxModule, AutocompleteModule, ActDataselectModule } from 'actio-components-mdbs/dist';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { DndModule } from 'ngx-drag-drop';
import { Routes, RouterModule } from '@angular/router';
import { ModalModule } from 'angular-bootstrap-md';
import { DetalleComponent } from './detalle.component';
import { MatNativeDateModule, MatTooltipModule, MatExpansionModule, MatFormFieldModule, MatInputModule, MatProgressSpinnerModule, MatDatepickerModule, MAT_DATE_LOCALE } from '@angular/material';
import { OrderService } from '../../../../shared/services/webapi/order/order.service';
import { TemplateButtonModule } from '../../../../shared/components/_template/form/button/button.module';

export const routes: Routes = [
    {
        path: '',
        component: DetalleComponent
    },
    {
        path: ':id',
        component: DetalleComponent
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
        ActDatatableModule,
        ActDataselectModule,
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
        AutocompleteModule,
        MatFormFieldModule,
        MatDatepickerModule,
        TemplateButtonModule
    ],
    declarations: [DetalleComponent],
    providers: [
        OrderService,
        { provide: MAT_DATE_LOCALE, useValue: 'es-ES' }
    ]
})
export class DetalleModule { }
