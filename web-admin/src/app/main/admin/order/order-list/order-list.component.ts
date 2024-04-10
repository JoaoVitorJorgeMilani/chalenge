import { AfterViewInit, Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AlertComponent } from 'src/app/shared/alert/alert.component';
import { OrderService } from '../order.service';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html'
})
export class OrderListComponent implements AfterViewInit {
  @ViewChild(AlertComponent) alert!: AlertComponent;
  @Output() onOrderDetails = new EventEmitter<any>();


  orders: any[] = [];
  isEditing: boolean = false;

  constructor(private router: Router, private service : OrderService){}

  ngAfterViewInit(): void {
    this.loadRefreshList();
  }

  loadRefreshList(){
    this.alert.clear();
    this.service.getList().subscribe(
      { 
        next: (data) => {
          this.orders = data;
          this.normalizeOrders();
        },
        error: () => {
          this.alert.clear();
          this.alert.addErrorMessage("Erro ao carregar os dados, contate o administrador");
        }
      }
    );
  }

  normalizeOrders() {
    this.orders.forEach((order: any) => {
      order.formattedStatus = this.formatOrderStatus(order.status);
      this.normalizeDeliveries(order.deliveries);
    });
  }

  normalizeDeliveries(_deliveries: any) {
     _deliveries.forEach((delivery: any) => {
      delivery.formattedStatus = this.formatDeliveryStatus(delivery.status);
    })
  }


  formatOrderStatus(status: string) : string {
      if(status == "Available") return "Disponível";
      if(status == "OnDelivery") return "Em entrega";
      if(status == "Delivered") return "Entregue";
      return "Indefinido";
  }

  formatDeliveryStatus(status: string) : string {
    if(status == "OnDelivery") return "Em entrega";
    if(status == "Delivered") return "Entregue";
    if(status == "DeliveryFailed") return "Falhou";
    if(status == "DeliveryDeclined") return "Rejeitado";
    return "Indefinido";
  }  

  onViewingOrder(order: any){
    this.onOrderDetails.emit(order);
  }
}
