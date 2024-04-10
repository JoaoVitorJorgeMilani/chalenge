import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { AlertComponent } from 'src/app/shared/alert/alert.component';
import { ModalComponent } from 'src/app/shared/modal/modal.component';
import { DeliveryService } from '../delivery.service';

@Component({
  selector: 'app-delivery-workspace-running',
  templateUrl: './delivery-workspace-running.component.html',
})
export class DeliveryWorkspaceRunningComponent { 
  @ViewChild(ModalComponent) modal!: ModalComponent;
  @ViewChild(AlertComponent) alert!: AlertComponent;
  @Output() onDeliveryDeclined = new EventEmitter<any>();
  
  loading: boolean = false;
  _deliveringOrder: any = null;
  
  @Input()
  set deliveringOrder(value: any) {
    if (value != null) {
      this._deliveringOrder = value;
      this._deliveringOrder.formattedFare = value.fare.toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' });
    } 
  }

  constructor(private service: DeliveryService) { }

  onDecline() {
    this.modal.message = "Tem certeza que deseja cancelar a entrega do pedido? Essa ação não pode ser desfeita.";
    this.modal.title = "Cancelar Entrega"
    this.modal.open();
  }

  modalOnAccept() {
    this.loading = true;
    this.service.declineDelivery(this._deliveringOrder).subscribe(
      {
        next: () => {
          this.alert.addSuccessMessage("Entrega cancelada com sucesso!");
          this.loading = false;
          this._deliveringOrder = null;
          this.onDeliveryDeclined.emit();
        },
        error: (error) => {
          this.alert.addErrorMessage("Não foi possível concluir o cancelamento");
          this.alert.addErrorMessage(error.error);
          this.loading = false;
        }
      }
    )
  }
}
