import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { CatalogService } from '../catalog.service';

@Component({
  selector: 'app-catalog-list',
  templateUrl: './catalog-list.component.html'
})
export class CatalogListComponent implements OnInit {
  
  data: any[] = [];

  constructor(private router: Router, private service : CatalogService){}
  
  errorMessages: string[] = [];
  
  get showError() : boolean { return this.errorMessages.length > 0 };
  showSuccess : boolean = false;
  sucessMessage: string = "";
  isEditing: boolean = false;
  bikeEdit : any = {};


  ngOnInit(): void {
    this.loadRefreshList();
  }

  loadRefreshList(){
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
    this.service.delete(item).subscribe(
      {
        next: data => {
          this.errorMessages = [];
          this.showSuccess = true;
          this.sucessMessage = "Item excluído com sucesso!";
          this.loadRefreshList();
        },
        error: error => {
          this.errorMessages.push(error.error);
          this.loadRefreshList();
        }
      }
    );
  }

  editItem(item: any) {
    this.bikeEdit = item;
    this.isEditing = true;

    // this.service.delete(item).subscribe(
    //   {
    //     next: data => {
    //       this.errorMessages = [];
    //       this.showSuccess = true;
    //       this.sucessMessage = "Item excluído com sucesso!";
    //       this.loadRefreshList();
    //     },
    //     error: error => {
    //       this.errorMessages.push(error.error);
    //       this.loadRefreshList();
    //     }
    //   }
    // );

  }

  onCloseEdit(){
    this.isEditing = false;
  }


}
