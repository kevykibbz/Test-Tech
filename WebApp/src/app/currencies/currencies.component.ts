import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { Currency } from '../currency';
import { CurrencyService } from '../currency.service';

@Component({
  selector: 'app-currencies',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './currencies.component.html',
  styleUrls: ['./currencies.component.scss']
})
export class CurrenciesComponent implements OnInit {
  currencies$!: Observable<Currency[]>;
  selectedCurrency: Currency | null = null;
  loading = false;
  error: string | null = null;

  constructor(private currencyService: CurrencyService) {}

  ngOnInit(): void {
    this.loadCurrencies();
  }

  loadCurrencies(): void {
    this.loading = true;
    this.error = null;
    this.currencies$ = this.currencyService.getCurrencies();
  }

  onCurrencySelect(currency: Currency): void {
    this.selectedCurrency = currency;
  }

  onRefresh(): void {
    this.selectedCurrency = null;
    this.loadCurrencies();
  }

  trackById(index: number, currency: Currency): string {
    return currency.id;
  }
}
