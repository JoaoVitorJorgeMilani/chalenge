import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from '../app-routing.module';
import { SharedModule } from '../shared/shared.module';
import { AdminComponent } from './admin/admin.component';
import { CatalogAddComponent } from './admin/catalog/catalog-add/catalog-add.component';
import { CatalogEditComponent } from './admin/catalog/catalog-edit/catalog-edit.component';
import { CatalogListComponent } from './admin/catalog/catalog-list/catalog-list.component';
import { CatalogComponent } from './admin/catalog/catalog.component';
import { OrderAddComponent } from './admin/order/order-add/order-add.component';
import { OrderDetailComponent } from './admin/order/order-detail/order-detail.component';
import { OrderListComponent } from './admin/order/order-list/order-list.component';
import { OrderComponent } from './admin/order/order.component';
import { UsersManagementAddComponent } from './admin/users-management/users-management-add/users-management-add.component';
import { UsersManagementListComponent } from './admin/users-management/users-management-list/users-management-list.component';
import { UsersManagementComponent } from './admin/users-management/users-management.component';
import { MainComponent } from './main.component';
import { DeliveryWorkspaceOrdersComponent } from './user/delivery-workspace/delivery-workspace-orders/delivery-workspace-orders.component';
import { DeliveryWorkspaceRunningComponent } from './user/delivery-workspace/delivery-workspace-running/delivery-workspace-running.component';
import { DeliveryWorkspaceComponent } from './user/delivery-workspace/delivery-workspace.component';
import { SignInComponent } from './user/signin/signin.component';
import { SignupComponent } from './user/signup/signup.component';
import { SignalRService } from './user/stream/signalr.service';
import { UserComponent } from './user/user.component';

@NgModule({
  declarations: [
    MainComponent,
    AdminComponent,
    CatalogComponent,
    CatalogAddComponent,
    CatalogListComponent,
    CatalogEditComponent,
    OrderComponent,
    OrderAddComponent,
    OrderListComponent,
    OrderComponent,
    UserComponent,
    SignupComponent,
    UsersManagementComponent,
    UsersManagementAddComponent,
    UsersManagementListComponent,
    SignInComponent,
    DeliveryWorkspaceComponent,
    DeliveryWorkspaceOrdersComponent,
    DeliveryWorkspaceRunningComponent,
    OrderDetailComponent
  ],
  imports: [
    HttpClientModule,
    CommonModule,
    AppRoutingModule,
    BrowserModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [
    SignalRService
  ]

})
export class MainModule { }
