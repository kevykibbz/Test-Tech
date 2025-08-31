import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AsyncSubject, Observable, map, finalize } from 'rxjs';
import { LegalMatter } from './legal-matter';
import { LegalMatterCategory } from './legal-matter-category';
import { environment } from '../environments/environment';


export const DEFAULT_PAGE_SIZE = 15;

@Injectable({
  providedIn: 'root'
})
export class LegalMatterService {

   private legalMatterCategories$?: AsyncSubject<LegalMatterCategory[]>;
   private readonly baseUrl = `${environment.apiServerBase}/legalmatter`;

  constructor(private http: HttpClient) {
  }

  createLegalMatter(legalMatterData: Partial<LegalMatter>): Observable<Partial<LegalMatter>> {
    // Transform the data to match the backend ServiceModel format
    const transformedData = {
      MatterName: legalMatterData.matterName,
      ContractType: legalMatterData.contractType,
      Parties: legalMatterData.parties,
      EffectiveDate: legalMatterData.effectiveDate ? new Date(legalMatterData.effectiveDate).toISOString() : null,
      ExpirationDate: legalMatterData.expirationDate ? new Date(legalMatterData.expirationDate).toISOString() : null,
      GoverningLaw: legalMatterData.governingLaw,
      ContractValue: legalMatterData.contractValue,
      Status: legalMatterData.status,
      Description: legalMatterData.description,
      LawyerId: legalMatterData.lawyerId
    };

    // Remove null/undefined properties
    Object.keys(transformedData).forEach(key => 
      (transformedData[key as keyof typeof transformedData] === undefined || 
       transformedData[key as keyof typeof transformedData] === null) && 
      delete transformedData[key as keyof typeof transformedData]
    );

    return this.http.post(`${this.baseUrl}`, transformedData)
      .pipe(
        map(createdLegalMatter => new LegalMatter(createdLegalMatter)),
      );
  }

  deleteLegalMatter(legalMatterId: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${legalMatterId}`);
  }

  getLegalMatter(legalMatterId: string): Observable<LegalMatter> {
    return this.http.get(`${this.baseUrl}/${legalMatterId}`)
      .pipe(
        map(legalMatter => new LegalMatter(legalMatter)),
      );
  }

  getLegalMatterCategories(): Observable<LegalMatterCategory[]> {
    if (!this.legalMatterCategories$) {
      const legalMatterCategories$ = new AsyncSubject<LegalMatterCategory[]>();
      this.http.get<LegalMatterCategory[]>(`${environment.apiServerBase}/api/v1/LegalMatterCategories`)
        .pipe(
          finalize(() => legalMatterCategories$.complete()),
        )
        .subscribe(
          legalMatterCategories => legalMatterCategories$.next(legalMatterCategories),
          err => {
            console.error('Error loading legal matter categories', err);
            legalMatterCategories$.next([]);
            this.legalMatterCategories$ = undefined;
          },
        );

      this.legalMatterCategories$ = legalMatterCategories$;
    }

    return this.legalMatterCategories$;
  }

  getLegalMatterCategory(legalMatterCategoryId: string): Observable<LegalMatterCategory | undefined> {
    return this.getLegalMatterCategories()
      .pipe(
        map(legalMatterCategories => legalMatterCategories.find(c => c.id === legalMatterCategoryId)),
      );
  }

  getLegalMatterCount(): Observable<number> {
    return this.http.get<{ total: number }>(`${this.baseUrl}/Total`)
      .pipe(
        map(({ total }) => total),
      );
  }

  getLegalMatters(page: number, pageSize = DEFAULT_PAGE_SIZE): Observable<LegalMatter[]> {
    return this.http.get<LegalMatter[]>(`${this.baseUrl}`, {
      params: new HttpParams({
        fromObject: {
          take: `${DEFAULT_PAGE_SIZE}`,
          skip: `${Math.max(page - 1, 0) * pageSize}`,
        },
      }),
    })
      .pipe(
        map(legalMatters => (legalMatters).map(lm => new LegalMatter(lm))),
      );
  }

  updateLegalMatter(legalMatterId: string, legalMatterData: Partial<LegalMatter>): Observable<any> {
    // Helper function to safely convert dates
    const safeISODate = (date: Date | string | null | undefined): string | null => {
      if (!date) return null;
      try {
        const dateObj = new Date(date);
        // Check if the date is valid and not a default/zero date
        if (isNaN(dateObj.getTime()) || dateObj.getFullYear() < 1900) {
          return null;
        }
        return dateObj.toISOString();
      } catch {
        return null;
      }
    };

    // Transform the data to match the backend ServiceModel format
    const transformedData = {
      Id: legalMatterId,
      MatterName: legalMatterData.matterName,
      ContractType: legalMatterData.contractType,
      Parties: legalMatterData.parties,
      EffectiveDate: safeISODate(legalMatterData.effectiveDate),
      ExpirationDate: safeISODate(legalMatterData.expirationDate),
      GoverningLaw: legalMatterData.governingLaw,
      ContractValue: legalMatterData.contractValue,
      Status: legalMatterData.status,
      Description: legalMatterData.description,
      LawyerId: legalMatterData.lawyerId,
      // Don't include CreatedAt in update - let the backend handle it
      LastModified: new Date().toISOString()
    };

    // Remove null/undefined properties
    Object.keys(transformedData).forEach(key => {
      const value = transformedData[key as keyof typeof transformedData];
      if (value === null || value === undefined) {
        delete transformedData[key as keyof typeof transformedData];
      }
    });

    console.log('Original data:', legalMatterData);
    console.log('Transformed data for API:', transformedData);

    return this.http.put(`${this.baseUrl}/${legalMatterId}`, transformedData);
  }

  assignLawyerToLegalMatter(legalMatterId: string, lawyerId: string): Observable<LegalMatter> {
    return this.http.put<LegalMatter>(`${this.baseUrl}/${legalMatterId}/assign-lawyer`, { lawyerId });
  }

  unassignLawyerFromLegalMatter(legalMatterId: string): Observable<LegalMatter> {
    return this.http.put<LegalMatter>(`${this.baseUrl}/${legalMatterId}/unassign-lawyer`, {});
  }

  getLegalMattersByLawyer(lawyerId: string): Observable<LegalMatter[]> {
    return this.http.get<LegalMatter[]>(`${this.baseUrl}/by-lawyer/${lawyerId}`)
      .pipe(
        map(legalMatters => legalMatters.map(lm => new LegalMatter(lm)))
      );
  }
}

