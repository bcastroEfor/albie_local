import { NgModule } from '@angular/core';
import { AuthComponent } from './auth.component';
import { routing } from './auth.routing';

@NgModule({
    imports: [
        routing
    ],
    declarations: [
        AuthComponent
    ]
})
export class AuthModule { }
