import { Component } from '@angular/core';
import { OrderService } from '../order.service';

@Component({
  selector: 'app-order-add',
  templateUrl: './order-add.component.html',
})
export class OrderAddComponent {
  
  errorMessages: string[] = [];
  get showError() : boolean { return this.errorMessages.length > 0 };
  showSuccess : boolean = false;

  order = {
    identifier: '',
    fare : 0
  }

  constructor(private service : OrderService){}
  
  onSubmit(){    
    if(this.validate()){
      this.service.add(this.order).subscribe(
        {
          next: data => {
            this.errorMessages = [];
            this.showSuccess = true;
          },
          error: error => {
            console.log("ERROR");
            console.log(error);
            if(error.error)
              this.errorMessages.push(error.error);
            else
              this.errorMessages.push("Erro ao realizar a operação, contate o administrador");
          }
        }
      );
    }   
  }

  validate() : boolean {
    console.log("validating");
    console.log
    this.errorMessages = [];
    
    if(!this.order.identifier || this.order.identifier.length < 10) {
      this.errorMessages.push('O identificador é obrigatório');
    }

    if (!this.order.fare || isNaN(this.order.fare)) {
      this.errorMessages.push('O valor é obrigatorio');
    }

    return this.errorMessages.length == 0;
  }
}
