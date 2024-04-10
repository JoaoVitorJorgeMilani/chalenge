import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-spinner',
  template: `
    <div *ngIf="loading" class="spinner-overlay">
      <div class="spinner"></div>
    </div>
  `,
  styleUrls: ['./spinner.component.css']
})
export class SpinnerComponent {

  @Input() loading: boolean = false;

  constructor() { }
}
