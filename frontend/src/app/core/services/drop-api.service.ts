import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { SearchResult } from '../../shared/models/search-result.model';

export interface PrimePartsQuery {
  itemName?: string;
  dropType?: string;
  partType?: string;
  relicTier?: string;
  dropRarity?: string;
}

export interface RelicsQuery {
  relicName?: string;
}

@Injectable({ providedIn: 'root' })
export class DropApiService {
  private readonly http = inject(HttpClient);
  searchPrimeParts(query: PrimePartsQuery = {}): Observable<SearchResult> {
    let params = new HttpParams();
    if (query.itemName) params = params.set('itemName', query.itemName.toLowerCase());
    if (query.dropType) params = params.set('dropType', query.dropType.toLowerCase());
    if (query.partType) params = params.set('partType', query.partType.toLowerCase());
    if (query.relicTier) params = params.set('relicTier', query.relicTier.toLowerCase());
    if (query.dropRarity) params = params.set('dropRarity', query.dropRarity.toLowerCase());
    return this.http.get<SearchResult>(`/primeparts/search?limit=20`, {
      params,
    });
  }

  searchRelics(query: RelicsQuery = {}): Observable<SearchResult> {
    let params = new HttpParams();
    if (query.relicName) params = params.set('relicName', query.relicName);
    return this.http.get<SearchResult>(`/relics/search`, {
      params,
    });
  }

  searchMods(): Observable<SearchResult> {
    return this.http.get<SearchResult>(`/mods/search`);
  }
}
