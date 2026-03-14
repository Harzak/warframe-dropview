import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';

import { SearchResult } from '../../shared/models/search-result.model';
import { PrimePartsQuery } from '../../shared/models/primePartsQuery.model';
import { RelicsQuery } from '../../shared/models/relicsQuery.model';

@Injectable({ providedIn: 'root' })
export class DropApiService {

  private readonly _http = inject(HttpClient);

  public searchPrimeParts(query: PrimePartsQuery = {}): Observable<SearchResult> {
    let params = new HttpParams();
    
    if (query.itemName) params = params.set('itemName', query.itemName.toLowerCase());
    if (query.relicTiers) params = params.set('relicTiers', query.relicTiers.toLowerCase());
    if (query.dropRarities) params = params.set('dropRarities', query.dropRarities.toLowerCase());
    if (query.refinements) params = params.set('refinements', query.refinements.toLowerCase());
    if (query.offset != null) params = params.set('offset', query.offset);
    if (query.limit != null) params = params.set('limit', query.limit);

    return this._http.get<SearchResult>('/primeparts/search', {
      params,
    });
  }

  public searchRelics(query: RelicsQuery = {}): Observable<SearchResult> {
    let params = new HttpParams();
    if (query.relicName) params = params.set('relicName', query.relicName);
    return this._http.get<SearchResult>(`/relics/search`, {
      params,
    });
  }

  public searchMods(): Observable<SearchResult> {
    return this._http.get<SearchResult>(`/mods/search`);
  }
}
