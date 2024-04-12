import { AfterViewInit, Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { AlertComponent } from 'src/app/shared/alert/alert.component';
import { ModalComponent } from 'src/app/shared/modal/modal.component';
import { DeliveryService } from '../delivery.service';
import { AuthService } from 'src/app/shared/auth/auth.service';

@Component({
  selector: 'app-delivery-workspace-orders',
  templateUrl: './delivery-workspace-orders.component.html',
})
export class DeliveryWorkspaceOrdersComponent implements OnInit, AfterViewInit {

  @ViewChild(ModalComponent) modal!: ModalComponent;
  @ViewChild(AlertComponent) alert!: AlertComponent;
  loading: boolean = false;
  data: any[] = [];
  selectedOrder = {}

  @Output() onDeliveryAccepted = new EventEmitter<any>();

  constructor(private service: DeliveryService, private authService: AuthService) { }

  ngOnInit(): void {
    this.loadRefreshList();
  }

  ngAfterViewInit(): void {
  }



  loadRefreshList() {
    this.loading = true;

    this.service.getAvailableOrders().subscribe(
      {
        next: (data) => {
          this.formatOrders(data);
          this.loading = false;
        },
        error: async (error) => {
          this.alert.clear();
          this.alert.addErrorMessage("Erro ao buscar pedidos");
          this.alert.addErrorMessage(error.error);
          this.loading = false;        
        }
      }
    );
  }

  deliveryOrder(event: any) {
    if(event.alreadyTryToDelivery) {
      this.alert.clear();
      this.alert.addErrorMessage("Você já tentou entregar este pedido!");
    } else {
      this.selectedOrder = event;
      this.openModal();
    }
    
  }

  openModal() {
    this.modal.message = "Tem certeza que deseja entregar o pedido? Essa ação não pode ser desfeita.";
    this.modal.title = "Entregar pedido"
    this.modal.open();
  }

  modalOnAccept() {
    this.alert.clear();
    this.loading = true;
    this.service.acceptDelivery(this.selectedOrder).subscribe(
      {
        next: () => {
          this.onDeliveryAccepted.emit(this.selectedOrder);
          this.loading = false;
        },
        error: (error) => {
          this.alert.addErrorMessage("Erro ao aceitar o pedido");
          this.alert.addErrorMessage(error.error);
          this.loading = false;
        }
      }
    )
  }
  
  formatOrders(data: any) {
    this.data = data;
    this.data.forEach((order: any) => {
      order.deliveries.forEach((delivery: any) => {
        if(delivery.encryptedUserId == this.authService.getLogedUser().id) {
          order.alreadyTryToDelivery = true;
        }
      })
    })
  }
}
