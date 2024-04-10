import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-grid',
  template: `
    <div class='container-fluid {{customClasses}}' [style]="customStyles" >
      <ng-content></ng-content>
    <div>
  `
})
export class GridComponent {
  @Input() customClasses : string = "";
  @Input() customStyles : string = "";

}
