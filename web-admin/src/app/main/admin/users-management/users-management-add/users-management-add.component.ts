import { Component, ViewChild } from '@angular/core';
import { AlertComponent } from 'src/app/shared/alert/alert.component';
import { UsersManagementService } from '../users-management.service';

@Component({
  selector: 'app-users-management-add',
  templateUrl: './users-management-add.component.html'
})
export class UsersManagementAddComponent {
  
  @ViewChild(AlertComponent) alert!: AlertComponent;

  user = {
    name: '',
    cnpj: '',
    cnh: '',
    birthday: '',
  }

  constructor(private service: UsersManagementService){}

  onSubmit(){
    this.alert.clear();
    if(this.validate()){
      this.service.add(this.user).subscribe(
        {
          next: () => {
            this.alert.clear();
            this.alert.addSuccessMessage('Salvo com sucesso!');
          },
          error: error => {
            this.alert.addErrorMessage(error.error);
          }
        }
      );
    }   
  }

  validate() : boolean {
    var valid = true;
    
    if(!this.user.name || this.user.name.length <= 0) {
      this.alert.addErrorMessage('O nome é obrigatório');
      valid = false;
    }

    if(!this.user.cnpj || this.user.cnpj.length < 4) {
      this.alert.addErrorMessage('O cnpj é obrigatório');
      valid = false;
    }

    if(!this.user.cnh || this.user.cnh.length < 5) {
      this.alert.addErrorMessage('O número da cng é obrigatório');
      valid = false;
    }

    if(!this.user.birthday) {
      this.alert.addErrorMessage('A data de nascimento é obrigatória');
      valid = false;
    }

    return valid;
  }
}
