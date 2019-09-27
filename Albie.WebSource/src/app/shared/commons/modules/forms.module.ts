import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {
    RippleModule,
    ActiveModule,
    DropdownModule,
    ModalModule
} from 'angular-bootstrap-md';

import {
    MatProgressSpinnerModule,
    MatDatepickerModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatSelectModule,
    MatExpansionModule,

    MatAutocompleteModule
} from '@angular/material';
import { ActDataselectModule, AutocompleteModule } from 'actio-components-mdbs/dist';
import { TemplateButtonModule } from '../../components/_template/form/button/button.module';

@NgModule({
    imports: [
        // MD Angular
        // Temp:
        MatAutocompleteModule,
        // ----
        MatFormFieldModule,
        MatDatepickerModule,
        MatCheckboxModule,
        MatSelectModule,
        MatProgressSpinnerModule,
        MatExpansionModule,
        // MDBootstrap Angular
        ModalModule,
        CommonModule,
        FormsModule,
        RippleModule,
        ActiveModule,
        DropdownModule,
        // Actio modules
        AutocompleteModule,
        ActDataselectModule,
        TemplateButtonModule,
    ],
    declarations: [
    ],
    exports: [
        // MD Angular
        // Temp:
        MatAutocompleteModule,
        // ----
        MatFormFieldModule,
        MatDatepickerModule,
        MatCheckboxModule,
        MatSelectModule,
        MatProgressSpinnerModule,
        MatExpansionModule,
        // MDBootstrap Angular
        ModalModule,
        CommonModule,
        FormsModule,
        RippleModule,
        ActiveModule,
        DropdownModule,
        // Actio modules
        AutocompleteModule,
        // Template modules
        TemplateButtonModule,
        //----
    ]
})
export class CommonFormsModule { }
