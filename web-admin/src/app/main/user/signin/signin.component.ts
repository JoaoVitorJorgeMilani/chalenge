import { Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AlertComponent } from 'src/app/shared/alert/alert.component';
import { AuthService } from 'src/app/shared/auth/auth.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html'
})
export class SignInComponent {

  @ViewChild(AlertComponent) alert!: AlertComponent;
  
  user = {
    name: '',
    cnpj: '',
    cnh: '',
    birthday: ''
  };
  
  constructor(private service: AuthService, private router: Router ){}


  onSubmit() {
    this.alert.clear();
    this.service.login(this.user).subscribe({
      next: () => {
        this.router.navigateByUrl(
          this.router.url.replace('users_signin', 'delivery_workspace')
        );
      },
      error: () => {
        this.alert.addErrorMessage('Falha durante a autenticação, tente inserir o nome de um usuario válido');
      }
    });
  }
}
