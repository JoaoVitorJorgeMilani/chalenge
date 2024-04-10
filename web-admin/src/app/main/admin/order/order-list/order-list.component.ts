import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { OrderService } from '../order.service';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent {
  data: any[] = [];
  errorMessages: string[] = [];
  
  get showError() : boolean { return this.errorMessages.length > 0 };
  showSuccess : boolean = false;
  sucessMessage: string = "";
  isEditing: boolean = false;

  constructor(private router: Router, private service : OrderService){}

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
