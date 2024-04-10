import { Component } from '@angular/core';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
})
export class AlertComponent {

  errorMessages: string[] = [];
  successMessages: string[] = [];
  get showError() : boolean { return this.errorMessages.length > 0 };
  get showSuccess() : boolean { return this.successMessages.length > 0 };
  

  constructor() { }

  clear() {
    this.errorMessages = [];
    this.successMessages = [];
  }

  addErrorMessage(message: string) {
    this.errorMessages.push(message)
  }

  addSuccessMessage(message: string) {
    this.successMessages.push(message)
  }

}
