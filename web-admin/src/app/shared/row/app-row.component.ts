import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-row',
  template: `
      <div class='row {{customClasses}}' [style]="customStyles">
          <ng-content></ng-content>
      </div>

  `
})
export class RowComponent {
  @Input() customStyles : string = ""; 
  @Input() customClasses : string = "";

}
