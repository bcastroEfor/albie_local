import { ActDatatableModule, LoadingboxModule, AutocompleteModule } from 'actio-components-mdbs/dist';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { CommonModule } from '@angular/common';
import { MatNativeDateModule, MatTooltipModule, MatExpansionModule, MatFormFieldModule, MatInputModule, MatProgressSpinnerModule } from '@angular/material';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { DndModule } from 'ngx-drag-drop';
import { Routes, RouterModule } from '@angular/router';
import { ModalModule } from 'angular-bootstrap-md';
import { DetalleComponent } from './detalle.component';
import { ProveedorService } from '../../../../shared/services/webapi/proveedor/proveedor.service';

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
        AutocompleteModule,
        MatFormFieldModule,
        MatInputModule
    ],
    declarations: [DetalleComponent],
    providers: [ProveedorService]
})
export class DetalleModule { }
