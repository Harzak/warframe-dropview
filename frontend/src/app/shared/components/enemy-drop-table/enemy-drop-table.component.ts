import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { DecimalPipe, TitleCasePipe } from '@angular/common';
import { TableModule } from 'primeng/table';
import { Button } from 'primeng/button';

import { EnemyDrop } from '../../models';

@Component({
  selector: 'app-enemy-drop-table',
  imports: [DecimalPipe, TitleCasePipe, TableModule, Button],
  templateUrl: './enemy-drop-table.component.html',
  styleUrl: './enemy-drop-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EnemyDropTableComponent {
  readonly drops = input<EnemyDrop[]>([]);
  readonly hasMore = input(false);
  readonly loading = input(false);

  readonly loadMore = output<void>();
}
