import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UsersManagementService } from '../users-management.service';

@Component({
  selector: 'app-users-management-list',
  templateUrl: './users-management-list.component.html',
  styleUrls: ['./users-management-list.component.css']
})
export class UsersManagementListComponent {
  data: any[] = [];
  errorMessages: string[] = [];
  
  get showError() : boolean { return this.errorMessages.length > 0 };
  showSuccess : boolean = false;
  sucessMessage: string = "";
  isEditing: boolean = false;

  constructor(private router: Router, private service : UsersManagementService){}

  ngOnInit(): void {
    this.loadRefreshList();
  }

  loadRefreshList(){
    this.service.getList().subscribe(
      { 
        next: data => {
          this.data = data;
        },
        error: error => {
          console.log(error);
        }
      }
    );
  }
  
}
