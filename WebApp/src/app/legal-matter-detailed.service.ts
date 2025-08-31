import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LegalMatterDetailed, LegalMatterCreateUpdate } from './legal-matter-detailed';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LegalMatterService {
  private readonly baseUrl = `${environment.apiServerBase}/LegalMatter`;

  constructor(private http: HttpClient) {}

  getMatters(skip: number = 0, take: number = 100): Observable<LegalMatterDetailed[]> {
    return this.http.get<LegalMatterDetailed[]>(`${this.baseUrl}?skip=${skip}&take=${take}`);
  }

  getMatter(id: string): Observable<LegalMatterDetailed> {
    return this.http.get<LegalMatterDetailed>(`${this.baseUrl}/${id}`);
  }

  getSampleMatter(): Observable<LegalMatterDetailed> {
    return this.http.get<LegalMatterDetailed>(`${this.baseUrl}/sample`);
  }

  createMatter(matter: LegalMatterCreateUpdate): Observable<LegalMatterDetailed> {
    return this.http.post<LegalMatterDetailed>(this.baseUrl, matter);
  }

  updateMatter(id: string, matter: Partial<LegalMatterCreateUpdate>): Observable<LegalMatterDetailed> {
    return this.http.put<LegalMatterDetailed>(`${this.baseUrl}/${id}`, matter);
  }

  deleteMatter(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  assignLawyer(matterId: string, lawyerId: string): Observable<LegalMatterDetailed> {
    return this.http.put<LegalMatterDetailed>(`${this.baseUrl}/${matterId}/assign-lawyer/${lawyerId}`, {});
  }

  unassignLawyer(matterId: string): Observable<LegalMatterDetailed> {
    return this.http.put<LegalMatterDetailed>(`${this.baseUrl}/${matterId}/unassign-lawyer`, {});
  }
}
