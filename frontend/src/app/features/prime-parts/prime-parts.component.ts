import { ChangeDetectionStrategy, Component, inject, Signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { switchMap } from 'rxjs';
import { of } from 'rxjs';

import { DropApiService } from '../../core/services/drop-api.service';
import { RelicDropTableComponent } from '../../shared/components/relic-drop-table/relic-drop-table.component';
import { SearchResult } from '../../shared/models/search-result.model';

@Component({
  selector: 'app-prime-parts',
  imports: [RelicDropTableComponent],
  templateUrl: './prime-parts.component.html',
  styleUrl: './prime-parts.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PrimePartsComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly api = inject(DropApiService);

  result : Signal<SearchResult | null | undefined> = toSignal(
    this.route.queryParams.pipe(
      switchMap(params => {
        if (!params['itemName']) {
          return of(null);
        }
        return this.api.searchPrimeParts({
          itemName: params['itemName'],
          partType: params['partType'],
          relicTier: params['relicTier'],
          dropRarity: params['dropRarity'],
        })
      })
    )
  );
}
