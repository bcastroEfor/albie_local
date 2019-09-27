import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';

import {
  TranslateModule,
  TranslateLoader,
  TranslateService,
  MissingTranslationHandler,
  MissingTranslationHandlerParams
} from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { MAT_DATE_LOCALE, MatInputModule } from '@angular/material';
import { ToastrModule } from 'ngx-toastr';
import { MDBBootstrapModule, ModalModule } from 'angular-bootstrap-md';
import { environment } from '../environments/environment';

import {
  NotificationService,
  SettingsService,
  ApiService,
  API_BASE_URL,
  DecimalToStringPipeModule,
  ReplacePipeModule,
  DecimalToStringPipe,
  ReplacePipe,
  API_HEADER_KEY_LANG,
  API_SETTINGS_KEY_LANG
} from 'actio-components-mdbs/dist';

import { AppComponent } from './app.component';
import { routing } from './app.routing';
import { LayoutsModule } from './layouts/layouts.module';
import { AuthService } from './shared/services/angular/auth/auth.service';
import { MenuService } from './shared/services/angular/menu/menu.service';
import { ProductService } from './shared/services/angular/product/product.service';
export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, '/assets/i18n/', '.json');
}
class AbpMissingTranslationHandler implements MissingTranslationHandler {
  handle(params: MissingTranslationHandlerParams) {
    return params.key;
  }
}

const APPLICATION_WIDE_SERVICES = [
  MenuService
];

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    routing,
    LayoutsModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MDBBootstrapModule.forRoot(),
    ModalModule.forRoot(),
    TranslateModule.forRoot({
      missingTranslationHandler: {
        provide: MissingTranslationHandler,
        useClass: AbpMissingTranslationHandler
      },
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    }),
    ToastrModule.forRoot(),
    MatInputModule,
    DecimalToStringPipeModule.forRoot()
  ],
  providers: [
    APPLICATION_WIDE_SERVICES,
    TranslateService,
    ApiService,
    ProductService,
    SettingsService,
    NotificationService,
    { provide: MAT_DATE_LOCALE, useValue: 'es-ES' },
    { provide: API_BASE_URL, useValue: environment.apiUrl },
    { provide: API_SETTINGS_KEY_LANG, useValue: 'lang' },
    { provide: API_HEADER_KEY_LANG, useValue: 'lang' },
    AuthService,
    DecimalToStringPipe,
    ReplacePipe,
  ],
  bootstrap: [AppComponent],
  schemas: [NO_ERRORS_SCHEMA]
})
export class AppModule { }
