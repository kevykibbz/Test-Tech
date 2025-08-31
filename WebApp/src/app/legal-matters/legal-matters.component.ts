import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AsyncSubject, distinctUntilChanged, filter, map, Subscription, takeUntil } from 'rxjs';
import { LegalMatter } from '../legal-matter';
import { DEFAULT_PAGE_SIZE, LegalMatterService } from '../legal-matter.service';
import { PaginatorComponent } from '../paginator/paginator.component';
import { MOCK_LEGAL_MATTERS } from '../../mocks/legal-matters';
import { SocketService } from '../socket.service';
import { SocketState } from '../socket-state';
import { LawyerAssignmentModalComponent } from '../lawyer-assignment-modal/lawyer-assignment-modal.component';
import { LawyerService } from '../lawyer.service';
import { Lawyer } from '../lawyer';

@Component({
  selector: 'app-legal-matters',
  imports: [CommonModule, FormsModule, PaginatorComponent, LawyerAssignmentModalComponent],
  templateUrl: './legal-matters.component.html',
  styleUrl: './legal-matters.component.scss',
  providers: [],
})
export class LegalMattersComponent implements OnInit, OnDestroy {
  legalMatterCount?: number;
  legalMatters: LegalMatter[] = [];
  legalMattersPerPage = DEFAULT_PAGE_SIZE;
  loading = true; // Initialize loading to true
  page = 0; // Initialize page to 0
  showCreateForm = false; // Add flag for showing create form
  newLegalMatter: Partial<LegalMatter> = {}; // New legal matter form data
  creating = false; // Flag for create operation
  editingLegalMatter: LegalMatter | null = null; // Currently editing legal matter
  showEditForm = false; // Flag for showing edit form
  deleting = false; // Flag for delete operation
  selectedLegalMatter: LegalMatter | null = null; // For viewing details
  showDetailsModal = false; // Flag for showing details modal
  
  // Lawyer assignment properties
  showLawyerAssignmentModal = false;
  selectedLegalMattersForAssignment: string[] = [];
  selectedLegalMattersData: { [id: string]: boolean } = {};
  lawyers: Lawyer[] = [];
  isMultiSelectMode = false;

  private destroy$ = new AsyncSubject<any>();
  private legalMatterListChangeListener?: Subscription;

  constructor(
    private router: Router,
    private legalMatterService: LegalMatterService,
    private socketService: SocketService,
    private lawyerService: LawyerService
  ) {}

  ngOnInit(): void {
    this.startListeningForLegalMatterListChanges();
    this.reloadLegalMatterCount();
    this.loadLegalMatters(1); // Load first page initially
    this.loadLawyers(); // Load lawyers for assignment dropdown
  }

  ngOnDestroy(): void {
    this.stopListeningForLegalMatterListChanges();
    this.destroy$.next(true);
    this.destroy$.complete();
  }
  createLegalMatter(): void {
    this.showCreateForm = true;
    this.newLegalMatter = {
      matterName: '',
      contractType: '',
      status: '',
      description: '',
      contractValue: null,
      governingLaw: ''
    };
  }

  cancelCreate(): void {
    this.showCreateForm = false;
    this.newLegalMatter = {};
  }

  submitLegalMatter(): void {
    if (!this.newLegalMatter.matterName?.trim()) {
      alert('Matter name is required');
      return;
    }

    this.creating = true;
    
    this.legalMatterService.createLegalMatter(this.newLegalMatter).subscribe({
      next: (createdMatter) => {
        console.log('Legal matter created:', createdMatter);
        this.showCreateForm = false;
        this.newLegalMatter = {};
        this.creating = false;
        // Reload the list to show the new matter
        this.loadLegalMatters(this.page || 1);
        this.reloadLegalMatterCount();
      },
      error: (err) => {
        console.error('Error creating legal matter:', err);
        alert('Failed to create legal matter: ' + (err.error?.error || err.message));
        this.creating = false;
      }
    });
  }

  // View legal matter details
  viewLegalMatter(legalMatter: LegalMatter): void {
    this.selectedLegalMatter = legalMatter;
    this.showDetailsModal = true;
  }

  closeDetailsModal(): void {
    this.showDetailsModal = false;
    this.selectedLegalMatter = null;
  }

  // Edit legal matter
  editLegalMatter(legalMatter: LegalMatter): void {
    this.editingLegalMatter = new LegalMatter(legalMatter);
    this.showEditForm = true;
  }

  cancelEdit(): void {
    this.showEditForm = false;
    this.editingLegalMatter = null;
  }

  submitEditLegalMatter(): void {
    if (!this.editingLegalMatter?.matterName?.trim()) {
      alert('Matter name is required');
      return;
    }

    if (!this.editingLegalMatter?.id) {
      alert('Legal matter ID is missing');
      return;
    }

    this.creating = true;
    
    this.legalMatterService.updateLegalMatter(this.editingLegalMatter.id, this.editingLegalMatter).subscribe({
      next: (updatedMatter) => {
        console.log('Legal matter updated:', updatedMatter);
        this.showEditForm = false;
        this.editingLegalMatter = null;
        this.creating = false;
        // Reload the list to show the updated matter
        this.loadLegalMatters(this.page || 1);
      },
      error: (err) => {
        console.error('Error updating legal matter:', err);
        alert('Failed to update legal matter: ' + (err.error?.error || err.message));
        this.creating = false;
      }
    });
  }

  // Delete legal matter
  deleteLegalMatter(legalMatter: LegalMatter): void {
    if (!legalMatter.id) {
      alert('Legal matter ID is missing');
      return;
    }

    const confirmDelete = confirm(`Are you sure you want to delete "${legalMatter.matterName}"? This action cannot be undone.`);
    if (!confirmDelete) {
      return;
    }

    this.deleting = true;
    
    this.legalMatterService.deleteLegalMatter(legalMatter.id).subscribe({
      next: () => {
        console.log('Legal matter deleted:', legalMatter.id);
        this.deleting = false;
        // Reload the list to remove the deleted matter
        this.loadLegalMatters(this.page || 1);
        this.reloadLegalMatterCount();
      },
      error: (err) => {
        console.error('Error deleting legal matter:', err);
        alert('Failed to delete legal matter: ' + (err.error?.error || err.message));
        this.deleting = false;
      }
    });
  }

  onPageChange(page: number): void {
    // Only load if it's a different page to avoid duplicate calls
    if (this.page !== page) {
      this.loadLegalMatters(page);
    }
  }

  private loadLegalMatters(page: number): void {
    this.page = page;
    this.loading = true;
    
    console.log('Loading legal matters for page:', page);

    this.legalMatterService.getLegalMatters(page).subscribe({
      next: (legalMatters) => {
        console.log('Received legal matters:', legalMatters);
        this.legalMatters = legalMatters;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading legal matters', err);
        this.legalMatters = [];
        this.loading = false;
      },
    });
  }

  private reloadLegalMatters(): void {
    this.loadLegalMatters(this.page);
  }

  private reloadLegalMatterCount(): void {
    this.legalMatterService.getLegalMatterCount().subscribe({
      next: (count) => {
        this.legalMatterCount = count;
      },
      error: (err) => {
        console.error('Error loading legal matter count', err);
      },
    });
  }

  private startListeningForLegalMatterListChanges(): void {
    this.socketService.socketState$
      .pipe(
        map((s) => s === SocketState.Opened),
        distinctUntilChanged(),
        filter((opened) => opened),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.socketService.sendMessage({
          type: 'action',
          parameters: {
            action: 'join-channel',
            parameters: {
              channel_type: 'legal_matter.list.updates',
            },
          },
        });
      });

    this.legalMatterListChangeListener = this.socketService.message$
      .pipe(filter((m: any) => m && m.context === 'legal_matter.list'))
      .subscribe(() => {
        this.reloadLegalMatters();
        this.reloadLegalMatterCount();
      });
  }

  private stopListeningForLegalMatterListChanges(): void {
    if (this.legalMatterListChangeListener) {
      this.legalMatterListChangeListener.unsubscribe();
      this.legalMatterListChangeListener = undefined;
    }

    this.socketService.sendMessage({
      type: 'action',
      parameters: {
        action: 'leave-channel',
        parameters: {
          channel_type: 'legal_matter.list.updates',
        },
      },
    });
  }

  // Lawyer assignment methods
  loadLawyers(): void {
    this.lawyerService.getLawyers(0, 1000).subscribe({
      next: (lawyers) => {
        this.lawyers = lawyers;
      },
      error: (error) => {
        console.error('Error loading lawyers:', error);
      }
    });
  }

  toggleMultiSelectMode(): void {
    this.isMultiSelectMode = !this.isMultiSelectMode;
    if (!this.isMultiSelectMode) {
      this.selectedLegalMattersData = {};
      this.selectedLegalMattersForAssignment = [];
    }
  }

  toggleLegalMatterSelection(legalMatterId: string): void {
    if (this.selectedLegalMattersData[legalMatterId]) {
      delete this.selectedLegalMattersData[legalMatterId];
      this.selectedLegalMattersForAssignment = this.selectedLegalMattersForAssignment.filter(id => id !== legalMatterId);
    } else {
      this.selectedLegalMattersData[legalMatterId] = true;
      this.selectedLegalMattersForAssignment.push(legalMatterId);
    }
  }

  selectAllLegalMatters(): void {
    this.selectedLegalMattersData = {};
    this.selectedLegalMattersForAssignment = [];
    this.legalMatters.forEach(matter => {
      if (matter.id) {
        this.selectedLegalMattersData[matter.id] = true;
        this.selectedLegalMattersForAssignment.push(matter.id);
      }
    });
  }

  clearSelection(): void {
    this.selectedLegalMattersData = {};
    this.selectedLegalMattersForAssignment = [];
  }

  openLawyerAssignmentModal(legalMatterId?: string): void {
    if (legalMatterId) {
      // Single assignment
      this.selectedLegalMattersForAssignment = [legalMatterId];
    }
    // For multiple assignment, use already selected matters
    this.showLawyerAssignmentModal = true;
  }

  assignLawyer(legalMatter: LegalMatter): void {
    if (legalMatter.id) {
      this.openLawyerAssignmentModal(legalMatter.id);
    }
  }

  closeLawyerAssignmentModal(): void {
    this.showLawyerAssignmentModal = false;
    if (!this.isMultiSelectMode) {
      this.selectedLegalMattersForAssignment = [];
    }
  }

  onLawyerAssignmentComplete(event: {lawyerId: string, action: 'assign' | 'unassign'}): void {
    if (event.action === 'assign') {
      console.log(`Assigned lawyer ${event.lawyerId} to legal matters:`, this.selectedLegalMattersForAssignment);
    } else {
      console.log('Unassigned lawyer from legal matter');
    }
    
    // Reload the legal matters to show updated assignments
    this.loadLegalMatters(this.page + 1);
    
    // Clear selection if not in multi-select mode
    if (!this.isMultiSelectMode) {
      this.clearSelection();
    }
  }

  getLawyerName(lawyerId: string | null | undefined): string {
    if (!lawyerId) return 'Unassigned';
    const lawyer = this.lawyers.find(l => l.id === lawyerId);
    return lawyer ? lawyer.fullName : 'Unknown Lawyer';
  }

  get selectedCount(): number {
    return this.selectedLegalMattersForAssignment.length;
  }

  get hasSelectedItems(): boolean {
    return this.selectedLegalMattersForAssignment.length > 0;
  }

  get currentLawyerIdForAssignment(): string | null {
    if (this.selectedLegalMattersForAssignment.length === 1) {
      const matter = this.legalMatters.find(lm => lm.id === this.selectedLegalMattersForAssignment[0]);
      return matter?.lawyerId || null;
    }
    return null;
  }
}
