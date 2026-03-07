import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { switchMap } from 'rxjs';

import { DropApiService } from '../../core/services/drop-api.service';
import { MissionDropTableComponent } from '../../shared/components/mission-drop-table/mission-drop-table.component';

@Component({
  selector: 'app-relics',
  imports: [MissionDropTableComponent],
  templateUrl: './relics.component.html',
  styleUrl: './relics.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RelicsComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly api = inject(DropApiService);

  result = toSignal(
    this.route.queryParams.pipe(
      switchMap(params =>
        this.api.searchRelics({
          relicName: params['relicName'],
        })
      )
    )
  );
}
