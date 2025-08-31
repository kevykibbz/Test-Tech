import { Component, OnInit } from '@angular/core';
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
  eventTypes$!: Observable<EventType[]>;
  eventTypeGroups$!: Observable<EventTypeGroup[]>;
  selectedEventType: EventType | null = null;
  selectedGroup: EventTypeGroup | null = null;
  filteredEventTypes: EventType[] = [];
  loading = false;
  error: string | null = null;

  constructor(private eventService: EventService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.loading = true;
    this.error = null;
    this.eventTypes$ = this.eventService.getEventTypes();
    this.eventTypeGroups$ = this.eventService.getEventTypeGroups();
  }

  onGroupSelect(group: EventTypeGroup): void {
    this.selectedGroup = group;
    this.selectedEventType = null;
    
    this.eventTypes$.subscribe(eventTypes => {
      this.filteredEventTypes = eventTypes.filter(et => et.group_id === group.id);
    });
  }

  onEventTypeSelect(eventType: EventType): void {
    this.selectedEventType = eventType;
  }

  onRefresh(): void {
    this.selectedEventType = null;
    this.selectedGroup = null;
    this.filteredEventTypes = [];
    this.loadData();
  }

  trackByGroupId(index: number, group: EventTypeGroup): string {
    return group.id;
  }

  trackByEventTypeId(index: number, eventType: EventType): string {
    return eventType.id;
  }
}
