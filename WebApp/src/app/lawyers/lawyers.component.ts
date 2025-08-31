import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs';
import { Lawyer } from '../lawyer';
import { LawyerService, LawyerCreateRequest } from '../lawyer.service';

@Component({
  selector: 'app-lawyers',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './lawyers.component.html',
  styleUrls: ['./lawyers.component.scss']
})
export class LawyersComponent implements OnInit {
  // State signals
  lawyers = signal<Lawyer[]>([]);
  selectedLawyer = signal<Lawyer | null>(null);
  showCreateForm = signal(false);
  showEditForm = signal(false);
  loading = signal(false);
  error = signal<string | null>(null);

  // Form data signals
  newLawyer = signal<LawyerCreateRequest>({
    firstName: '',
    lastName: '',
    companyName: ''
  });

  editLawyer = signal<LawyerCreateRequest>({
    firstName: '',
    lastName: '',
    companyName: ''
  });

  // Computed signals
  hasLawyers = computed(() => this.lawyers().length > 0);
  hasSelection = computed(() => !!this.selectedLawyer());
  showingForm = computed(() => this.showCreateForm() || this.showEditForm());
  isValidNewLawyer = computed(() => {
    const lawyer = this.newLawyer();
    return lawyer.firstName.trim() && lawyer.lastName.trim();
  });
  isValidEditLawyer = computed(() => {
    const lawyer = this.editLawyer();
    return lawyer.firstName.trim() && lawyer.lastName.trim();
  });

  constructor(private lawyerService: LawyerService) {}

  ngOnInit(): void {
    this.loadLawyers();
  }

  loadLawyers(): void {
    this.loading.set(true);
    this.error.set(null);
    
    this.lawyerService.getLawyers().subscribe({
      next: (lawyers) => {
        this.lawyers.set(lawyers);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load lawyers');
        this.loading.set(false);
      }
    });
  }

  // Form update methods for template binding
  updateNewLawyerFirstName(value: string): void {
    this.newLawyer.update(lawyer => ({ ...lawyer, firstName: value }));
  }

  updateNewLawyerLastName(value: string): void {
    this.newLawyer.update(lawyer => ({ ...lawyer, lastName: value }));
  }

  updateNewLawyerCompanyName(value: string): void {
    this.newLawyer.update(lawyer => ({ ...lawyer, companyName: value }));
  }

  updateEditLawyerFirstName(value: string): void {
    this.editLawyer.update(lawyer => ({ ...lawyer, firstName: value }));
  }

  updateEditLawyerLastName(value: string): void {
    this.editLawyer.update(lawyer => ({ ...lawyer, lastName: value }));
  }

  updateEditLawyerCompanyName(value: string): void {
    this.editLawyer.update(lawyer => ({ ...lawyer, companyName: value }));
  }

  onLawyerSelect(lawyer: Lawyer): void {
    this.hideAllForms();
    // Load detailed lawyer information including assigned matters
    this.lawyerService.getLawyer(lawyer.id).subscribe({
      next: (detailedLawyer) => {
        this.selectedLawyer.set(detailedLawyer);
      },
      error: (error) => {
        console.error('Error loading lawyer details:', error);
        this.selectedLawyer.set(lawyer); // Fall back to basic info
      }
    });
  }

  onRefresh(): void {
    this.selectedLawyer.set(null);
    this.hideAllForms();
    this.loadLawyers();
  }

  onShowCreateForm(): void {
    this.hideAllForms();
    this.showCreateForm.set(true);
    this.resetNewLawyerForm();
  }

  onShowEditForm(): void {
    const selected = this.selectedLawyer();
    if (!selected) return;
    this.hideAllForms();
    this.showEditForm.set(true);
    this.editLawyer.set({
      firstName: selected.firstName,
      lastName: selected.lastName,
      companyName: selected.companyName
    });
  }

  onCreateLawyer(): void {
    if (!this.isValidNewLawyer()) {
      this.error.set('Please fill in all required fields');
      return;
    }

    this.loading.set(true);
    this.error.set(null);

    this.lawyerService.createLawyer(this.newLawyer()).subscribe({
      next: (lawyer) => {
        this.hideAllForms();
        this.resetNewLawyerForm();
        this.loadLawyers();
        this.selectedLawyer.set(lawyer);
        this.loading.set(false);
      },
      error: (error) => {
        this.error.set('Failed to create lawyer: ' + (error.error?.message || error.message));
        this.loading.set(false);
      }
    });
  }

  onUpdateLawyer(): void {
    const selected = this.selectedLawyer();
    if (!selected || !this.isValidEditLawyer()) {
      this.error.set('Please fill in all required fields');
      return;
    }

    this.loading.set(true);
    this.error.set(null);

    this.lawyerService.updateLawyer(selected.id, this.editLawyer()).subscribe({
      next: (lawyer) => {
        this.hideAllForms();
        this.loadLawyers();
        this.selectedLawyer.set(lawyer);
        this.loading.set(false);
      },
      error: (error) => {
        this.error.set('Failed to update lawyer: ' + (error.error?.message || error.message));
        this.loading.set(false);
      }
    });
  }

  onDeleteLawyer(): void {
    const selected = this.selectedLawyer();
    if (!selected) return;

    if (!confirm(`Are you sure you want to delete ${this.getFullName(selected)}?`)) {
      return;
    }

    this.loading.set(true);
    this.error.set(null);

    this.lawyerService.deleteLawyer(selected.id).subscribe({
      next: () => {
        this.selectedLawyer.set(null);
        this.hideAllForms();
        this.loadLawyers();
        this.loading.set(false);
      },
      error: (error) => {
        this.error.set('Failed to delete lawyer: ' + (error.error?.message || error.message));
        this.loading.set(false);
      }
    });
  }

  onCancelForm(): void {
    this.hideAllForms();
    this.resetNewLawyerForm();
    this.error.set(null);
  }

  private hideAllForms(): void {
    this.showCreateForm.set(false);
    this.showEditForm.set(false);
  }

  private resetNewLawyerForm(): void {
    this.newLawyer.set({
      firstName: '',
      lastName: '',
      companyName: ''
    });
  }

  private isValidLawyer(lawyer: LawyerCreateRequest): boolean {
    return lawyer.firstName.trim() !== '' && 
           lawyer.lastName.trim() !== '' && 
           lawyer.companyName.trim() !== '';
  }

  getFullName(lawyer: Lawyer): string {
    return `${lawyer.firstName} ${lawyer.lastName}`;
  }

  trackById(index: number, lawyer: Lawyer): string {
    return lawyer.id;
  }

  trackMatterById(index: number, matter: any): string {
    return matter.id || index.toString();
  }
}
