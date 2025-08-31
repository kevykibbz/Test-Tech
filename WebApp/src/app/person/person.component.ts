import { Component, input, OnDestroy, OnInit, signal, effect } from '@angular/core';
import { AsyncSubject, finalize, takeUntil } from 'rxjs';
import { PeopleService } from '../people.service';
import { Person } from '../person';

@Component({
  selector: 'app-person',
  imports: [],
  templateUrl: './person.component.html',
  styleUrl: './person.component.scss'
})
export class PersonComponent implements OnInit, OnDestroy {
  // Input signals
  personId = input.required<string>();

  // State signals
  loading = signal(false);
  person = signal<Person | undefined>(undefined);

  private destroy$ = new AsyncSubject<any>();

  constructor(private peopleService: PeopleService) {
    // Effect to reload data when personId changes
    effect(() => {
      const id = this.personId();
      if (id) {
        this.loadPersonData();
      }
    });
  }

  ngOnInit(): void {
    this.loadPersonData();
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  private loadPersonData(): void {
    this.loading.set(true);
    this.peopleService.getPerson(this.personId())
      .pipe(
        finalize(() => this.loading.set(false)),
        takeUntil(this.destroy$),
      )
      .subscribe(person => this.person.set(person));
  }
}