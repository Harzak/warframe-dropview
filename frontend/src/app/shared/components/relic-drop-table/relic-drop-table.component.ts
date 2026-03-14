import { ChangeDetectionStrategy, Component, Input, output } from '@angular/core';
import { DecimalPipe, TitleCasePipe } from '@angular/common';
import { TableModule } from 'primeng/table';
import { Button } from 'primeng/button';

import { RelicDrop } from '../../models/relic-drop.model';

@Component({
  selector: 'app-relic-drop-table',
  imports: [DecimalPipe, TitleCasePipe, TableModule, Button],
  templateUrl: './relic-drop-table.component.html',
  styleUrl: './relic-drop-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class RelicDropTableComponent {
  @Input() drops: RelicDrop[] = [];
  @Input() hasMore = false;
  @Input() loading = false;

  readonly loadMore = output<void>();
}
