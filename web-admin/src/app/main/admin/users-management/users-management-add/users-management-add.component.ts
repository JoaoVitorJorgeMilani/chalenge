import { Component } from '@angular/core';
import { UsersManagementService } from '../users-management.service';

@Component({
  selector: 'app-users-management-add',
  templateUrl: './users-management-add.component.html',
  styleUrls: ['./users-management-add.component.css']
})
export class UsersManagementAddComponent {
  
  errorMessages: string[] = [];
  
  get showError() : boolean { return this.errorMessages.length > 0 };
  showSuccess : boolean = false;
  
  user = {
    name: '',
    cnpj: '',
    cnh: '',
    birthday: '',
  }

  constructor(private service: UsersManagementService){}

  onSubmit(){
    if(this.validate()){
      this.service.add(this.user).subscribe(
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
    
    if(!this.user.name || this.user.name.length < 10) {
      this.errorMessages.push('O nome é obrigatório');
    }

    if(!this.user.cnpj || this.user.cnpj.length < 4) {
      this.errorMessages.push('O cnpj é obrigatório');
    }

    if(!this.user.cnh || this.user.cnh.length < 5) {
      this.errorMessages.push('O número da cng é obrigatório');
    }

    if(!this.user.birthday) {
      this.errorMessages.push('A data de nascimento é obrigatória');
    }
    return this.errorMessages.length == 0;
  }
}
