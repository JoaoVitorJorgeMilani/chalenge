import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'catalog',
  templateUrl: './catalog.component.html',
  styleUrls: ['./catalog.component.css']
})
export class CatalogComponent {
  
  activeTab: string = 'list';

  activateTab(tab: string){
    this.activeTab = tab;
  }
}
