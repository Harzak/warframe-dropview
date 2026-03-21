import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { SearchResult, PrimePartsQuery, RelicsQuery, ModsQuery } from '../../shared/models';

@Injectable({ providedIn: 'root' })
export class DropApiService {

  private readonly _http = inject(HttpClient);

  searchPrimeParts(query: PrimePartsQuery = {}): Observable<SearchResult> {
    return this._http.get<SearchResult>('/primeparts/search', {
      params: this.buildParams(query),
    });
  }

  searchRelics(query: RelicsQuery = {}): Observable<SearchResult> {
    return this._http.get<SearchResult>('/relics/search', {
      params: this.buildParams(query),
    });
  }

  searchMods(query: ModsQuery = {}): Observable<SearchResult> {
    return this._http.get<SearchResult>('/mods/search', {
      params: this.buildParams(query),
    });
  }

  private buildParams(query: object): HttpParams {
    let params = new HttpParams();
    for (const [key, value] of Object.entries(query)) {
      if (value == null || value === '') continue;
      const paramValue = typeof value === 'string' ? value.toLowerCase() : String(value);
      params = params.set(key, paramValue);
    }
    return params;
  }
}
