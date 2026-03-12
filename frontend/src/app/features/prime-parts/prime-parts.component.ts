import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { toSignal, toObservable } from '@angular/core/rxjs-interop';
import { combineLatest, switchMap, of } from 'rxjs';
import { MultiSelectModule } from 'primeng/multiselect';
import { FloatLabelModule } from 'primeng/floatlabel';

import { DropApiService } from '../../core/services/drop-api.service';
import { RelicDropTableComponent } from '../../shared/components/relic-drop-table/relic-drop-table.component';

@Component({
  selector: 'app-prime-parts',
  imports: [RelicDropTableComponent, MultiSelectModule, FloatLabelModule, FormsModule],
  templateUrl: './prime-parts.component.html',
  styleUrl: './prime-parts.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PrimePartsComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly api = inject(DropApiService);

  rarities: string[] = ['Common', 'Uncommon', 'Rare', 'Legendary'];
  selectedRarities = signal<string[]>([]);

  tiers: string[] = ['Lith', 'Meso', 'Neo', 'Axi'];
  selectedTiers = signal<string[]>([]);

  refinements: string[] = ['Intact', 'Exceptional', 'Flawless', 'Radiant'];
  selectedRefinements = signal<string[]>(['Radiant']);

  result = toSignal(
    combineLatest([
      this.route.queryParams,
      toObservable(this.selectedRarities),
      toObservable(this.selectedTiers),
      toObservable(this.selectedRefinements),
    ]).pipe(
      switchMap(([params, rarities, relicTiers, refinements]) => {
        if (!params['itemName']) {
          return of(null);
        }
        return this.api.searchPrimeParts({
          itemName: params['itemName'],
          dropRarities: rarities.join(','),
          relicTiers: relicTiers.join(','),
          refinements: refinements.join(','),
        });
      }),
    ),
  );
}
