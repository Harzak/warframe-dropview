import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, switchMap, of, tap } from 'rxjs';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { MultiSelectModule } from 'primeng/multiselect';
import { FloatLabelModule } from 'primeng/floatlabel';
import { FormsModule } from '@angular/forms';

import { DropApiService } from '../../core';
import { MissionDropTableComponent, EnemyDropTableComponent } from '../../shared/components';
import { EnemyDrop, MissionDrop, SearchResult } from '../../shared/models';
import { RARITIES, ITEM_TYPES, MISSION_TYPES } from '../../shared/constants';
import { PaginatedSearch } from '../../shared/utils/paginated-search';

const LIMIT = 20;

@Component({
  selector: 'app-mods',
  imports: [MissionDropTableComponent, EnemyDropTableComponent, MultiSelectModule, FloatLabelModule, FormsModule],
  templateUrl: './mods.component.html',
  styleUrl: './mods.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ModsComponent {
  private readonly _route = inject(ActivatedRoute);
  private readonly _api = inject(DropApiService);
  private readonly _destroyRef = inject(DestroyRef);

  readonly rarities = [...RARITIES];
  readonly selectedRarities = signal<string[]>([]);

  readonly types = [...ITEM_TYPES];
  readonly selectedTypes = signal<string[]>([]);

  readonly missionTypes = [...MISSION_TYPES];
  readonly selectedMissionTypes = signal<string[]>([]);

  readonly enemyDrops = signal<EnemyDrop[]>([]);
  readonly itemName = signal<string | null>(null);

  readonly search = new PaginatedSearch<SearchResult, MissionDrop>({
    limit: LIMIT,
    fetchFn: (offset) => this._api.searchMods({
      itemName: this.itemName()!,
      dropRarities: this.selectedRarities().join(','),
      itemTypes: this.selectedTypes().join(','),
      missionTypes: this.selectedMissionTypes().join(','),
      offset,
      limit: LIMIT,
    }),
    extractItems: (result) => result.missionDrops,
    hasMore: (result, limit) => result.missionDrops.length === limit || result.enemyDrops.length === limit,
    destroyRef: this._destroyRef,
    onResult: (result) => {
      this.enemyDrops.update(current => [...current, ...result.enemyDrops]);
    },
  });

  constructor() {
    this.watchFilters();
  }

  private watchFilters(): void {
    combineLatest([
      this._route.queryParams,
      toObservable(this.selectedRarities),
      toObservable(this.selectedTypes),
      toObservable(this.selectedMissionTypes),
    ]).pipe(
      tap(([params]) => {
        this.itemName.set(params['itemName'] ?? null);
        this.enemyDrops.set([]);
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
