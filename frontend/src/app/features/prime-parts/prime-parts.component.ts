import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { combineLatest, switchMap, of, tap, Observable } from 'rxjs';
import { MultiSelectModule } from 'primeng/multiselect';
import { FloatLabelModule } from 'primeng/floatlabel';

import { DropApiService } from '../../core/services/drop-api.service';
import { RelicDropTableComponent } from '../../shared/components/relic-drop-table/relic-drop-table.component';
import { RelicDrop } from '../../shared/models/relic-drop.model';
import { SearchResult } from '../../shared/models/search-result.model';

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

  private readonly LIMIT = 20;
  private _offset = 0;

  public rarities: string[];
  public selectedRarities = signal<string[]>([]);

  public tiers: string[];
  public selectedTiers = signal<string[]>([]);

  public refinements: string[];
  public selectedRefinements = signal<string[]>(['Radiant']);

  public drops = signal<RelicDrop[]>([]);
  public hasMore = signal(false);
  public loading = signal(false);
  public itemName = signal<string | null>(null);

   constructor() {
    this.rarities = ['Common', 'Uncommon', 'Rare', 'Legendary'];
    this.tiers = ['Lith', 'Meso', 'Neo', 'Axi'];
    this.refinements = ['Intact', 'Exceptional', 'Flawless', 'Radiant'];
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
      toObservable(this.selectedTiers),
      toObservable(this.selectedRefinements),
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
    return this._api.searchPrimeParts({
      itemName: this.itemName()!,
      dropRarities: this.selectedRarities().join(','),
      relicTiers: this.selectedTiers().join(','),
      refinements: this.selectedRefinements().join(','),
      offset,
      limit: this.LIMIT,
    });
  }

  private handleResult(result: SearchResult | null): void {
    this.loading.set(false);
    if (!result) return;
    this.drops.update(current => [...current, ...result.relicDrops]);
    this.hasMore.set(result.relicDrops.length === this.LIMIT);
  }
}
