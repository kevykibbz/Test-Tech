import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AsyncSubject, Observable } from 'rxjs';
import { finalize, map } from 'rxjs/operators';

import { Currency } from './currency';

@Injectable({
  providedIn: 'root',
})
export class CurrencyService {
  private currencies$?: AsyncSubject<Currency[]>;

  constructor(private http: HttpClient) {
  }

  getCurrencies(): Observable<Currency[]> {
    if (!this.currencies$) {
      const currencies$ = new AsyncSubject<Currency[]>();
      this.http.get<Currency[]>(`/api/v1/Currencies`)
        .pipe(
          finalize(() => currencies$.complete()),
        )
        .subscribe(
          currencies => currencies$.next(currencies),
          err => {
            console.error('Error loading currencies', err);
            currencies$.next([]);
            this.currencies$ = undefined;
          },
        );

      this.currencies$ = currencies$;
    }

    return this.currencies$;
  }

  getCurrency(code: string): Observable<Currency | undefined> {
    return this.getCurrencies()
      .pipe(
        map(currencies => currencies.find(c => c.id === code)),
      );
  }
}
