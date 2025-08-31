import { Component } from '@angular/core';
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
  activeTab: 'default' | 'text' | 'file' = 'default';
  contractText = '';
  selectedFile: File | null = null;
  result: ContractExtractionResult | null = null;
  loading = false;
  error: string | null = null;

  constructor(private contractExtractionService: ContractExtractionService) {}

  onTabChange(tab: 'default' | 'text' | 'file'): void {
    this.activeTab = tab;
    this.clearResults();
  }

  onExtractDefault(): void {
    this.loading = true;
    this.error = null;
    this.result = null;

    this.contractExtractionService.extractFromDefaultContract().subscribe({
      next: (result) => {
        this.result = result;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to extract from default contract: ' + (error.error?.message || error.message);
        this.loading = false;
      }
    });
  }

  onExtractFromText(): void {
    if (!this.contractText.trim()) {
      this.error = 'Please enter contract text';
      return;
    }

    this.loading = true;
    this.error = null;
    this.result = null;

    this.contractExtractionService.extractFromText(this.contractText).subscribe({
      next: (result) => {
        this.result = result;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to extract from text: ' + (error.error?.message || error.message);
        this.loading = false;
      }
    });
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      this.error = null;
    }
  }

  onExtractFromFile(): void {
    if (!this.selectedFile) {
      this.error = 'Please select a file';
      return;
    }

    this.loading = true;
    this.error = null;
    this.result = null;

    this.contractExtractionService.extractFromFile(this.selectedFile).subscribe({
      next: (result) => {
        this.result = result;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to extract from file: ' + (error.error?.message || error.message);
        this.loading = false;
      }
    });
  }

  private clearResults(): void {
    this.result = null;
    this.error = null;
    this.contractText = '';
    this.selectedFile = null;
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
