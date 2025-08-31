import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LegalMatterCategory } from './legal-matter-category';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class LegalMatterCategoryService {
  private readonly baseUrl = `${environment.apiServerBase}/api/v1/LegalMatterCategories`;

  constructor(private http: HttpClient) {}

  getCategories(): Observable<LegalMatterCategory[]> {
    return this.http.get<LegalMatterCategory[]>(this.baseUrl);
  }

  getCategory(id: string): Observable<LegalMatterCategory> {
    return this.http.get<LegalMatterCategory>(`${this.baseUrl}/${id}`);
  }

  createCategory(category: Omit<LegalMatterCategory, 'id'>): Observable<LegalMatterCategory> {
    return this.http.post<LegalMatterCategory>(this.baseUrl, category);
  }

  updateCategory(id: string, category: Partial<LegalMatterCategory>): Observable<LegalMatterCategory> {
    return this.http.put<LegalMatterCategory>(`${this.baseUrl}/${id}`, category);
  }

  deleteCategory(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
