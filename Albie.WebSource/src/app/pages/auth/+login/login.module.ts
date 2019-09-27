import { NgModule } from '@angular/core';
import { LoginComponent } from './login.component';
import { routing } from './login.routing';

import { TranslateModule } from '@ngx-translate/core';
import { AuthService } from '../../../shared/services/angular/auth/auth.service';
import { CommonFormsModule } from '../../../shared/commons/modules/forms.module';

@NgModule({
    imports: [
        routing,
        TranslateModule,
        // Common forms:
        CommonFormsModule,
    ],
    declarations: [
        LoginComponent
    ],
    providers: [
        AuthService
    ]

})
export class LoginModule { }
