import { Component, OnInit } from '@angular/core';
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
  lawyers$!: Observable<Lawyer[]>;
  selectedLawyer: Lawyer | null = null;
  showCreateForm = false;
  showEditForm = false;
  loading = false;
  error: string | null = null;

  // Form data
  newLawyer: LawyerCreateRequest = {
    firstName: '',
    lastName: '',
    companyName: ''
  };

  editLawyer: LawyerCreateRequest = {
    firstName: '',
    lastName: '',
    companyName: ''
  };

  constructor(private lawyerService: LawyerService) {}

  ngOnInit(): void {
    this.loadLawyers();
  }

  loadLawyers(): void {
    this.loading = true;
    this.error = null;
    this.lawyers$ = this.lawyerService.getLawyers();
    this.loading = false;
  }

  onLawyerSelect(lawyer: Lawyer): void {
    this.hideAllForms();
    // Load detailed lawyer information including assigned matters
    this.lawyerService.getLawyer(lawyer.id).subscribe({
      next: (detailedLawyer) => {
        this.selectedLawyer = detailedLawyer;
      },
      error: (error) => {
        console.error('Error loading lawyer details:', error);
        this.selectedLawyer = lawyer; // Fall back to basic info
      }
    });
  }

  onRefresh(): void {
    this.selectedLawyer = null;
    this.hideAllForms();
    this.loadLawyers();
  }

  onShowCreateForm(): void {
    this.hideAllForms();
    this.showCreateForm = true;
    this.resetNewLawyerForm();
  }

  onShowEditForm(): void {
    if (!this.selectedLawyer) return;
    this.hideAllForms();
    this.showEditForm = true;
    this.editLawyer = {
      firstName: this.selectedLawyer.firstName,
      lastName: this.selectedLawyer.lastName,
      companyName: this.selectedLawyer.companyName
    };
  }

  onCreateLawyer(): void {
    if (!this.isValidLawyer(this.newLawyer)) {
      this.error = 'Please fill in all required fields';
      return;
    }

    this.loading = true;
    this.error = null;

    this.lawyerService.createLawyer(this.newLawyer).subscribe({
      next: (lawyer) => {
        this.hideAllForms();
        this.resetNewLawyerForm();
        this.loadLawyers();
        this.selectedLawyer = lawyer;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to create lawyer: ' + (error.error?.message || error.message);
        this.loading = false;
      }
    });
  }

  onUpdateLawyer(): void {
    if (!this.selectedLawyer || !this.isValidLawyer(this.editLawyer)) {
      this.error = 'Please fill in all required fields';
      return;
    }

    this.loading = true;
    this.error = null;

    this.lawyerService.updateLawyer(this.selectedLawyer.id, this.editLawyer).subscribe({
      next: (lawyer) => {
        this.hideAllForms();
        this.loadLawyers();
        this.selectedLawyer = lawyer;
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to update lawyer: ' + (error.error?.message || error.message);
        this.loading = false;
      }
    });
  }

  onDeleteLawyer(): void {
    if (!this.selectedLawyer) return;

    if (!confirm(`Are you sure you want to delete ${this.getFullName(this.selectedLawyer)}?`)) {
      return;
    }

    this.loading = true;
    this.error = null;

    this.lawyerService.deleteLawyer(this.selectedLawyer.id).subscribe({
      next: () => {
        this.selectedLawyer = null;
        this.hideAllForms();
        this.loadLawyers();
        this.loading = false;
      },
      error: (error) => {
        this.error = 'Failed to delete lawyer: ' + (error.error?.message || error.message);
        this.loading = false;
      }
    });
  }

  onCancelForm(): void {
    this.hideAllForms();
    this.resetNewLawyerForm();
    this.error = null;
  }

  private hideAllForms(): void {
    this.showCreateForm = false;
    this.showEditForm = false;
  }

  private resetNewLawyerForm(): void {
    this.newLawyer = {
      firstName: '',
      lastName: '',
      companyName: ''
    };
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
