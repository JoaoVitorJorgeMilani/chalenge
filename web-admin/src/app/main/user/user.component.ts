import { Component } from '@angular/core';

@Component({
  selector: 'user-client',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent {
  constructor() { }

  sidebarOptions = [
    {
      name: 'SignUp', 
      route: 'users_signup' ,
      icon: 'fa fa-pencil-square-o'
    },
    {
      name: 'SignIn', 
      route: 'users_signin' ,
      icon: 'fa fa-rocket'
    }
  ];

  ngOnInit(): void {
    
  }
}
