import { Component, ViewChild } from '@angular/core';
import { AlertComponent } from 'src/app/shared/alert/alert.component';
import { SignupService } from './signup.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html'
})
export class SignupComponent {

  @ViewChild(AlertComponent) alert!: AlertComponent

  activeTab: string = 'list';
  user = {
    name: '',
    cnpj: '',
    cnh: '',
    birthday: '',
  }

  constructor(private service: SignupService) { }

  onSubmit() {
    if (this.validate()) {
      this.alert.clear();
      this.service.add(this.user).subscribe(
        {
          next: () => {
            this.alert.addSuccessMessage('Salvo com sucesso!');
          },
          error: (error) => {
            this.alert.addErrorMessage(error.error);
          }
        }
      );
    }
  }

  validate(): boolean {
    var valid = true;
    this.alert.clear();

    if (!this.user.name || this.user.name.length < 10) {
      this.alert.addErrorMessage('O nome é obrigatório');
      valid = false;
    }

    if (!this.user.cnpj || this.user.cnpj.length < 4) {
      this.alert.addErrorMessage('O cnpj é obrigatório');
      valid = false;
    }

    if (!this.user.cnh || this.user.cnh.length < 5) {
      this.alert.addErrorMessage('O número da cng é obrigatório');
      valid = false;
    }

    if (!this.user.birthday) {
      this.alert.addErrorMessage('A data de nascimento é obrigatória');
      valid = false;
    }

    return valid;
  }
}
