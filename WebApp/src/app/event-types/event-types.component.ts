import { Component, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { EventType } from '../event-type';
import { EventTypeGroup } from '../event-type-group';
import { EventService } from '../event.service';

@Component({
  selector: 'app-event-types',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './event-types.component.html',
  styleUrls: ['./event-types.component.scss']
})
export class EventTypesComponent implements OnInit {
  // State signals
  eventTypes = signal<EventType[]>([]);
  eventTypeGroups = signal<EventTypeGroup[]>([]);
  selectedEventType = signal<EventType | null>(null);
  selectedGroup = signal<EventTypeGroup | null>(null);
  loading = signal(false);
  error = signal<string | null>(null);

  // Computed signals
  filteredEventTypes = computed(() => {
    const group = this.selectedGroup();
    const allEventTypes = this.eventTypes();
    return group ? allEventTypes.filter(et => et.group_id === group.id) : [];
  });

  hasSelection = computed(() => !!this.selectedGroup());
  hasEventTypeSelection = computed(() => !!this.selectedEventType());

  constructor(private eventService: EventService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading.set(true);
    this.error.set(null);
    
    this.eventService.getEventTypes().subscribe({
      next: (eventTypes) => {
        this.eventTypes.set(eventTypes);
        this.loading.set(false);
      },
      error: (err) => {
        this.error.set('Failed to load event types');
        this.loading.set(false);
      }
    });

    this.eventService.getEventTypeGroups().subscribe({
      next: (groups) => {
        this.eventTypeGroups.set(groups);
      },
      error: (err) => {
        this.error.set('Failed to load event type groups');
      }
    });
  }

  onGroupSelect(group: EventTypeGroup): void {
    this.selectedGroup.set(group);
    this.selectedEventType.set(null);
  }

  onEventTypeSelect(eventType: EventType): void {
    this.selectedEventType.set(eventType);
  }

  onRefresh(): void {
    this.selectedEventType.set(null);
    this.selectedGroup.set(null);
    this.loadData();
  }

  trackByGroupId(index: number, group: EventTypeGroup): string {
    return group.id;
  }

  trackByEventTypeId(index: number, eventType: EventType): string {
    return eventType.id;
  }
}
