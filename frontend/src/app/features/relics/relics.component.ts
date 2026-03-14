import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { combineLatest, switchMap, of, tap, Observable } from 'rxjs';

import { DropApiService } from '../../core/services/drop-api.service';
import { MissionDropTableComponent } from '../../shared/components/mission-drop-table/mission-drop-table.component';
import { MissionDrop } from '../../shared/models/mission-drop.model';
import { SearchResult } from '../../shared/models/search-result.model';

@Component({
  selector: 'app-relics',
  imports: [MissionDropTableComponent],
  templateUrl: './relics.component.html',
  styleUrl: './relics.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class RelicsComponent {
  private readonly _route = inject(ActivatedRoute);
  private readonly _api = inject(DropApiService);
   private readonly _destroyRef = inject(DestroyRef);

  private readonly LIMIT = 20;
  private _offset = 0;

  public rarities: string[];
  public selectedRarities = signal<string[]>([]);

  public drops = signal<MissionDrop[]>([]);
  public hasMore = signal(false);
  public loading = signal(false);
  public itemName = signal<string | null>(null);

  constructor() {
    this.rarities = ['Common', 'Uncommon', 'Rare', 'Legendary'];
    this.watchFilters();
  }

  public onLoadMore(): void {
    if (this.loading()) return;
    
    this._offset += this.LIMIT;
    this.loading.set(true);
    this.fetchPage(this._offset).pipe(
      takeUntilDestroyed(this._destroyRef),
    ).subscribe(result => this.handleResult(result));
  }

  private watchFilters(): void {
    combineLatest([
      this._route.queryParams,
      toObservable(this.selectedRarities),
    ]).pipe(
      tap(([params]) => this.resetState(params['itemName'] ?? null)),
      switchMap(([params]) => {
        if (!params['itemName']) return of(null);
        this.loading.set(true);
        return this.fetchPage(0);
      }),
      takeUntilDestroyed(this._destroyRef),
    ).subscribe(result => this.handleResult(result));
  }

  private resetState(name: string | null): void {
    this.itemName.set(name);
    this._offset = 0;
    this.drops.set([]);
    this.hasMore.set(false);
  }

  private fetchPage(offset: number): Observable<SearchResult> {
    return this._api.searchRelics({
      itemName: this.itemName()!,
      dropRarities: this.selectedRarities().join(','),
      offset,
      limit: this.LIMIT,
    });
  }
  
  private handleResult(result: SearchResult | null): void {
    this.loading.set(false);
    if (!result) return;
    this.drops.update(current => [...current, ...result.missionDrops]);
    this.hasMore.set(result.missionDrops.length === this.LIMIT);
  }
}
