import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CatalogComponent } from './admin/catalog/catalog.component';
import { AdminComponent } from './admin/admin.component';
import { MainComponent } from './main.component';
import { SharedModule } from '../shared/shared.module';
import { AppRoutingModule } from '../app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import { CatalogAddComponent } from './admin/catalog/catalog-add/catalog-add.component';
import { CatalogListComponent } from './admin/catalog/catalog-list/catalog-list.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CatalogEditComponent } from './admin/catalog/catalog-edit/catalog-edit.component';
import { OrderComponent } from './admin/order/order.component';
import { OrderAddComponent } from './admin/order/order-add/order-add.component';
import { OrderListComponent } from './admin/order/order-list/order-list.component';
import { UserComponent } from './user/user.component';
import { SseComponent } from './user/sse/sse.component';
import { SignupComponent } from './user/signup/signup.component';
import { UsersManagementComponent } from './admin/users-management/users-management.component';
import { UsersManagementAddComponent } from './admin/users-management/users-management-add/users-management-add.component';
import { UsersManagementListComponent } from './admin/users-management/users-management-list/users-management-list.component';
import { SignInComponent } from './user/signin/signin.component';
import { SignedComponent } from './user/signed/signed.component';

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
    SseComponent,
    SignupComponent,
    UsersManagementComponent,
    UsersManagementAddComponent,
    UsersManagementListComponent,
    SignInComponent,
    SignedComponent
    
  ],
  imports: [
    HttpClientModule,
    CommonModule,
    AppRoutingModule,
    BrowserModule,
    SharedModule,
    FormsModule,
    ReactiveFormsModule,
  ]
  
})
export class MainModule { }
