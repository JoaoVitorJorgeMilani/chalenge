import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'customDate'
})
export class CustomDatePipe implements PipeTransform {

  transform(value: string | number | Date, format: string = 'short'): string  {
    const datePipe: DatePipe = new DatePipe('pt-BR');
    return datePipe.transform(value, format) ?? '';
  }

}
