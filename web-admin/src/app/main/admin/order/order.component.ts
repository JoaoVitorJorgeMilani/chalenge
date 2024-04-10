import { Component } from '@angular/core';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']

})
export class OrderComponent {
  activeTab: string = 'list';
  orderDetails: any = null;

  activateTab(tab: string) {
    if (tab === 'detail' && !this.orderDetails) {
      return;
    }

    this.activeTab = tab;
  }

  get showDetail(): boolean { return this.activeTab === 'detail' && this.orderDetails; };

  onOrderDetails(order: any) {
    this.orderDetails = order;
    this.activateTab('detail');
  }


}
