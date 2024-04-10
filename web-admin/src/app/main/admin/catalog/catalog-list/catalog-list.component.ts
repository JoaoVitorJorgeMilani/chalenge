import { AfterViewInit, Component, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AlertComponent } from 'src/app/shared/alert/alert.component';
import { CatalogService } from '../catalog.service';

@Component({
  selector: 'app-catalog-list',
  templateUrl: './catalog-list.component.html'
})
export class CatalogListComponent implements AfterViewInit {

  @ViewChild('alert') alert!: AlertComponent;
  
  isEditing : boolean = false;
  bikeEdit : any = {};
  data: any[] = [];

  constructor(private router: Router, private service : CatalogService){}


  ngAfterViewInit(): void {
    this.loadRefreshList();
  }

  loadRefreshList(){
    this.alert.clear();
    this.service.getList().subscribe(
      { 
        next: data => {
          this.data = data;
        },
        error: error => {
          console.log(error);
        }
      }
    );
  }
  
  addBike(){
    this.router.navigate(["add"]);
  }

  deleteItem(item: any) {
    this.alert.clear();
    this.service.delete(item).subscribe(
      {
        next: data => {
          this.alert.clear();
          this.alert.addSuccessMessage("Item excluído com sucesso!");
          this.loadRefreshList();
        },
        error: error => {
          this.alert.clear();
          this.alert.addErrorMessage(error.error);
          this.loadRefreshList();
        }
      }
    );
  }

  editItem(item: any) {
    this.bikeEdit = item;
    this.isEditing = true;
  }

  onCloseEdit(){
    this.isEditing = false;
  }
}
