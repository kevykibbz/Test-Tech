

import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges, signal, computed, effect, input, output } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { AsyncSubject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-paginator',
  imports: [ReactiveFormsModule],
  templateUrl: './paginator.component.html',
  styleUrl: './paginator.component.scss'
})
export class PaginatorComponent implements OnChanges, OnInit, OnDestroy {
  // Modern input signals
  disabled = input<boolean>(false);
  pageSize = input<number>(10);
  totalItems = input<number | undefined>(undefined);

  // Modern output signals
  selectedPageChange = output<number>();

  // Signals for reactive state management
  selectedPage = signal(1);
  totalPages = signal<number | undefined>(undefined);
  
  // Computed signals for derived state
  canSelectNext = computed(() => {
    const current = this.selectedPage();
    const total = this.totalPages();
    return total ? current < total : false;
  });
  
  canSelectPrevious = computed(() => {
    return this.selectedPage() > 1;
  });
  
  paginatorItems = computed(() => {
    const total = this.totalPages();
    if (!total) return [];
    
    const items: number[] = [];
    for (let i = 1; i <= total; i++) {
      items.push(i);
    }
    return items;
  });

  selectedPageControl = new FormControl<number>(1);
  private destroy$ = new AsyncSubject<any>();

  constructor() {
    // Effect to update pagination when inputs change
    effect(() => {
      const items = this.totalItems();
      const size = this.pageSize();
      this.updatePagination(items, size);
    });
    
    // Effect to emit page changes
    effect(() => {
      const page = this.selectedPage();
      this.selectedPageChange.emit(page);
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    // No longer needed - effects handle input changes automatically
  }

  ngOnInit(): void {
    this.selectedPageControl.valueChanges
      .pipe(
        takeUntil(this.destroy$),
      )
      .subscribe((page: any) => {
        this.selectedPage.set(page || 1);
      });

    this.selectedPageControl.setValue(1);
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  selectNextPage(): void {
    const currentValue = this.selectedPageControl.value ?? 1;
    const totalPages = this.totalPages();
    this.selectedPageControl.setValue(Math.min(currentValue + 1, totalPages || 1));
  }

  selectPreviousPage(): void {
    const currentValue = this.selectedPageControl.value ?? 1;
    this.selectedPageControl.setValue(Math.max(currentValue - 1, 1));
  }

  trackByIndex(index: number): number {
    return index;
  }

  private updatePagination(totalItems: number | undefined, pageSize: number): void {
    if (!totalItems || !pageSize) {
      this.totalPages.set(undefined);
      return;
    }
    
    const calculatedTotalPages = Math.ceil(totalItems / pageSize);
    this.totalPages.set(calculatedTotalPages);
  }
}
