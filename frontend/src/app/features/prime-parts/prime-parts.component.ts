import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { toSignal, toObservable } from '@angular/core/rxjs-interop';
import { combineLatest, switchMap, of } from 'rxjs';
import { MultiSelectModule } from 'primeng/multiselect';

import { DropApiService } from '../../core/services/drop-api.service';
import { RelicDropTableComponent } from '../../shared/components/relic-drop-table/relic-drop-table.component';

@Component({
  selector: 'app-prime-parts',
  imports: [RelicDropTableComponent, MultiSelectModule, FormsModule],
  templateUrl: './prime-parts.component.html',
  styleUrl: './prime-parts.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PrimePartsComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly api = inject(DropApiService);

  rarities: string[] = ['Common', 'Uncommon', 'Rare', 'Legendary'];
  selectedRarities = signal<string[]>(['Rare']);

  result = toSignal(
    combineLatest([this.route.queryParams, toObservable(this.selectedRarities)]).pipe(
      switchMap(([params, rarities]) => {
        if (!params['itemName']) {
          return of(null);
        }
        return this.api.searchPrimeParts({
          itemName: params['itemName'],
          partType: params['partType'],
          relicTier: params['relicTier'],
          dropRarity: rarities.join(','),
        });
      }),
    ),
  );
}
