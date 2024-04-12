import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { AdminComponent } from './main/admin/admin.component';
import { CatalogComponent } from './main/admin/catalog/catalog.component';
import { OrderComponent } from './main/admin/order/order.component';
import { UserComponent } from './main/user/user.component';
import { SignupComponent } from './main/user/signup/signup.component';
import { UsersManagementComponent } from './main/admin/users-management/users-management.component';
import { SignInComponent } from './main/user/signin/signin.component';
import { SignedComponent } from './main/user/signed/signed.component';
import { AuthGuard } from './shared/auth/authguard';


const routes: Routes = [
  {
    path: '', component: MainComponent,
    children: [
      {
        path: 'admin',
        component: AdminComponent,
        children: [
          {
            path: 'catalog',
            component: CatalogComponent
          },
          {
            path: 'order',
            component: OrderComponent
          },
          {
            path: 'users_management',
            component: UsersManagementComponent           
          }
         
        ]
      },
      {
        path: 'client',
        component: UserComponent,
        children: [          
            {
              path: 'users_signup',
              component: SignupComponent
            },
            {
              path: 'users_signin',
              component: SignInComponent
            },
            {
              path: 'user_signed',
              component: SignedComponent,
              canActivate: [AuthGuard] 
            }
          ]  
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
