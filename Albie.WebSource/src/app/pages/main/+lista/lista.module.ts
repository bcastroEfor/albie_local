import { ActDatatableModule, LoadingboxModule } from 'actio-components-mdbs/dist';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';
import { MatNativeDateModule, MatTooltipModule, MatExpansionModule, MatFormFieldModule, MatInputModule, MatProgressSpinnerModule } from '@angular/material';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { DndModule } from 'ngx-drag-drop';
import { Routes, RouterModule } from '@angular/router';
import { ListaComponent } from './lista.component';
import { ModalModule } from 'angular-bootstrap-md';
import { ListaService } from '../../../shared/services/webapi/list/list.service';
import { OrderService } from '../../../shared/services/webapi/order/order.service';

export const routes: Routes = [
    {
        path: '',
        component: ListaComponent
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
    ],
    declarations: [ListaComponent],
    providers: [ListaService, OrderService]
})
export class ListaModule { }