import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent  {
  
  @Input() headers: string[] = [];
  @Input() property: string[] = [];
  @Input() data: any[] = [];
  @Input() enableActionView: boolean = false;
  @Input() enableActionEdit: boolean = false;
  @Input() enableActionDelete: boolean = false;

  @Output() deleteItem = new EventEmitter<any>();
  @Output() editItem = new EventEmitter<any>();

  get enableActions() : boolean {
    return this.enableActionView || this.enableActionDelete || this.enableActionEdit;
  }

  isCurrencyProperty(prop: string): boolean {
    return prop.includes('| customCurrency');
  }

  getProperty(prop: string): string {
    return prop.split('|')[0].trim();
  }

  onDelete(item: any) {
    this.deleteItem.emit(item);
  }

  onEdit(item: any) {
    this.editItem.emit(item);
  }

}
