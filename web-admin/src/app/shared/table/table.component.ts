import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent  {
  
  @Input() title: string = '';
  @Input() headers: string[] = [];
  @Input() property: string[] = [];
  @Input() data: any[] = [];
  @Input() customActionIcon: string = 'fa fa-question-circle-o';
  @Input() customActionClass: string = 'btn btn-primary';
  @Input() customActionText: string = '';
  @Input() enableActionView: boolean = false;
  @Input() enableActionEdit: boolean = false;
  @Input() enableActionDelete: boolean = false;
  @Input() enableCustomAction: boolean = false;
  

  @Output() deleteItem = new EventEmitter<any>();
  @Output() editItem = new EventEmitter<any>();
  @Output() viewItem = new EventEmitter<any>();
  @Output() customAction = new EventEmitter<any>();

  get enableActions() : boolean {
    return this.enableActionView || this.enableActionDelete || this.enableActionEdit || this.enableCustomAction;
  }

  get hasTitle() : boolean { return this.title.length > 0; }

  isCurrencyProperty(prop: string): boolean {
    return prop.includes('| customCurrency');
  }

  isDateProperty(prop: string): boolean {
    return prop.includes('| customDate');
  }

  hasNoProperty(prop: string): boolean {
    return !prop.includes('|');
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

  onView(item: any) {
    this.viewItem.emit(item);
  }

  onCustom(item: any) {
    this.customAction.emit(item);
  }

}
