import { ChangeDetectionStrategy, Component, input, output } from '@angular/core';
import { DecimalPipe, TitleCasePipe } from '@angular/common';
import { TableModule } from 'primeng/table';
import { Button } from 'primeng/button';

import { RelicDrop } from '../../models';

@Component({
  selector: 'app-relic-drop-table',
  imports: [DecimalPipe, TitleCasePipe, TableModule, Button],
  templateUrl: './relic-drop-table.component.html',
  styleUrl: './relic-drop-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RelicDropTableComponent {
  readonly drops = input<RelicDrop[]>([]);
  readonly hasMore = input(false);
  readonly loading = input(false);

  readonly loadMore = output<void>();
}
