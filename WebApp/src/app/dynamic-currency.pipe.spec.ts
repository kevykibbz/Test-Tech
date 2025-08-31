import { LOCALE_ID, ChangeDetectorRef } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { CurrencyService } from './currency.service';
import { DynamicCurrencyPipe } from './dynamic-currency.pipe';
import { httpTestProviders } from '../test-helpers';

describe('DynamicCurrencyPipe', () => {
  it('create an instance', () => {
    // Arrange: configure the testing module and inject dependencies
    TestBed.configureTestingModule({
      providers: [
        ...httpTestProviders,
        { provide: LOCALE_ID, useValue: 'en-US' },
        ChangeDetectorRef,
        CurrencyService
      ]
    });
    const locale = TestBed.inject(LOCALE_ID);
    const changeDetectorRef = TestBed.inject(ChangeDetectorRef);
    const currencyService = TestBed.inject(CurrencyService);

    // Act: create the pipe instance with dependencies
    const pipe = new DynamicCurrencyPipe(locale, changeDetectorRef, currencyService);
    expect(pipe).toBeTruthy();
  });
});
