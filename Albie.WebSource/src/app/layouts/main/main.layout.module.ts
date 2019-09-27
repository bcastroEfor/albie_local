import { NgModule } from '@angular/core';
import { LayoutMainComponent } from './main.layout';
import { CommonModule, DatePipe } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';
import { RouterModule } from '@angular/router';
import { MatSidenavModule, MatDatepickerModule, DateAdapter, MAT_DATE_LOCALE, MatNativeDateModule, MatExpansionModule, MatFormFieldModule, MatTableModule, MatDialogModule, MatInputModule } from '@angular/material';
import { NavbarModule, DropdownModule, RippleModule, ModalModule } from 'angular-bootstrap-md';
import {
    ActPageTitleModule,
    BreadcrumbModule,
    LanguageSelectorModule,
    ActDataselectModule,
    AutocompleteModule
} from 'actio-components-mdbs/dist';
import { MenuService } from '../../shared/services/angular/menu/menu.service';
import { MyDateAdapter } from '../../datepicker-adapter';
import { FormsModule } from '@angular/forms';
import { CompraService } from '../../shared/services/webapi/compra/compra.service';

@NgModule({
    imports: [
        RouterModule,
        CommonModule,
        TranslateModule.forChild(),
        MatSidenavModule,
        NavbarModule,
        ActPageTitleModule,
        BreadcrumbModule,
        DropdownModule,
        MatTableModule,
        RippleModule,
        LanguageSelectorModule,
        MatDatepickerModule,
        MatNativeDateModule,
        FormsModule,
        ActDataselectModule,
        AutocompleteModule,
        MatExpansionModule,
        MatFormFieldModule,
        ModalModule,
        MatFormFieldModule,
        MatInputModule,
        AutocompleteModule
    ],
    declarations: [
        LayoutMainComponent
    ],
    exports: [
        LayoutMainComponent
    ],
    providers: [
        MenuService,
        CompraService,
        DatePipe,
        { provide: DateAdapter, useClass: MyDateAdapter },
        { provide: MAT_DATE_LOCALE, useValue: 'es-ES' },
    ]
})
export class LayoutMainModule { }
