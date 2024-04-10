import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-display-field',
  template: `
    <app-grid>
      <div class="form-group">
        <app-row>
          <app-col col_span="4">
            <label for="displayField" 
              class="col-form-label fs-7 fw-bold fst-italic" 
              [ngStyle]="{ 'max-width': '100%', 'white-space': 'pre-wrap', 'word-wrap': 'break-word' }">
                {{ labelText }}:
            </label>
          </app-col>
          <app-col col_span="8">
            <input type="text" readonly class="form-control-plaintext" id="displayField" [value]='value'>
          </app-col>
        </app-row>
        </div>
    </app-grid>
  `
})
export class DisplayFieldComponent {

  @Input() labelText: string = '';
  @Input() value: string  = '';

}
