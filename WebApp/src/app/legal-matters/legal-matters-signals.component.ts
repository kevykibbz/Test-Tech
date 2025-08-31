import { Component, OnInit, OnDestroy, computed, signal, effect, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { takeUntil, map, distinctUntilChanged, filter } from 'rxjs/operators';
import { Subject } from 'rxjs';

import { LegalMatter } from '../legal-matter';
import { LegalMatterService } from '../legal-matter.service';
import { LawyerService } from '../lawyer.service';
import { SocketService } from '../socket.service';
import { SocketState } from '../socket-state';
import { PaginatorComponent } from '../paginator/paginator.component';
import { LawyerAssignmentModalComponent } from '../lawyer-assignment-modal/lawyer-assignment-modal.component';

/**
 * EXAMPLE: Legal Matters Component using Angular Signals
 * This demonstrates how the component should be refactored to use signals
 */
@Component({
  selector: 'app-legal-matters-signals',
  standalone: true,
  imports: [CommonModule, FormsModule, PaginatorComponent, LawyerAssignmentModalComponent],
  template: `
    <div class="legal-matters-container">
      <h2>Legal Matters Management (Signals Version)</h2>

      <!-- Add New Legal Matter Form -->
      @if (showCreateForm()) {
        <div class="create-form">
          <h3>Create New Legal Matter</h3>
          <form (ngSubmit)="createLegalMatter()">
            <div style="margin-bottom: 15px;">
              <label for="matterName">Matter Name</label>
              <input 
                type="text" 
                id="matterName" 
                [(ngModel)]="newLegalMatter.matterName"
                required>
            </div>
            
            <div style="margin-bottom: 15px;">
              <label for="contractType">Contract Type</label>
              <input 
                type="text" 
                id="contractType" 
                [(ngModel)]="newLegalMatter.contractType">
            </div>
            
            <div style="margin-bottom: 15px;">
              <label for="status">Status</label>
              <select id="status" [(ngModel)]="newLegalMatter.status">
                <option value="">Select Status</option>
                <option value="Active">Active</option>
                <option value="Pending">Pending</option>
                <option value="Completed">Completed</option>
                <option value="On Hold">On Hold</option>
              </select>
            </div>
            
            <div class="form-actions">
              <button type="submit" [disabled]="creating()">
                {{ creating() ? 'Creating...' : 'Create Legal Matter' }}
              </button>
              <button type="button" (click)="cancelCreate()">Cancel</button>
            </div>
          </form>
        </div>
      }

      <!-- Loading State -->
      @if (loading()) {
        <div style="text-align: center; padding: 20px;">
          Loading legal matters...
        </div>
      } @else {
        <!-- Multi-select Controls -->
        <div class="multi-select-controls">
          <div style="display: flex; justify-content: space-between; align-items: center;">
            <div>
              <button type="button" (click)="toggleMultiSelectMode()">
                {{ isMultiSelectMode() ? 'Exit Multi-Select' : 'Multi-Select Mode' }}
              </button>
              
              @if (isMultiSelectMode()) {
                <button type="button" (click)="clearSelection()">
                  Clear Selection ({{ selectedCount() }})
                </button>
                
                @if (selectedCount() > 0) {
                  <button type="button" (click)="openLawyerAssignmentModal()">
                    Assign Lawyer to Selected ({{ selectedCount() }})
                  </button>
                }
              }
            </div>
            
            <div>
              <button type="button" (click)="showCreateForm.set(true)" 
                      [disabled]="showCreateForm()">
                Add New Legal Matter
              </button>
              <button type="button" (click)="refreshLegalMatters()">
                Refresh
              </button>
            </div>
          </div>
        </div>

        <!-- Legal Matters Table -->
        <div class="table-container">
          <table class="legal-matters-table">
            <thead>
              <tr>
                @if (isMultiSelectMode()) {
                  <th>
                    <input type="checkbox" 
                           [checked]="allSelected()"
                           [indeterminate]="someSelected()"
                           (change)="toggleSelectAll()">
                  </th>
                }
                <th>Matter Name</th>
                <th>Contract Type</th>
                <th>Status</th>
                <th>Contract Value</th>
                <th>Governing Law</th>
                <th>Assigned Lawyer</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              @for (legalMatter of legalMatters(); track legalMatter.id) {
                <tr [class.selected-row]="isSelected(legalMatter.id!)">
                  @if (isMultiSelectMode()) {
                    <td>
                      <input type="checkbox"
                             [checked]="isSelected(legalMatter.id!)"
                             (change)="toggleSelection(legalMatter.id!)">
                    </td>
                  }
                  <td>{{ legalMatter.matterName }}</td>
                  <td>{{ legalMatter.contractType || 'N/A' }}</td>
                  <td>
                    <span [class]="'status-' + (legalMatter.status || 'unknown').toLowerCase()">
                      {{ legalMatter.status || 'N/A' }}
                    </span>
                  </td>
                  <td>
                    @if (legalMatter.contractValue) {
                      {{ legalMatter.contractValue | currency }}
                    } @else {
                      N/A
                    }
                  </td>
                  <td>{{ legalMatter.governingLaw || 'N/A' }}</td>
                  <td>{{ legalMatter.assignedLawyerName || 'Unassigned' }}</td>
                  <td>
                    <button type="button" 
                            (click)="openLawyerAssignmentModal(legalMatter.id!, legalMatter.assignedLawyerId)">
                      {{ legalMatter.assignedLawyerName ? 'Reassign' : 'Assign' }} Lawyer
                    </button>
                    <button type="button" (click)="editLegalMatter(legalMatter)">
                      Edit
                    </button>
                    <button type="button" 
                            (click)="deleteLegalMatter(legalMatter.id!)"
                            [disabled]="deleting().has(legalMatter.id!)">
                      {{ deleting().has(legalMatter.id!) ? 'Deleting...' : 'Delete' }}
                    </button>
                  </td>
                </tr>
              }
            </tbody>
          </table>
        </div>

        <!-- Pagination -->
        <app-paginator
          [pageSize]="pageSize()"
          [totalItems]="totalItems()"
          [disabled]="loading()"
          (selectedPageChange)="onPageChange($event)">
        </app-paginator>
      }

      <!-- Lawyer Assignment Modal -->
      @if (showLawyerModal()) {
        <app-lawyer-assignment-modal
          [selectedLegalMatterIds]="selectedLegalMatterIds()"
          [currentLawyerId]="currentLawyerId()"
          (assignmentComplete)="handleAssignmentComplete($event)"
          (modalClosed)="closeLawyerModal()">
        </app-lawyer-assignment-modal>
      }
    </div>
  `,
  styleUrls: ['./legal-matters.component.scss']
})
export class LegalMattersSignalsComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();

  // Signals for reactive state management
  legalMatters = signal<LegalMatter[]>([]);
  loading = signal(false);
  creating = signal(false);
  deleting = signal(new Set<string>());
  
  // Pagination signals
  currentPage = signal(1);
  pageSize = signal(10);
  totalItems = signal(0);
  
  // Selection signals
  isMultiSelectMode = signal(false);
  selectedLegalMatters = signal(new Set<string>());
  
  // Modal signals
  showCreateForm = signal(false);
  showLawyerModal = signal(false);
  selectedLegalMatterIds = signal<string[]>([]);
  currentLawyerId = signal<string | null>(null);

  // Computed signals for derived state
  selectedCount = computed(() => this.selectedLegalMatters().size);
  
  allSelected = computed(() => {
    const matters = this.legalMatters();
    const selected = this.selectedLegalMatters();
    return matters.length > 0 && matters.every(m => selected.has(m.id!));
  });
  
  someSelected = computed(() => {
    const selected = this.selectedLegalMatters();
    return selected.size > 0 && !this.allSelected();
  });

  // Form data (could also be signals)
  newLegalMatter: Partial<LegalMatter> = {};

  constructor(
    private legalMatterService: LegalMatterService,
    private lawyerService: LawyerService,
    private socketService: SocketService
  ) {
    // Effect for automatic data loading when page changes
    effect(() => {
      const page = this.currentPage();
      const size = this.pageSize();
      this.loadLegalMatters(page, size);
    });

    // Effect for clearing selection when exiting multi-select mode
    effect(() => {
      if (!this.isMultiSelectMode()) {
        this.selectedLegalMatters.set(new Set());
      }
    });

    // Effect for updating selected IDs for modal
    effect(() => {
      const selected = Array.from(this.selectedLegalMatters());
      if (selected.length !== this.selectedLegalMatterIds().length) {
        this.selectedLegalMatterIds.set(selected);
      }
    });
  }

  ngOnInit() {
    this.loadLegalMatters();
    this.startListeningForLegalMatterListChanges();
  }

  ngOnDestroy() {
    this.destroy$.next();
    this.destroy$.complete();
  }

  // Helper methods for selection
  isSelected(id: string): boolean {
    return this.selectedLegalMatters().has(id);
  }

  toggleSelection(id: string) {
    const current = new Set(this.selectedLegalMatters());
    if (current.has(id)) {
      current.delete(id);
    } else {
      current.add(id);
    }
    this.selectedLegalMatters.set(current);
  }

  toggleSelectAll() {
    if (this.allSelected()) {
      this.selectedLegalMatters.set(new Set());
    } else {
      const allIds = new Set(this.legalMatters().map(m => m.id!));
      this.selectedLegalMatters.set(allIds);
    }
  }

  clearSelection() {
    this.selectedLegalMatters.set(new Set());
  }

  toggleMultiSelectMode() {
    this.isMultiSelectMode.update(mode => !mode);
  }

  // Data loading methods
  loadLegalMatters(page = 1, size = 10) {
    this.loading.set(true);
    
    // Load both legal matters and total count
    Promise.all([
      this.legalMatterService.getLegalMatters((page - 1) * size, size).toPromise(),
      this.legalMatterService.getLegalMatterCount().toPromise()
    ]).then(([legalMatters, totalCount]) => {
      this.legalMatters.set(legalMatters || []);
      this.totalItems.set(totalCount || 0);
      this.loading.set(false);
    }).catch((error) => {
      console.error('Error loading legal matters:', error);
      this.loading.set(false);
    });
  }

  refreshLegalMatters() {
    this.loadLegalMatters(this.currentPage(), this.pageSize());
  }

  onPageChange(page: number) {
    this.currentPage.set(page);
  }

  // CRUD operations
  createLegalMatter() {
    if (!this.newLegalMatter.matterName?.trim()) return;

    this.creating.set(true);
    this.legalMatterService.createLegalMatter(this.newLegalMatter as LegalMatter)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          this.creating.set(false);
          this.showCreateForm.set(false);
          this.newLegalMatter = {};
          this.refreshLegalMatters();
        },
        error: (error) => {
          console.error('Error creating legal matter:', error);
          this.creating.set(false);
        }
      });
  }

  cancelCreate() {
    this.showCreateForm.set(false);
    this.newLegalMatter = {};
  }

  editLegalMatter(legalMatter: LegalMatter) {
    // Implementation for edit functionality
    console.log('Edit legal matter:', legalMatter);
  }

  deleteLegalMatter(id: string) {
    const current = new Set(this.deleting());
    current.add(id);
    this.deleting.set(current);

    this.legalMatterService.deleteLegalMatter(id)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          const updated = new Set(this.deleting());
          updated.delete(id);
          this.deleting.set(updated);
          this.refreshLegalMatters();
        },
        error: (error) => {
          console.error('Error deleting legal matter:', error);
          const updated = new Set(this.deleting());
          updated.delete(id);
          this.deleting.set(updated);
        }
      });
  }

  // Lawyer assignment modal methods
  openLawyerAssignmentModal(matterId?: string, currentLawyerId?: string) {
    if (matterId) {
      this.selectedLegalMatterIds.set([matterId]);
      this.currentLawyerId.set(currentLawyerId || null);
    }
    this.showLawyerModal.set(true);
  }

  closeLawyerModal() {
    this.showLawyerModal.set(false);
    this.selectedLegalMatterIds.set([]);
    this.currentLawyerId.set(null);
  }

  handleAssignmentComplete(event: {lawyerId: string, action: 'assign' | 'unassign'}) {
    this.refreshLegalMatters();
    this.closeLawyerModal();
  }

  // WebSocket listening
  private startListeningForLegalMatterListChanges() {
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

    this.socketService.message$
      .pipe(
        filter((m: any) => m && m.context === 'legal_matter.list'),
        takeUntil(this.destroy$)
      )
      .subscribe(() => {
        this.refreshLegalMatters();
      });
  }
}
