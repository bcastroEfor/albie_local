import { NgModule } from '@angular/core';

import { LayoutMainModule } from './main/main.layout.module';
import { LayoutAuthModule } from './auth/auth.module';

@NgModule({
    imports: [
        LayoutMainModule,
        LayoutAuthModule
    ],
    declarations: [
    ],
    exports: [
        LayoutMainModule,
        LayoutAuthModule
    ]
})

export class LayoutsModule { }
