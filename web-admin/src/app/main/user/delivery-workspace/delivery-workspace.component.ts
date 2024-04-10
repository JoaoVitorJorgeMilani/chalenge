import { Component, OnInit, ViewChild } from '@angular/core';
import { AlertComponent } from 'src/app/shared/alert/alert.component';
import { AuthService } from 'src/app/shared/auth/auth.service';
import { ModalComponent } from 'src/app/shared/modal/modal.component';
import { CustomCurrencyPipe } from 'src/app/shared/pipes/custom-currency.pipe';
import { SignalRService } from '../stream/signalr.service';
import { DeliveryService } from './delivery.service';


@Component({
  selector: 'app-delivery-workspace',
  templateUrl: './delivery-workspace.component.html',
  styles: [`.nav-link {
    cursor: pointer;
  }`]
})
export class DeliveryWorkspaceComponent implements OnInit {

  @ViewChild(ModalComponent) modal!: ModalComponent;
  @ViewChild(AlertComponent) alert!: AlertComponent;

  customCurrencyPipe = new CustomCurrencyPipe();

  activeTab: string = 'orders';
  deliveringOrder: any = null;

  orderNotifications: any[] = [];
  notificationOnShowing: any;

  constructor(private streamService: SignalRService, private service: DeliveryService, private authService: AuthService) { }

  ngOnInit(): void {
    this.streamService.streaming.subscribe(message => this.onStreamMessage(message));
    this.loadUserData();
  }

  activateTab(tab: string) {
    this.activeTab = tab;
  }

  onDeliveryAccepted(event: any) {
    this.deliveringOrder = event;
    this.activeTab = 'running';
  }

  onStreamMessage(event: any) {
    this.orderNotifications.push({
      encryptedId: event.encryptedId,
      title: "Nova entrega disponível!",
      message: `
      O pedido <strong>${event.identifier}</strong> esta disponível para entrega.
      Com o valor de <strong>${this.customCurrencyPipe.transform(event.fare)}</strong>,
      Clique no botão abaixo para entregar o pedido.
    `,
      showed: false

    });

    this.showRemainNotifications();

  }

  showRemainNotifications() {
    if (this.hasNotificationsToShow()) {
      if (!this.modal.isOpen) {
        this.orderNotifications.forEach(notification => {
          if (!notification.showed) {
            notification.showed = true;
            this.notificationOnShowing = notification;
            this.modal.message = this.notificationOnShowing.message;
            this.modal.title = this.notificationOnShowing.title;
            this.modal.open();
            return;
          }
        });
      }
    }
  }

  orderNotificationAccept() {
    this.service.acceptDelivery(this.notificationOnShowing).subscribe(
      {
        next: (data) => {
          this.onDeliveryAccepted(data);
        },
        error: (error) => {
          this.alert.addErrorMessage("Erro ao aceitar o pedido");
          this.alert.addErrorMessage(error.error);
        }
      }
    )

    this.showRemainNotifications();
  }

  orderNotificationDecline() {
    this.showRemainNotifications();
  }

  hasNotificationsToShow(): boolean {
    if (this.orderNotifications.length > 0) {
      for (let notification of this.orderNotifications) {
        if (!notification.showed)
          return true;
      }
    }
    return false;
  }

  loadUserData() {
    var user = this.authService.getLogedUser();
    if (user.status == 'Delivering') {
      this.service.loadDelivering(user.deliveringOrder).subscribe(
        {
          next: (data) => {
            this.deliveringOrder = data;
            this.activeTab = 'running';
          },
          error: (error) => {
            console.log(error);
          }
        }
      )
    }
  }

  onDeliveryDeclined() {
    this.activeTab = 'orders';
    this.deliveringOrder = null;
  }

  isDelivering(): boolean {
    return this.deliveringOrder != null;
  }
}
