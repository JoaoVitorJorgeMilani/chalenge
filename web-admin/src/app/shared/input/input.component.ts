import { Component, ElementRef, Input, ViewChild, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { NgxMaskDirective } from 'ngx-mask/lib/ngx-mask.directive';

@Component({
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => InputComponent),
      multi: true
    },
  ],  
  selector: 'app-input',
  template: `
    <div style="padding: 10px 10px 10px 0px">
      <label [for]='id' class="mb-1">{{labelText}}</label>
      <input #inputField (input)="onInput($event)" [type]='type' [maxLength]='maxLength' class="form-control" 
              [id]='id' [name]='name' [placeholder]='placeholder' [disabled]='disabled'  >
    </div>
  `
})
export class InputComponent {

  @ViewChild('inputField') inputField!: ElementRef;

  @Input() labelText: string = '';
  @Input() type: string = '';
  @Input() id: string = '';
  @Input() name: string = '';
  @Input() placeholder: string = '';
  @Input() maxLength: number = 500;
  @Input() disabled: boolean = false;

  private onChange!: (value: any) => void;

  onInput(event: any): void {
    this.onChange(event.target.value);
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
