import { NgModule } from '@angular/core';
import { LayoutAuthComponent } from './auth.component';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [
        RouterModule,
    ],
    declarations: [
        LayoutAuthComponent
    ],
    exports: [
        LayoutAuthComponent
    ]
})

export class LayoutAuthModule { }
