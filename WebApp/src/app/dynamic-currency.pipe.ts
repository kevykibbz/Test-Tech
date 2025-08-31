import { CurrencyPipe } from '@angular/common';
import { ChangeDetectorRef, Inject, LOCALE_ID, OnDestroy, Pipe, PipeTransform } from '@angular/core';
import { AsyncSubject, Subscription } from 'rxjs';

import { CurrencyService } from './currency.service';

const NO_VALUE = '-';

@Pipe({
  name: 'dynamicCurrency',
  pure: false,
})
export class DynamicCurrencyPipe implements OnDestroy, PipeTransform {
  private destroy$ = new AsyncSubject();
  private latestInputValue?: any;
  private latestCurrencyInputValue?: any;
  private latestValue?: any;
  private pendingData?: Subscription;

  constructor(
    @Inject(LOCALE_ID) private locale: string,
    private changeDetectionRef: ChangeDetectorRef,
    private currencyService: CurrencyService,
  ) {
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  transform(value: number | null, currencyCode: any): unknown {
    if (!value) {
      return NO_VALUE;
    }

    if (!currencyCode) {
      return `${value}`;
    }

    if (!this.pendingData) {
      this.latestInputValue = value;
      this.latestCurrencyInputValue = currencyCode;

      this.latestValue = (new CurrencyPipe(this.locale)).transform(value, '');
      this.pendingData = this.currencyService.getCurrency(currencyCode)
        .subscribe(currency => {
          if (currency) {
            this.latestValue = (new CurrencyPipe(this.locale)).transform(value, currency.symbol);
          }
          this.changeDetectionRef.markForCheck();
        });

      return this.latestValue;
    }

    if (this.latestInputValue !== value || this.latestCurrencyInputValue !== currencyCode) {
      if (this.pendingData) {
        this.pendingData.unsubscribe();
        this.pendingData = undefined;
        this.transform(value, currencyCode);
      }
    }

    return this.latestValue;
  }
}
