import { Component, OnDestroy, OnInit, signal, computed, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AsyncSubject, distinctUntilChanged, filter, map, Subscription, takeUntil } from 'rxjs';
import { LegalMatter } from '../legal-matter';
import { DEFAULT_PAGE_SIZE, LegalMatterService } from '../legal-matter.service';
import { PaginatorComponent } from '../paginator/paginator.component';
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
  // Signals for reactive state management
  legalMatterCount = signal<number | undefined>(undefined);
  legalMatters = signal<LegalMatter[]>([]);
  legalMattersPerPage = signal(DEFAULT_PAGE_SIZE);
  loading = signal(true);
  page = signal(0);
  showCreateForm = signal(false);
  newLegalMatter: Partial<LegalMatter> = {}; // Keep as regular property for form binding
  creating = signal(false);
  editingLegalMatter = signal<LegalMatter | null>(null);
  showEditForm = signal(false);
  deleting = signal(false);
  selectedLegalMatter = signal<LegalMatter | null>(null);
  showDetailsModal = signal(false);
  
  // Lawyer assignment signals
  showLawyerAssignmentModal = signal(false);
  selectedLegalMattersForAssignment = signal<string[]>([]);
  selectedLegalMattersData = signal<{ [id: string]: boolean }>({});
  lawyers = signal<Lawyer[]>([]);
  isMultiSelectMode = signal(false);

  // Computed signals for derived state
  selectedCount = computed(() => {
    const selected = this.selectedLegalMattersData();
    return Object.values(selected).filter(Boolean).length;
  });

  hasSelection = computed(() => this.selectedCount() > 0);

  allSelected = computed(() => {
    const matters = this.legalMatters();
    const selected = this.selectedLegalMattersData();
    return matters.length > 0 && matters.every(m => selected[m.id!]);
  });

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
    this.showCreateForm.set(true);
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
    this.showCreateForm.set(false);
    this.newLegalMatter = {};
  }

  submitLegalMatter(): void {
    if (!this.newLegalMatter.matterName?.trim()) {
      alert('Matter name is required');
      return;
    }

    this.creating.set(true);
    
    this.legalMatterService.createLegalMatter(this.newLegalMatter).subscribe({
      next: (createdMatter) => {
        console.log('Legal matter created:', createdMatter);
        this.showCreateForm.set(false);
        this.newLegalMatter = {};
        this.creating.set(false);
        // Reload the list to show the new matter
        this.loadLegalMatters(this.page() || 1);
        this.reloadLegalMatterCount();
      },
      error: (err) => {
        console.error('Error creating legal matter:', err);
        alert('Failed to create legal matter: ' + (err.error?.error || err.message));
        this.creating.set(false);
      }
    });
  }

  // View legal matter details
  viewLegalMatter(legalMatter: LegalMatter): void {
    this.selectedLegalMatter.set(legalMatter);
    this.showDetailsModal.set(true);
  }

  closeDetailsModal(): void {
    this.showDetailsModal.set(false);
    this.selectedLegalMatter.set(null);
  }

  // Edit legal matter
  editLegalMatter(legalMatter: LegalMatter): void {
    this.editingLegalMatter.set(new LegalMatter(legalMatter));
    this.showEditForm.set(true);
  }

  cancelEdit(): void {
    this.showEditForm.set(false);
    this.editingLegalMatter.set(null);
  }

  submitEditLegalMatter(): void {
    const editing = this.editingLegalMatter();
    if (!editing?.matterName?.trim()) {
      alert('Matter name is required');
      return;
    }

    if (!editing?.id) {
      alert('Legal matter ID is missing');
      return;
    }

    this.creating.set(true);
    
    this.legalMatterService.updateLegalMatter(editing.id, editing).subscribe({
      next: (updatedMatter) => {
        console.log('Legal matter updated:', updatedMatter);
        this.showEditForm.set(false);
        this.editingLegalMatter.set(null);
        this.creating.set(false);
        // Reload the list to show the updated matter
        this.loadLegalMatters(this.page() || 1);
      },
      error: (err) => {
        console.error('Error updating legal matter:', err);
        alert('Failed to update legal matter: ' + (err.error?.error || err.message));
        this.creating.set(false);
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

    this.deleting.set(true);
    
    this.legalMatterService.deleteLegalMatter(legalMatter.id).subscribe({
      next: () => {
        console.log('Legal matter deleted:', legalMatter.id);
        this.deleting.set(false);
        // Reload the list to remove the deleted matter
        this.loadLegalMatters(this.page() || 1);
        this.reloadLegalMatterCount();
      },
      error: (err) => {
        console.error('Error deleting legal matter:', err);
        alert('Failed to delete legal matter: ' + (err.error?.error || err.message));
        this.deleting.set(false);
      }
    });
  }

  onPageChange(page: number): void {
    // Only load if it's a different page to avoid duplicate calls
    if (this.page() !== page) {
      this.loadLegalMatters(page);
    }
  }

  private loadLegalMatters(page: number): void {
    this.page.set(page);
    this.loading.set(true);
    
    console.log('Loading legal matters for page:', page);

    this.legalMatterService.getLegalMatters(page).subscribe({
      next: (legalMatters) => {
        console.log('Received legal matters:', legalMatters);
        this.legalMatters.set(legalMatters);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error loading legal matters', err);
        this.legalMatters.set([]);
        this.loading.set(false);
      },
    });
  }

  private reloadLegalMatters(): void {
    this.loadLegalMatters(this.page());
  }

  private reloadLegalMatterCount(): void {
    this.legalMatterService.getLegalMatterCount().subscribe({
      next: (count) => {
        this.legalMatterCount.set(count);
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
        this.lawyers.set(lawyers);
      },
      error: (error) => {
        console.error('Error loading lawyers:', error);
      }
    });
  }

  toggleMultiSelectMode(): void {
    this.isMultiSelectMode.update(mode => !mode);
    if (!this.isMultiSelectMode()) {
      this.selectedLegalMattersData.set({});
      this.selectedLegalMattersForAssignment.set([]);
    }
  }

  toggleLegalMatterSelection(legalMatterId: string): void {
    const currentData = { ...this.selectedLegalMattersData() };
    const currentAssignment = [...this.selectedLegalMattersForAssignment()];
    
    if (currentData[legalMatterId]) {
      delete currentData[legalMatterId];
      const updatedAssignment = currentAssignment.filter(id => id !== legalMatterId);
      this.selectedLegalMattersForAssignment.set(updatedAssignment);
    } else {
      currentData[legalMatterId] = true;
      currentAssignment.push(legalMatterId);
      this.selectedLegalMattersForAssignment.set(currentAssignment);
    }
    this.selectedLegalMattersData.set(currentData);
  }

  selectAllLegalMatters(): void {
    const newData: { [id: string]: boolean } = {};
    const newAssignment: string[] = [];
    
    this.legalMatters().forEach(matter => {
      if (matter.id) {
        newData[matter.id] = true;
        newAssignment.push(matter.id);
      }
    });
    
    this.selectedLegalMattersData.set(newData);
    this.selectedLegalMattersForAssignment.set(newAssignment);
  }

  clearSelection(): void {
    this.selectedLegalMattersData.set({});
    this.selectedLegalMattersForAssignment.set([]);
  }

  openLawyerAssignmentModal(legalMatterId?: string): void {
    if (legalMatterId) {
      // Single assignment
      this.selectedLegalMattersForAssignment.set([legalMatterId]);
    }
    // For multiple assignment, use already selected matters
    this.showLawyerAssignmentModal.set(true);
  }

  assignLawyer(legalMatter: LegalMatter): void {
    if (legalMatter.id) {
      this.openLawyerAssignmentModal(legalMatter.id);
    }
  }

  closeLawyerAssignmentModal(): void {
    this.showLawyerAssignmentModal.set(false);
    if (!this.isMultiSelectMode()) {
      this.selectedLegalMattersForAssignment.set([]);
    }
  }

  onLawyerAssignmentComplete(event: {lawyerId: string, action: 'assign' | 'unassign'}): void {
    if (event.action === 'assign') {
      console.log(`Assigned lawyer ${event.lawyerId} to legal matters:`, this.selectedLegalMattersForAssignment());
    } else {
      console.log('Unassigned lawyer from legal matter');
    }
    
    // Reload the legal matters to show updated assignments
    this.loadLegalMatters(this.page() + 1);
    
    // Clear selection if not in multi-select mode
    if (!this.isMultiSelectMode()) {
      this.clearSelection();
    }
  }

  getLawyerName(lawyerId: string | null | undefined): string {
    if (!lawyerId) return 'Unassigned';
    const lawyer = this.lawyers().find(l => l.id === lawyerId);
    return lawyer ? lawyer.fullName : 'Unknown Lawyer';
  }

  get hasSelectedItems(): boolean {
    return this.selectedLegalMattersForAssignment().length > 0;
  }

  get currentLawyerIdForAssignment(): string | null {
    const assignments = this.selectedLegalMattersForAssignment();
    if (assignments.length === 1) {
      const matter = this.legalMatters().find(lm => lm.id === assignments[0]);
      return matter?.lawyerId || null;
    }
    return null;
  }
}
