import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { combineLatest, switchMap, of, tap } from 'rxjs';
import { MultiSelectModule } from 'primeng/multiselect';
import { FloatLabelModule } from 'primeng/floatlabel';
import { Observable } from 'rxjs';

import { DropApiService } from '../../core/services/drop-api.service';
import { MissionDropTableComponent } from '../../shared/components/mission-drop-table/mission-drop-table.component';
import { EnemyDropTableComponent } from '../../shared/components/enemy-drop-table/enemy-drop-table.component';
import { MissionDrop } from '../../shared/models/mission-drop.model';
import { EnemyDrop } from '../../shared/models/enemy-drop.model';
import { SearchResult } from '../../shared/models/search-result.model';

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

  private readonly LIMIT = 20;
  private _offset = 0;

  public rarities: string[];
  public selectedRarities = signal<string[]>([]);

  public types: string[];
  public selectedTypes = signal<string[]>([]);

  public missionTypes: string[];
  public selectedMissionTypes = signal<string[]>([]);

  public missionDrops = signal<MissionDrop[]>([]);
  public enemyDrops = signal<EnemyDrop[]>([]);
  public hasMore = signal(false);
  public loading = signal(false);
  public itemName = signal<string | null>(null);

  constructor() {
    this.rarities = ['Common', 'Uncommon', 'Rare', 'Legendary'];
    this.types = ['Mod', 'Arcane'];
    this.missionTypes = ['Assassination', 'Defense', 'Extermination', 'Interception', 'Mobile Defense', 'Rescue', 'Sabotage', 'Survival', 'Spy', 'Hive', 'Disruption'];

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
      toObservable(this.selectedTypes),
      toObservable(this.selectedMissionTypes),
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
    this.missionDrops.set([]);
    this.enemyDrops.set([]);
    this.hasMore.set(false);
  }

  private fetchPage(offset: number): Observable<SearchResult> {
    return this._api.searchMods({
      itemName: this.itemName()!,
      dropRarities: this.selectedRarities().join(','),
      itemTypes: this.selectedTypes().join(','),
      missionTypes: this.selectedMissionTypes().join(','),
      offset,
      limit: this.LIMIT,
    });
  }

  private handleResult(result: SearchResult | null): void {
    this.loading.set(false);
    if (!result) return;
    this.missionDrops.update(current => [...current, ...result.missionDrops]);
    this.enemyDrops.update(current => [...current, ...result.enemyDrops]);
    this.hasMore.set(result.missionDrops.length === this.LIMIT || result.enemyDrops.length === this.LIMIT);
  }
}
