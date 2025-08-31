import { Component, Input, OnDestroy, OnInit } from '@angular/core';
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
  @Input() personId!: string;

  loading = false;
  person?: Person;

  private destroy$ = new AsyncSubject<any>();

  constructor(private peopleService: PeopleService) {
  }

  ngOnInit(): void {
    this.loadPersonData();
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  private loadPersonData(): void {
    this.loading = true;
    this.peopleService.getPerson(this.personId)
      .pipe(
        finalize(() => this.loading = false),
        takeUntil(this.destroy$),
      )
      .subscribe(person => this.person = person);
  }
}