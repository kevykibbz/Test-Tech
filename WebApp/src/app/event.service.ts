import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AsyncSubject, Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';

import { EventType } from './event-type';
import { EventTypeGroup } from './event-type-group';

@Injectable({
  providedIn: 'root',
})
export class EventService {
  private eventTypeGroups$?: AsyncSubject<EventTypeGroup[]>;
  private eventTypes$?: AsyncSubject<EventType[]>;

  constructor(private http: HttpClient) {
  }

  getEventTypeGroups(): Observable<EventTypeGroup[]> {
    if (!this.eventTypeGroups$) {
      const eventTypeGroups$ = new AsyncSubject<EventTypeGroup[]>();
      this.http.get<EventTypeGroup[]>(`/api/v1/EventTypeGroups`)
        .pipe(
          finalize(() => eventTypeGroups$.complete()),
        )
        .subscribe(
          eventTypeGroups => eventTypeGroups$.next(eventTypeGroups),
          err => {
            console.error('Error loading event type groups err');
            eventTypeGroups$.next([]);
            this.eventTypeGroups$ = undefined;
          },
        );

      this.eventTypeGroups$ = eventTypeGroups$;
    }

    return this.eventTypeGroups$;
  }

  getEventTypes(): Observable<EventType[]> {
    if (!this.eventTypes$) {
      const eventTypes$ = new AsyncSubject<EventType[]>();
      this.http.get<EventType[]>(`/api/v1/EventTypes`)
        .pipe(
          finalize(() => eventTypes$.complete()),
        )
        .subscribe(
          eventTypes => eventTypes$.next(eventTypes),
          err => {
            console.error('Error loading event types err');
            eventTypes$.next([]);
            this.eventTypes$ = undefined;
          },
        );

      this.eventTypes$ = eventTypes$;
    }

    return this.eventTypes$;
  }
}
