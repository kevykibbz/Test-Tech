import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ContractExtractionResult } from './contract-extraction-result';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ContractExtractionService {
  private readonly baseUrl = `${environment.apiServerBase}/contractextraction`;

  constructor(private http: HttpClient) {}

  extractFromDefaultContract(): Observable<ContractExtractionResult> {
    return this.http.get<ContractExtractionResult>(this.baseUrl);
  }

  extractFromText(contractText: string): Observable<ContractExtractionResult> {
    return this.http.post<ContractExtractionResult>(`${this.baseUrl}/extract-text`, `"${contractText}"`, {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  extractFromFile(file: File): Observable<ContractExtractionResult> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<ContractExtractionResult>(`${this.baseUrl}/extract-file`, formData);
  }
}
