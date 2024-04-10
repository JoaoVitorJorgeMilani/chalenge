import { Component, ViewChild } from '@angular/core';
import { OrderService } from '../order.service';
import { AlertComponent } from 'src/app/shared/alert/alert.component';

@Component({
  selector: 'app-order-add',
  templateUrl: './order-add.component.html',
})
export class OrderAddComponent {

  @ViewChild(AlertComponent) alert!: AlertComponent;

  order = {
    identifier: '',
    fare: 0
  }

  constructor(private service: OrderService) { }

  onSubmit() {
    this.alert.clear();
    if (this.validate()) {
      this.service.add(this.order).subscribe(
        {
          next: () => {
            this.alert.clear();
            this.alert.addSuccessMessage('Salvo com sucesso!');
          },
          error: error => {
            this.alert.clear();
            if (error.error)
              this.alert.addErrorMessage(error.error);
            else
              this.alert.addErrorMessage("Erro ao realizar a operação, contate o administrador");
          }
        }
      );
    }
  }

  validate(): boolean {
    var valid = true;
    this.alert.clear();

    if (!this.order.identifier || this.order.identifier.length <= 0) {
      this.alert.addErrorMessage('O identificador é obrigatório');
      valid = false;
    }

    if (!this.order.fare || isNaN(this.order.fare)) {
      this.alert.addErrorMessage('O valor é obrigatorio');
      valid = false;
    }

    return valid;
  }
}
