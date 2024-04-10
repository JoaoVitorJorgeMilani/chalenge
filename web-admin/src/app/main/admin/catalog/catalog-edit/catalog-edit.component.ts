import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { AlertComponent } from 'src/app/shared/alert/alert.component';
import { CatalogService } from '../catalog.service';

@Component({
  selector: 'app-catalog-edit',
  templateUrl: './catalog-edit.component.html',
})
export class CatalogEditComponent {

  @ViewChild(AlertComponent) alert!: AlertComponent;

  constructor(private service: CatalogService) { }

  @Input() bikeEdit = {
    identifier: '',
    manufacturingYear: '',
    bikeModel: '',
    licensePlate: '',
  };

  @Output() closeEdit = new EventEmitter<any>();

  onBack() {
    this.closeEdit.emit();

  }
  onSubmit() {
    this.alert.clear();
    if (this.validate()) {
      this.service.edit(this.bikeEdit).subscribe(
        {
          next: () => {
            this.alert.clear();
            this.alert.addSuccessMessage('Editado com sucesso!');
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
    this.alert.clear();

    if (!this.bikeEdit.licensePlate || this.bikeEdit.licensePlate.length != 7) {
      this.alert.addErrorMessage('O número da placa com 7 caracteres é obrigatório');
      return false;
    }

    return true;
  }




}
