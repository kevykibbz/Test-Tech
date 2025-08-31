import { Component, OnInit, signal, computed } from '@angular/core';
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
  // State signals
  currencies = signal<Currency[]>([]);
  selectedCurrency = signal<Currency | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  // Computed signals
  hasCurrencies = computed(() => this.currencies().length > 0);
  hasSelection = computed(() => !!this.selectedCurrency());

  constructor(private currencyService: CurrencyService) {}

  ngOnInit(): void {
    this.loadCurrencies();
  }

  loadCurrencies(): void {
    this.loading.set(true);
    this.error.set(null);
    
    this.currencyService.getCurrencies().subscribe({
      next: (currencies) => {
        this.currencies.set(currencies);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load currencies');
        this.loading.set(false);
      }
    });
  }

  onCurrencySelect(currency: Currency): void {
    this.selectedCurrency.set(currency);
  }

  onRefresh(): void {
    this.selectedCurrency.set(null);
    this.loadCurrencies();
  }

  trackById(index: number, currency: Currency): string {
    return currency.id;
  }
}
