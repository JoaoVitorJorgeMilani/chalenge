import { Component, ElementRef, Input, ViewChild, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-currency',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CurrencyComponent),
      multi: true
    },
  ],  
  template: `
    <div style="padding: 10px 10px 10px 0px">
      <label [for]='id' class="mb-1">{{labelText}}</label>
        <input 
          #inputField
          (input)="onInput($event)"
          [id]='id' 
          [name]='name' 
          [placeholder]='placeholder' 
          [disabled]='disabled' 
          mask="separator.2"
          class="form-control" 
          [thousandSeparator]="'.'"
          [decimalMarker]="','"
          prefix="R$ "
          type="text"
          [dropSpecialCharacters]="false"         
        >
    </div>
  `
})
export class CurrencyComponent {
  @ViewChild('inputField') inputField!: ElementRef;

  @Input() labelText: string = '';
  @Input() id: string = '';
  @Input() name: string = '';
  @Input() placeholder: string = '';
  @Input() disabled: boolean = false;

  private onChange!: (value: any) => void;

  onInput(event: any): void {
    // Remove pontos dos milhares
    var sanitizedValue = event.target.value.replace(/[^\d,]/g, '')
    
    //troca a virgula dos decimais por ponto (para o parseFloat funcionar)
    var sanitizedValue = sanitizedValue.replace(',', '.')

    this.onChange(parseFloat(sanitizedValue).toFixed(2))
  }

  constructor() {
  }

  writeValue(value: any): void {
    this.inputField.nativeElement.value = value;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
  }

  setDisabledState(isDisabled: boolean): void {
    this.inputField.nativeElement.disabled = isDisabled;
  }
}