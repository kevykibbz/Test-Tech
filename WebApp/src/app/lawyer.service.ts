import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Lawyer } from './lawyer';
import { environment } from '../environments/environment';

export interface LawyerCreateRequest {
  firstName: string;
  lastName: string;
  companyName: string;
}

@Injectable({
  providedIn: 'root'
})
export class LawyerService {
  private readonly baseUrl = `${environment.apiServerBase}/lawyer`;

  constructor(private http: HttpClient) {}

  getLawyers(skip: number = 0, take: number = 100): Observable<Lawyer[]> {
    return this.http.get<Lawyer[]>(`${this.baseUrl}?skip=${skip}&take=${take}`);
  }

  getLawyer(id: string): Observable<Lawyer> {
    return this.http.get<Lawyer>(`${this.baseUrl}/${id}`);
  }

  createLawyer(lawyer: LawyerCreateRequest): Observable<Lawyer> {
    return this.http.post<Lawyer>(this.baseUrl, lawyer);
  }

  updateLawyer(id: string, lawyer: Partial<LawyerCreateRequest>): Observable<Lawyer> {
    return this.http.put<Lawyer>(`${this.baseUrl}/${id}`, lawyer);
  }

  deleteLawyer(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
