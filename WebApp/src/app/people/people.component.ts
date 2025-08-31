import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { Person } from '../person';
import { PeopleService } from '../people.service';

@Component({
  selector: 'app-people',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './people.component.html',
  styleUrls: ['./people.component.scss']
})
export class PeopleComponent implements OnInit {
  people$!: Observable<Person[]>;
  selectedPerson: Person | null = null;
  loading = false;
  error: string | null = null;

  constructor(private peopleService: PeopleService) {}

  ngOnInit(): void {
    this.loadPeople();
  }

  loadPeople(): void {
    this.loading = true;
    this.error = null;
    this.people$ = this.peopleService.getPeople();
  }

  onPersonSelect(person: Person): void {
    this.selectedPerson = person;
  }

  onRefresh(): void {
    this.selectedPerson = null;
    this.loadPeople();
  }

  trackByPersonId(index: number, person: Person): string {
    return person.id;
  }

  formatDate(dateString: string | null | undefined): string {
    if (!dateString) return 'Not specified';
    
    try {
      const date = new Date(dateString);
      return date.toLocaleDateString();
    } catch {
      return 'Invalid date';
    }
  }

  getFullName(person: Person): string {
    const parts = [person.firstName, person.lastName].filter(part => part);
    return parts.length > 0 ? parts.join(' ') : 'Unknown Name';
  }
}
