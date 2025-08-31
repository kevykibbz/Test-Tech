

import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { AsyncSubject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-paginator',
  imports: [ReactiveFormsModule],
  templateUrl: './paginator.component.html',
  styleUrl: './paginator.component.scss'
})
export class PaginatorComponent implements OnChanges, OnInit, OnDestroy {
  @Input() disabled!: boolean;
  @Input() pageSize!: number;
  @Input() totalItems?: number;

  @Output() selectedPageChange = new EventEmitter<number>();

  canSelectNext = false;
  canSelectPrevious = false;
  paginatorItems: number[] = [];
  selectedPageControl = new FormControl<number>(1);
  totalPages?: number;

  private destroy$ = new AsyncSubject<any>();

  constructor() {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['totalItems'] || changes['pageSize']) {
      this.updatePaginator();
    }
  }

  ngOnInit(): void {
    this.selectedPageControl.valueChanges
      .pipe(
        takeUntil(this.destroy$),
      )
      .subscribe((page:any) => {
        this.selectedPageChange.emit(page);
        this.updateBackAndForwardLinkStates(page);
      });

    this.selectedPageControl.setValue(1);
    this.updatePaginator();
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  selectNextPage(): void {
    const currentValue = this.selectedPageControl.value ?? 1;
    this.selectedPageControl.setValue(Math.min(currentValue + 1, this.totalPages!));
  }

  selectPreviousPage(): void {
    const currentValue = this.selectedPageControl.value ?? 1;
    this.selectedPageControl.setValue(Math.max(currentValue - 1, 1));
  }

  trackByIndex(index: number): number {
    return index;
  }

  private updatePaginator(): void {
    console.log('Updating paginator with totalItems:', this.totalItems, 'and pageSize:', this.pageSize);
    if (!this.totalItems) {
      this.paginatorItems = [];
      return;
    }
    console.log('Updating paginator with totalItems:', this.totalItems, 'and pageSize:', this.pageSize);

    const selectedPage = this.selectedPageControl.value ?? 1;
  
    this.totalPages = Math.ceil(this.totalItems / this.pageSize);
    this.paginatorItems = [];
    for (let i = 1; i <= this.totalPages; i++) {
      this.paginatorItems.push(i);
    }

    this.updateBackAndForwardLinkStates(selectedPage);
  }

  private updateBackAndForwardLinkStates(selectedPage: number): void {
    this.canSelectNext = selectedPage < this.totalPages!;
    this.canSelectPrevious = selectedPage > 1;
  }
}
