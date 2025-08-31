import { Component, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ContractExtractionService } from '../contract-extraction.service';
import { ContractExtractionResult } from '../contract-extraction-result';

@Component({
  selector: 'app-contract-extraction',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './contract-extraction.component.html',
  styleUrls: ['./contract-extraction.component.scss']
})
export class ContractExtractionComponent {
  // Signals for reactive state management
  activeTab = signal<'default' | 'text' | 'file'>('default');
  contractText = signal('');
  selectedFile = signal<File | null>(null);
  result = signal<ContractExtractionResult | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  // Computed signals for derived state
  hasResults = computed(() => this.result() !== null);
  hasError = computed(() => this.error() !== null);
  isValidText = computed(() => this.contractText().trim().length > 0);
  isValidFile = computed(() => this.selectedFile() !== null);

  constructor(private contractExtractionService: ContractExtractionService) {}

  onTabChange(tab: 'default' | 'text' | 'file'): void {
    this.activeTab.set(tab);
    this.clearResults();
  }

  onExtractDefault(): void {
    this.loading.set(true);
    this.error.set(null);
    this.result.set(null);

    this.contractExtractionService.extractFromDefaultContract().subscribe({
      next: (result) => {
        this.result.set(result);
        this.loading.set(false);
      },
      error: (error) => {
        this.error.set('Failed to extract from default contract: ' + (error.error?.message || error.message));
        this.loading.set(false);
      }
    });
  }

  onExtractFromText(): void {
    if (!this.isValidText()) {
      this.error.set('Please enter contract text');
      return;
    }

    this.loading.set(true);
    this.error.set(null);
    this.result.set(null);

    this.contractExtractionService.extractFromText(this.contractText()).subscribe({
      next: (result) => {
        this.result.set(result);
        this.loading.set(false);
      },
      error: (error) => {
        this.error.set('Failed to extract from text: ' + (error.error?.message || error.message));
        this.loading.set(false);
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile.set(file);
      this.error.set(null);
    }
  }

  onExtractFromFile(): void {
    const file = this.selectedFile();
    if (!file) {
      this.error.set('Please select a file');
      return;
    }

    this.loading.set(true);
    this.error.set(null);
    this.result.set(null);

    this.contractExtractionService.extractFromFile(file).subscribe({
      next: (result) => {
        this.result.set(result);
        this.loading.set(false);
      },
      error: (error) => {
        this.error.set('Failed to extract from file: ' + (error.error?.message || error.message));
        this.loading.set(false);
      }
    });
  }

  private clearResults(): void {
    this.result.set(null);
    this.error.set(null);
    this.contractText.set('');
    this.selectedFile.set(null);
  }

  formatDate(dateString: string | null): string {
    if (!dateString) return 'Not specified';
    try {
      return new Date(dateString).toLocaleDateString();
    } catch {
      return dateString;
    }
  }

  formatValue(value: number | null): string {
    if (value === null || value === undefined) return 'Not specified';
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD'
    }).format(value);
  }
}
