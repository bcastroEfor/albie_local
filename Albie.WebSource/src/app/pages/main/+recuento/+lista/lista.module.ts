import { ActDatatableModule, LoadingboxModule, ActDataselectModule, AutocompleteModule } from 'actio-components-mdbs/dist';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';
import { MatNativeDateModule, MatTooltipModule, MatExpansionModule, MatFormFieldModule, MatInputModule, MatProgressSpinnerModule, MatDatepickerModule, MatCheckboxModule, MatSelectModule, MatOptionModule } from '@angular/material';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { DndModule } from 'ngx-drag-drop';
import { Routes, RouterModule } from '@angular/router';
import { ModalModule } from 'angular-bootstrap-md';
import { ListaComponent } from './lista.component';
import { TemplateButtonModule } from '../../../../shared/components/_template/form/button/button.module';
import { RecuentoService } from '../../../../shared/services/webapi/recuento/recuento.service';

export const routes: Routes = [
    {
        path: '',
        component: ListaComponent
    },
    {
        path: ':id',
        component: ListaComponent
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
        MatOptionModule
    ],
    declarations: [ListaComponent],
    providers: [RecuentoService]
})
export class ListaModule { }
