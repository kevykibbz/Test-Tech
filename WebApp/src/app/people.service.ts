import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AsyncSubject, finalize, map, Observable } from 'rxjs';
import { Person } from './person';

@Injectable({
  providedIn: 'root'
})
export class PeopleService {

  private people$?: AsyncSubject<Person[]>;

  constructor(private http: HttpClient) {
  }

  getPeople(): Observable<Person[]> {
    if (!this.people$) {
      const people$ = new AsyncSubject<Person[]>();
      this.http.get<Person[]>(`/api/v1/People`)
        .pipe(
          finalize(() => people$.complete()),
        )
        .subscribe(
          people => people$.next(people),
          err => {
            console.error('Error loading people', err);
            people$.next([]);
            this.people$ = undefined;
          },
        );

      this.people$ = people$;
    }

    return this.people$;
  }

  getPerson(personId: string): Observable<Person | undefined> {
    return this.getPeople()
      .pipe(
        map(people => people.find(p => p.id === personId)),
      );
  }
}
