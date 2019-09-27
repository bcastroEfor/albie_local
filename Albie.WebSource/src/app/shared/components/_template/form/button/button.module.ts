import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from './button.component';
import { RouterModule } from '@angular/router';
import { MatProgressSpinnerModule, MatTooltipModule } from '@angular/material';
import { RippleModule } from 'angular-bootstrap-md';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    
    MatProgressSpinnerModule,

    RippleModule,
    MatTooltipModule,
  ],
  declarations: [ButtonComponent],
  exports: [ButtonComponent]
})
export class TemplateButtonModule { }
