import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { DecimalPipe, TitleCasePipe } from '@angular/common';
import { TableModule } from 'primeng/table';
import { Button } from 'primeng/button';

import { MissionDrop } from '../../models';

@Component({
  selector: 'app-mission-drop-table',
  imports: [DecimalPipe, TitleCasePipe, TableModule, Button],
  templateUrl: './mission-drop-table.component.html',
  styleUrl: './mission-drop-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MissionDropTableComponent {
  readonly drops = input<MissionDrop[]>([]);
  readonly hasMore = input(false);
  readonly loading = input(false);

  readonly loadMore = output<void>();
}
