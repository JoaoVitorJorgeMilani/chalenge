import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html'
})
export class OrderDetailComponent {

  @Input() order: any;

  get hasDeliveries(): boolean { return this.order && this.order.deliveries && this.order.deliveries.length > 0; }
  get hasNotifications(): boolean { return this.order && this.order.notifications && this.order.notifications.length > 0; }

}
