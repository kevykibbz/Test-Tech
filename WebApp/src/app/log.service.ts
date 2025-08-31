import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { LogEntry } from './log-entry';
import { LogQueryParameters } from './log-query-parameters';

export const DEFAULT_LOG_PAGE_SIZE = 10;

@Injectable({
  providedIn: 'root',
})
export class LogService {

  constructor(private http: HttpClient) {
  }

  getLog(queryParams?: LogQueryParameters): Observable<LogEntry[]> {
    const params = new HttpParams({ fromObject: (queryParams || {}) as any });

    return this.http.get<LogEntry[]>(`/api/v1/App/Log`, { params });
  }
}
