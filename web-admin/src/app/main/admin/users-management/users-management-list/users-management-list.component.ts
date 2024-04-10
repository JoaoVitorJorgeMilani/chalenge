import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AlertComponent } from 'src/app/shared/alert/alert.component';
import { UsersManagementService } from '../users-management.service';

@Component({
  selector: 'app-users-management-list',
  templateUrl: './users-management-list.component.html'
})
export class UsersManagementListComponent implements AfterViewInit {
  
  @ViewChild(AlertComponent) alert!: AlertComponent;

  data: any[] = [];
  isEditing: boolean = false;

  constructor(private router: Router, private service : UsersManagementService){}

  ngAfterViewInit(): void {
    this.loadRefreshList();
  }

  loadRefreshList(){
    this.alert.clear();
    this.service.getList().subscribe(
      { 
        next: (data) => {
          this.data = data;
        },
        error: () => {
          this.alert.clear();
          this.alert.addErrorMessage("Erro ao carregar os dados, contate o administrador");
        }
      }
    );
  }
  
}
