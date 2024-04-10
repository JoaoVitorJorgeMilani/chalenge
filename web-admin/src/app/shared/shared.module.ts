import { LOCALE_ID, NgModule } from '@angular/core';
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
import { CustomCurrencyPipe } from './pipes/custom-currency.pipe';
import { DatepickerComponent } from './datepicker/datepicker.component';
import { AuthGuard } from './auth/authguard';
import { ModalComponent } from './modal/modal.component';
import { SpinnerComponent } from './spinner/spinner.component';
import { AlertComponent } from './alert/alert.component';
import { DisplayFieldComponent } from './display-field/display-field.component';
import { CustomDatePipe } from './pipes/custom-date.pipe';
import localePt from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';

registerLocaleData(localePt);

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
    DatepickerComponent,
    ModalComponent,
    SpinnerComponent,
    AlertComponent,
    DisplayFieldComponent,
    CustomDatePipe
    
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
    DatepickerComponent,
    ModalComponent,
    SpinnerComponent,
    AlertComponent,
    DisplayFieldComponent,
    CustomDatePipe
  ],
  providers : [
    provideNgxMask(),
    AuthGuard,
    { provide: LOCALE_ID, useValue: 'pt-BR' }
  ]
})
export class SharedModule { }
