import { Component, OnInit, signal, computed } from '@angular/core';
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
  // State signals
  people = signal<Person[]>([]);
  selectedPerson = signal<Person | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  // Computed signals
  hasPeople = computed(() => this.people().length > 0);
  hasSelection = computed(() => !!this.selectedPerson());

  constructor(private peopleService: PeopleService) {}

  ngOnInit(): void {
    this.loadPeople();
  }

  loadPeople(): void {
    this.loading.set(true);
    this.error.set(null);
    
    this.peopleService.getPeople().subscribe({
      next: (people) => {
        this.people.set(people);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load people');
        this.loading.set(false);
      }
    });
  }

  onPersonSelect(person: Person): void {
    this.selectedPerson.set(person);
  }

  onRefresh(): void {
    this.selectedPerson.set(null);
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
