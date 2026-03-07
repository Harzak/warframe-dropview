import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { switchMap } from 'rxjs';

import { DropApiService } from '../../core/services/drop-api.service';
import { RelicDropTableComponent } from '../../shared/components/relic-drop-table/relic-drop-table.component';

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

  result = () => {};
  // result = toSignal(
  //   this.route.queryParams.pipe(
  //     switchMap(params =>
  //       this.api.searchPrimeParts({
  //         partType: params['partType'],
  //         relicTier: params['relicTier'],
  //         dropRarity: params['dropRarity'],
  //       })
  //     )
  //   )
  // );
}
