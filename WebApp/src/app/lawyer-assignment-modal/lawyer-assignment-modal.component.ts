import { Component, EventEmitter, Input, OnInit, Output, signal, computed, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Lawyer } from '../lawyer';
import { LawyerService } from '../lawyer.service';
import { LegalMatterService } from '../legal-matter.service';

@Component({
  selector: 'app-lawyer-assignment-modal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="modal-overlay" (click)="closeModal()">
      <div class="modal-content" (click)="$event.stopPropagation()">
        <div class="modal-header">
          <h2>{{ isMultipleSelection() ? 'Assign Lawyer to Legal Matters' : 'Assign Lawyer to Legal Matter' }}</h2>
          <button class="close-btn" (click)="closeModal()">&times;</button>
        </div>
        
        <div class="modal-body">
          @if (isMultipleSelection()) {
            <div class="selected-matters">
              <h3>Selected Legal Matters ({{ selectedLegalMatterIds().length }})</h3>
              <div class="matter-list">
                @for (matterId of selectedLegalMatterIds(); track matterId) {
                  <div class="matter-item">
                    Matter ID: {{ matterId }}
                  </div>
                }
              </div>
            </div>
          }

          <div class="lawyer-selection">
            <h3>Select Lawyer</h3>
            <div class="search-box">
              <input 
                type="text" 
                [ngModel]="searchTerm()" 
                (ngModelChange)="searchTerm.set($event)"
                placeholder="Search lawyers..."
                class="form-control">
            </div>
            
            <div class="lawyers-list">
              @for (lawyer of filteredLawyers(); track lawyer.id) {
                <div class="lawyer-item"
                     [class.selected]="selectedLawyerId() === lawyer.id"
                     (click)="selectLawyer(lawyer)">
                  <div class="lawyer-info">
                    <div class="lawyer-name">{{ lawyer.fullName }}</div>
                    <div class="lawyer-company">{{ lawyer.companyName }}</div>
                  </div>
                  <div class="lawyer-actions">
                    <input 
                      type="radio" 
                      [value]="lawyer.id" 
                      [ngModel]="selectedLawyerId()" 
                      (ngModelChange)="selectedLawyerId.set($event)"
                      name="selectedLawyer">
                  </div>
                </div>
              }
            </div>
          </div>
        </div>
        
        <div class="modal-footer">
          <button 
            class="btn btn-secondary" 
            (click)="closeModal()">
            Cancel
          </button>
          <button 
            class="btn btn-primary" 
            [disabled]="!selectedLawyerId()"
            (click)="assignLawyer()">
            {{ isMultipleSelection() ? 'Assign to All' : 'Assign Lawyer' }}
          </button>
          @if (!isMultipleSelection() && currentLawyerId()) {
            <button 
              class="btn btn-warning" 
              (click)="unassignLawyer()">
              Unassign Current Lawyer
            </button>
          }
        </div>
      </div>
    </div>
  `,
  styles: [`
    .modal-overlay {
      position: fixed;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      background-color: rgba(0, 0, 0, 0.5);
      display: flex;
      justify-content: center;
      align-items: center;
      z-index: 1000;
    }

    .modal-content {
      background: white;
      border-radius: 8px;
      width: 90%;
      max-width: 600px;
      max-height: 80vh;
      overflow-y: auto;
      box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    }

    .modal-header {
      padding: 20px;
      border-bottom: 1px solid #e0e0e0;
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .modal-header h2 {
      margin: 0;
      color: #333;
    }

    .close-btn {
      background: none;
      border: none;
      font-size: 24px;
      cursor: pointer;
      color: #666;
    }

    .close-btn:hover {
      color: #000;
    }

    .modal-body {
      padding: 20px;
    }

    .selected-matters {
      margin-bottom: 20px;
      padding: 15px;
      background-color: #f8f9fa;
      border-radius: 5px;
    }

    .selected-matters h3 {
      margin: 0 0 10px 0;
      color: #495057;
    }

    .matter-list {
      max-height: 100px;
      overflow-y: auto;
    }

    .matter-item {
      padding: 5px 0;
      font-size: 14px;
      color: #666;
    }

    .lawyer-selection h3 {
      margin: 0 0 15px 0;
      color: #333;
    }

    .search-box {
      margin-bottom: 15px;
    }

    .form-control {
      width: 100%;
      padding: 10px;
      border: 1px solid #ddd;
      border-radius: 4px;
      font-size: 14px;
    }

    .lawyers-list {
      max-height: 300px;
      overflow-y: auto;
      border: 1px solid #e0e0e0;
      border-radius: 4px;
    }

    .lawyer-item {
      padding: 15px;
      border-bottom: 1px solid #f0f0f0;
      cursor: pointer;
      display: flex;
      justify-content: space-between;
      align-items: center;
      transition: background-color 0.2s;
    }

    .lawyer-item:hover {
      background-color: #f8f9fa;
    }

    .lawyer-item.selected {
      background-color: #e3f2fd;
      border-left: 3px solid #2196f3;
    }

    .lawyer-info {
      flex: 1;
    }

    .lawyer-name {
      font-weight: 600;
      color: #333;
      margin-bottom: 4px;
    }

    .lawyer-company {
      font-size: 14px;
      color: #666;
    }

    .lawyer-actions {
      margin-left: 15px;
    }

    .modal-footer {
      padding: 20px;
      border-top: 1px solid #e0e0e0;
      display: flex;
      justify-content: flex-end;
      gap: 10px;
    }

    .btn {
      padding: 10px 20px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
      font-size: 14px;
      transition: background-color 0.2s;
    }

    .btn-primary {
      background-color: #007bff;
      color: white;
    }

    .btn-primary:hover:not(:disabled) {
      background-color: #0056b3;
    }

    .btn-primary:disabled {
      background-color: #6c757d;
      cursor: not-allowed;
    }

    .btn-secondary {
      background-color: #6c757d;
      color: white;
    }

    .btn-secondary:hover {
      background-color: #545b62;
    }

    .btn-warning {
      background-color: #ffc107;
      color: #212529;
    }

    .btn-warning:hover {
      background-color: #e0a800;
    }
  `]
})
export class LawyerAssignmentModalComponent implements OnInit {
  // Using modern input signals (Angular 20 feature)
  selectedLegalMatterIds = input<string[]>([]);
  currentLawyerId = input<string | null>(null);
  
  // Using modern output signals (Angular 20 feature)
  assignmentComplete = output<{lawyerId: string, action: 'assign' | 'unassign'}>();
  modalClosed = output<void>();

  // Signals for reactive state management
  lawyers = signal<Lawyer[]>([]);
  selectedLawyerId = signal<string | null>(null);
  searchTerm = signal('');

  // Computed signals for derived state
  filteredLawyers = computed(() => {
    const lawyers = this.lawyers();
    const term = this.searchTerm().toLowerCase();
    
    if (!term.trim()) {
      return lawyers;
    }
    
    return lawyers.filter(lawyer => 
      lawyer.fullName.toLowerCase().includes(term) ||
      lawyer.companyName.toLowerCase().includes(term)
    );
  });

  isMultipleSelection = computed(() => {
    return this.selectedLegalMatterIds().length > 1;
  });

  constructor(
    private lawyerService: LawyerService,
    private legalMatterService: LegalMatterService
  ) {}

  ngOnInit() {
    this.loadLawyers();
    this.selectedLawyerId.set(this.currentLawyerId());
  }

  loadLawyers() {
    this.lawyerService.getLawyers(0, 1000).subscribe({
      next: (lawyers) => {
        this.lawyers.set(lawyers);
      },
      error: (error) => {
        console.error('Error loading lawyers:', error);
      }
    });
  }

  filterLawyers() {
    // This method is no longer needed since we use computed signals
    // The filtering is now handled automatically by the filteredLawyers computed signal
  }

  selectLawyer(lawyer: Lawyer) {
    this.selectedLawyerId.set(lawyer.id);
  }

  assignLawyer() {
    const selectedId = this.selectedLawyerId();
    if (!selectedId) return;

    if (this.isMultipleSelection()) {
      // Assign lawyer to multiple legal matters
      const selectedMatterIds = this.selectedLegalMatterIds();
      const assignments = selectedMatterIds.map(matterId =>
        this.legalMatterService.assignLawyerToLegalMatter(matterId, selectedId)
      );

      // Wait for all assignments to complete
      Promise.all(assignments.map(obs => obs.toPromise())).then(() => {
        this.assignmentComplete.emit({
          lawyerId: selectedId,
          action: 'assign'
        });
        this.closeModal();
      }).catch(error => {
        console.error('Error assigning lawyer to legal matters:', error);
        alert('Error assigning lawyer to some legal matters');
      });
    } else {
      // Assign lawyer to single legal matter
      const selectedMatterIds = this.selectedLegalMatterIds();
      this.legalMatterService.assignLawyerToLegalMatter(
        selectedMatterIds[0], 
        selectedId
      ).subscribe({
        next: () => {
          this.assignmentComplete.emit({
            lawyerId: selectedId,
            action: 'assign'
          });
          this.closeModal();
        },
        error: (error) => {
          console.error('Error assigning lawyer:', error);
          alert('Error assigning lawyer to legal matter');
        }
      });
    }
  }

  unassignLawyer() {
    const selectedMatterIds = this.selectedLegalMatterIds();
    if (selectedMatterIds.length !== 1) return;

    this.legalMatterService.unassignLawyerFromLegalMatter(selectedMatterIds[0]).subscribe({
      next: () => {
        this.assignmentComplete.emit({
          lawyerId: '',
          action: 'unassign'
        });
        this.closeModal();
      },
      error: (error) => {
        console.error('Error unassigning lawyer:', error);
        alert('Error unassigning lawyer from legal matter');
      }
    });
  }

  closeModal() {
    this.modalClosed.emit();
  }
}
