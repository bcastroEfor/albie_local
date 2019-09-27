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
import { MatNativeDateModule, MatTooltipModule, MatExpansionModule, MatFormFieldModule, MatInputModule, MatProgressSpinnerModule, MatDatepickerModule, MAT_DATE_LOCALE, MatCheckboxModule } from '@angular/material';
import { TemplateButtonModule } from '../../../../shared/components/_template/form/button/button.module';
import { AlbaranService } from '../../../../shared/services/webapi/albaran/albaran.service';
import { HistoricoService } from '../../../../shared/services/webapi/historico/historico.service';

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
        MatCheckboxModule
    ],
    declarations: [DetalleComponent],
    providers: [
        AlbaranService,
        HistoricoService,
        { provide: MAT_DATE_LOCALE, useValue: 'es-ES' }
    ]
})
export class DetalleModule { }
