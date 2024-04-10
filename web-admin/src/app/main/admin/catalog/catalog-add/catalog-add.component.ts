import { Component, ViewChild } from '@angular/core';
import { AlertComponent } from 'src/app/shared/alert/alert.component';
import { CatalogService } from '../catalog.service';

@Component({
  selector: 'app-catalog-add',
  templateUrl: './catalog-add.component.html'
})
export class CatalogAddComponent {

  @ViewChild(AlertComponent) alert!: AlertComponent;

  constructor(private service: CatalogService) { }

  bike = {
    identifier: '',
    manufacturingYear: '',
    bikeModel: '',
    licensePlate: '',
  }

  onSubmit() {
    this.alert.clear();
    if (this.validate()) {
      this.service.add(this.bike).subscribe(
        {
          next: data => {
            this.alert.clear();
            this.alert.addSuccessMessage('Salvo com sucesso!');
          },
          error: error => {
            this.alert.clear();
            this.alert.addErrorMessage(error.error);
          }
        }
      );
    }
  }

  validate(): boolean {

    var valid = true;
    this.alert.clear();

    if (!this.bike.identifier || this.bike.identifier.length <= 0) {
      this.alert.addErrorMessage('O identificador é obrigatório');
      valid = false;
    }

    if (!this.bike.manufacturingYear || this.bike.manufacturingYear.length < 4) {
      this.alert.addErrorMessage('O ano de fabricação é obrigatório');
      valid = false;
    }

    if (!this.bike.bikeModel || this.bike.bikeModel.length < 5) {
      this.alert.addErrorMessage('O modelo é obrigatório');
      valid = false;
    }

    if (!this.bike.licensePlate || this.bike.licensePlate.length < 7) {
      this.alert.addErrorMessage('O número da placa é obrigatório');
      valid = false;
    }

    return valid;
  }
}
