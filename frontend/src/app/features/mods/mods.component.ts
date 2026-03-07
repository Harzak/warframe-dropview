import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';

import { DropApiService } from '../../core/services/drop-api.service';
import { MissionDropTableComponent } from '../../shared/components/mission-drop-table/mission-drop-table.component';
import { EnemyDropTableComponent } from '../../shared/components/enemy-drop-table/enemy-drop-table.component';

@Component({
  selector: 'app-mods',
  imports: [MissionDropTableComponent, EnemyDropTableComponent],
  templateUrl: './mods.component.html',
  styleUrl: './mods.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ModsComponent {
  private readonly api = inject(DropApiService);

  result = toSignal(this.api.searchMods());
}
