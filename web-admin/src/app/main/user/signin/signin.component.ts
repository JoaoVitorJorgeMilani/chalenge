import { Component } from '@angular/core';
import { AuthService } from 'src/app/shared/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SignInComponent {

  errorMessages: string[] = [];
  
  get showError() : boolean { return this.errorMessages.length > 0 };
  showSuccess : boolean = false;
  
  user = {
    name: '',
    cnpj: '',
    cnh: '',
    birthday: ''
  };
  
  constructor(private service: AuthService, private router: Router ){}


  onSubmit() {
    this.service.login(this.user).subscribe({
      next: () => {
        this.errorMessages = [];
        this.router.navigateByUrl(
          this.router.url.replace('users_signin', 'user_signed')
        );
      },
      error: (error) => {
        this.errorMessages = [];
        this.errorMessages.push('Falha durante a autenticação, tente inserir o nome de um usuario válido');
      }
    });
  }
}
