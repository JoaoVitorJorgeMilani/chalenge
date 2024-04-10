import { Component, ViewChild } from '@angular/core';
import { CatalogService } from '../catalog.service';

@Component({
  selector: 'app-catalog-add',
  templateUrl: './catalog-add.component.html'
})
export class CatalogAddComponent {
  
  constructor(private service : CatalogService){}
  errorMessages: string[] = [];
  
  get showError() : boolean { return this.errorMessages.length > 0 };
  showSuccess : boolean = false;
  
  bike = {
    identifier: '',
    manufacturingYear: '',
    bikeModel: '',
    licensePlate: '',
  }

  onSubmit(){
    if(this.validate()){
      this.service.add(this.bike).subscribe(
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
    
    if(!this.bike.identifier || this.bike.identifier.length < 10) {
      this.errorMessages.push('O identificador é obrigatório');
    }

    if(!this.bike.manufacturingYear || this.bike.manufacturingYear.length < 4) {
      this.errorMessages.push('O ano de fabricação é obrigatório');
    }

    if(!this.bike.bikeModel || this.bike.bikeModel.length < 5) {
      this.errorMessages.push('O modelo é obrigatório');
    }

    if(!this.bike.licensePlate || this.bike.licensePlate.length < 7) {
      this.errorMessages.push('O número da placa é obrigatório');
    }
    return this.errorMessages.length == 0;
  }
}
