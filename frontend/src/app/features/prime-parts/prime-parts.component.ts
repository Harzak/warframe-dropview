import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, switchMap, of, tap } from 'rxjs';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { MultiSelectModule } from 'primeng/multiselect';
import { FloatLabelModule } from 'primeng/floatlabel';
import { FormsModule } from '@angular/forms';

import { DropApiService } from '../../core';
import { RelicDropTableComponent } from '../../shared/components';
import { RelicDrop, SearchResult } from '../../shared/models';
import { RARITIES, RELIC_TIERS, REFINEMENTS } from '../../shared/constants';
import { PaginatedSearch } from '../../shared/utils/paginated-search';

const LIMIT = 20;

@Component({
  selector: 'app-prime-parts',
  imports: [RelicDropTableComponent, MultiSelectModule, FloatLabelModule, FormsModule],
  templateUrl: './prime-parts.component.html',
  styleUrl: './prime-parts.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PrimePartsComponent {
  private readonly _route = inject(ActivatedRoute);
  private readonly _api = inject(DropApiService);
  private readonly _destroyRef = inject(DestroyRef);

  readonly rarities = [...RARITIES];
  readonly selectedRarities = signal<string[]>([]);

  readonly tiers = [...RELIC_TIERS];
  readonly selectedTiers = signal<string[]>([]);

  readonly refinements = [...REFINEMENTS];
  readonly selectedRefinements = signal<string[]>(['Radiant']);

  readonly itemName = signal<string | null>(null);

  readonly search = new PaginatedSearch<SearchResult, RelicDrop>({
    limit: LIMIT,
    fetchFn: (offset) => this._api.searchPrimeParts({
      itemName: this.itemName()!,
      dropRarities: this.selectedRarities().join(','),
      relicTiers: this.selectedTiers().join(','),
      refinements: this.selectedRefinements().join(','),
      offset,
      limit: LIMIT,
    }),
    extractItems: (result) => result.relicDrops,
    hasMore: (result, limit) => result.relicDrops.length === limit,
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
      toObservable(this.selectedRefinements),
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
