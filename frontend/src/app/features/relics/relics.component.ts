import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, switchMap, of, tap } from 'rxjs';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { MultiSelectModule } from 'primeng/multiselect';
import { FloatLabelModule } from 'primeng/floatlabel';
import { FormsModule } from '@angular/forms';

import { DropApiService } from '../../core';
import { MissionDropTableComponent } from '../../shared/components';
import { MissionDrop, SearchResult } from '../../shared/models';
import { RARITIES, RELIC_TIERS, MISSION_TYPES } from '../../shared/constants';
import { PaginatedSearch } from '../../shared/utils/paginated-search';

const LIMIT = 20;

@Component({
  selector: 'app-relics',
  imports: [MissionDropTableComponent, MultiSelectModule, FloatLabelModule, FormsModule],
  templateUrl: './relics.component.html',
  styleUrl: './relics.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RelicsComponent {
  private readonly _route = inject(ActivatedRoute);
  private readonly _api = inject(DropApiService);
  private readonly _destroyRef = inject(DestroyRef);

  readonly rarities = [...RARITIES];
  readonly selectedRarities = signal<string[]>([]);

  readonly tiers = [...RELIC_TIERS];
  readonly selectedTiers = signal<string[]>([]);

  readonly missionTypes = [...MISSION_TYPES];
  readonly selectedMissionTypes = signal<string[]>([]);

  readonly itemName = signal<string | null>(null);

  readonly search = new PaginatedSearch<SearchResult, MissionDrop>({
    limit: LIMIT,
    fetchFn: (offset) => this._api.searchRelics({
      itemName: this.itemName()!,
      dropRarities: this.selectedRarities().join(','),
      relicTiers: this.selectedTiers().join(','),
      missionTypes: this.selectedMissionTypes().join(','),
      offset,
      limit: LIMIT,
    }),
    extractItems: (result) => result.missionDrops,
    hasMore: (result, limit) => result.missionDrops.length === limit,
    destroyRef: this._destroyRef,
  });

  constructor() {
    this.watchFilters();
  }

  private watchFilters(): void {
    combineLatest([
      this._route.queryParams,
      toObservable(this.selectedRarities),
      toObservable(this.selectedTiers),
      toObservable(this.selectedMissionTypes),
    ]).pipe(
      tap(([params]) => {
        this.itemName.set(params['itemName'] ?? null);
        this.search.reset();
      }),
      switchMap(([params]) => {
        if (!params['itemName']) return of(null);
        this.search.search();
        return of(null);
      }),
      takeUntilDestroyed(this._destroyRef),
    ).subscribe();
  }
}
