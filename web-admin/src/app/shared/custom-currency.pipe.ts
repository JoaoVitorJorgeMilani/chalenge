import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'customCurrency'
})
export class CustomCurrencyPipe implements PipeTransform {

  transform(value: any, currencyCode: string = 'BRL', symbolDisplay: 'code' | 'symbol' | 'symbol-narrow' | 'name' = 'symbol', locale: string = 'pt-BR'): string | null {
    if (typeof value !== 'number') return value;
    return value.toLocaleString(locale, { style: 'currency', currency: currencyCode, currencyDisplay: symbolDisplay });
  }

}
