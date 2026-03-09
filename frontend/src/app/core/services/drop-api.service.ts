import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { SearchResult } from '../../shared/models/search-result.model';

export interface PrimePartsQuery {
  itemName?: string;
  relicTier?: string;
  dropRarity?: string;
  refinement?: string;
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
    if (query.relicTier) params = params.set('relicTiers', query.relicTier.toLowerCase());
    if (query.dropRarity) params = params.set('dropRarities', query.dropRarity.toLowerCase());
    if (query.refinement) params = params.set('refinement', query.refinement.toLowerCase());

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
