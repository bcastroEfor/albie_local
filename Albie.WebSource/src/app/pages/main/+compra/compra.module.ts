import { CompraComponent } from './compra.component';
import { ActDataselectModule, ActDatatableModule, LoadingboxModule, AutocompleteModule } from 'actio-components-mdbs/dist';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';
import { MatCardModule, MatCheckboxModule, MatDatepickerModule, MatNativeDateModule, MatDialogModule, MatSelectModule, MatOptionModule, MatTooltipModule, MatExpansionModule, MatFormFieldModule, MatInputModule, MatProgressSpinnerModule, MatDividerModule, MatSliderModule } from '@angular/material';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { DndModule } from 'ngx-drag-drop';
import { ModalModule } from 'angular-bootstrap-md';
import { Routes, RouterModule } from '@angular/router';
import { CompraService } from '../../../shared/services/webapi/compra/compra.service';

export const routes: Routes = [
    {
        path: '',
        component: CompraComponent
    },
    {
        path: ':id',
        component: CompraComponent
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(routes),
        ActDataselectModule,
        ActDatatableModule,
        LoadingboxModule,
        CommonModule,
        MatCardModule,
        MatCheckboxModule,
        HttpModule,
        TranslateModule,
        MatDatepickerModule,
        MatNativeDateModule,
        FormsModule,
        MatDialogModule,
        MatSelectModule,
        MatOptionModule,
        MatTooltipModule,
        DndModule,
        ModalModule,
        MatDividerModule,
        MatDatepickerModule,
        FormsModule,
        MatExpansionModule,
        MatFormFieldModule,
        MatInputModule,
        MatProgressSpinnerModule,
        MatSliderModule,
        AutocompleteModule
    ],
    declarations: [CompraComponent],
    providers: [CompraService]
})
export class CompraModule { }
