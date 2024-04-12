import { Component } from '@angular/core';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']

})
export class OrderComponent {
  activeTab: string = 'add';

  activateTab(tab: string){
    this.activeTab = tab;
  }
}
