import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './sidebar/sidebar.component';
import { RouterModule } from '@angular/router';
import { RowComponent } from './row/app-row.component';
import { GridComponent } from './grid/app-grid.component';
import { TableComponent } from './table/table.component';
import { InputComponent } from './input/input.component';
import { ColComponent } from './col/app-col.component';
import { FormsModule } from '@angular/forms';
import { CurrencyComponent } from './currency/currency.component';
import { NgxMaskDirective, NgxMaskPipe, provideNgxMask } from 'ngx-mask';
import { CustomCurrencyPipe } from './custom-currency.pipe';
import { DatepickerComponent } from './datepicker/datepicker.component';
import { AuthGuard } from './auth/authguard';


@NgModule({
  declarations: [
    SidebarComponent,
    RowComponent,
    ColComponent,
    GridComponent,
    TableComponent,
    InputComponent,
    CurrencyComponent,
    CustomCurrencyPipe,
    DatepickerComponent
    
  ],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,
    NgxMaskDirective,
    NgxMaskPipe
  ],
  exports: [
    SidebarComponent,
    RowComponent,
    ColComponent,
    GridComponent,
    TableComponent,
    InputComponent,
    CurrencyComponent,
    CustomCurrencyPipe,
    DatepickerComponent
  ],
  providers : [
    provideNgxMask(),
    AuthGuard
  ]
})
export class SharedModule { }
