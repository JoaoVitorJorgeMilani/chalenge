import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CatalogService } from '../catalog.service';

@Component({
  selector: 'app-catalog-edit',
  templateUrl: './catalog-edit.component.html',
})
export class CatalogEditComponent {
  
  constructor(private service : CatalogService){}
  errorMessages: string[] = [];
  
  get showError() : boolean { return this.errorMessages.length > 0 };
  showSuccess : boolean = false;

  @Input() bikeEdit = {
    identifier: '',
    manufacturingYear: '',
    bikeModel: '',
    licensePlate: '',
  };

  @Output() closeEdit = new EventEmitter<any>();

  onBack(){
    this.closeEdit.emit();

  }
  onSubmit(){
    if(this.validate()){
      this.service.edit(this.bikeEdit).subscribe(
        {
          next: data => {
            this.errorMessages = [];
            this.showSuccess = true;
          },
          error: error => {
            this.errorMessages.push(error.error);
          }
        }
      );
    }   
  }

  validate() : boolean {
    this.errorMessages = [];

    if(!this.bikeEdit.licensePlate || this.bikeEdit.licensePlate.length != 7) {
      this.errorMessages.push('O número da placa com 7 caracteres é obrigatório ');
    }
    return this.errorMessages.length == 0;
  }

  

  
}
