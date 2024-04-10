import { Component } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
})
export class AdminComponent {
  constructor() {}

  sidebarOptions = [
    {
      name: 'Bike Catalog', 
      route: 'catalog', 
      icon: 'fa fa-motorcycle'
    }, 
    {
      name: 'Orders', 
      route: 'order' ,
      icon: 'fa fa-truck'
    },
    {
      name: 'Users Management', 
      route: 'users_management' ,
      icon: 'fa fa-users'
    }
    

  ];
}
