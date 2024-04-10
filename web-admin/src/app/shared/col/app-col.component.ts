import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-col',
  template: `
    <div [class]='classes' [style]="customStyles" >
      <ng-content></ng-content>
    </div> 
  `,
  styleUrls: ['./app-col.component.css']


  
})
export class ColComponent {
  @Input() col_span : string = '';
  @Input() customClasses: string = '';
  @Input() customStyles: string = '';

  getColumnClasses(){
    const [sm = '12', md = sm, lg = md] = this.col_span.split(' ').map(num => `${num}`).slice(0, 3);
    return `col-sm-${sm} col-md-${md} col-lg-${lg}`;
  }

  get classes(){
    return (this.customClasses) ? this.customClasses.concat(" ", this.getColumnClasses()) : this.getColumnClasses();
  }
}
