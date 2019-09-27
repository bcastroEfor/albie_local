import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ThemePalette, TooltipPosition } from '@angular/material';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.scss']
})
export class ButtonComponent implements OnInit {
  //#region Button
  /**
   * CSS class(es) applied to the button
   * @default 'btn btn-primary waves-light'
   * @todo `btn-albie` should not exist. It should be `btn-primary`, as it only changes the color.
   */
  @Input() cssClass = 'btn btn-albie waves-light';
  /**
   * Wether an asynchronus action is being performed.
   * Setting `isLoading` to `true` will disable the button and make the spinner appear.
   * @default false
   */
  @Input() set isLoading(value: boolean) {
    if (value == null) { value = false; }
    if (value === this.isLoading) { return; } // Avoid flickering
    this._isLoading = value;
  }
  get isLoading() { return this._isLoading; }
  protected _isLoading = false;
  /**
   * Automatically set 'isLoading' property to true when the user clicks the button.
   * 
   * User -> click -> isLoading = true -> (onClick)
   */
  @Input() isLoadingOnClick = false;
  /**
   * Wether the button is disabled
   * @default false
   */
  @Input() set disabled(value: boolean) {
    if (value == null) { value = false; }
    if (value === this.disabled) { return; } // Avoid flickering
    this._disabled = value;
  }
  get disabled() { return this._disabled; }
  protected _disabled = false;
  /**
   * Button's `type` attribute.
   * @default 'button'
   */
  @Input() type = 'button';
  /**
   * Text to be displayed in the button
   * @default ''
   */
  @Input() label = '';
  /**
   * `aria-label` attibute. If null, `label` will be used.
   * @default null
   */
  @Input() ariaLabel = null;

  //#region Button Icon
  /**
   * If set, an icon will be displayed next to the button.
   * @default null
   */
  @Input() iconCssClass = null;
  @Input() iconPosition = TemplateButtonIconPosition.left;
  protected _dummyTemplateButtonIconPosition = { ...TemplateButtonIconPosition };
  //#endregion
  //#endregion

  //#region Spinner
  /**
   * Wether the spinner should not appear on loading.
   */
  @Input() spinnerDisabled = false;
  /**
   * CSS class(es) applied to the loading spinner.
   * @default 'd-inline-block align-middle mx-2'
   */
  @Input() spinnerCssClass = 'd-inline-block align-middle mx-2';
  /**
   * Diameter (px) applied to the loading spinner
   * @default 24
   */
  @Input() spinnerDiameter = 24;
  /**
   * `ThemePallete` to be applied to the spinner, as its color.
   * @default 'accent'
   */
  @Input() spinnerColor: ThemePalette = 'accent';
  //#endregion

  //#region Tooltip
  @Input() tooltipLabel = null;
  @Input() tooltipPosition: TooltipPosition = 'above';
  @Input() tooltipCssClass = '';
  //#endregion

  @Output() onClick = new EventEmitter<MouseEvent>();

  constructor() { }

  ngOnInit() {
  }

  click(ev: MouseEvent) {
    this.onClick.next(ev);
  }
}
export enum TemplateButtonIconPosition {
  'left',
  'right',
  'top',
  'bottom'
}
