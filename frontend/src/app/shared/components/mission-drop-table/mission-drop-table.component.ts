import { ChangeDetectionStrategy, Component, Input, output } from '@angular/core';
import { DecimalPipe, TitleCasePipe } from '@angular/common';
import { TableModule } from 'primeng/table';
import { Button } from 'primeng/button';

import { MissionDrop } from '../../models/mission-drop.model';

@Component({
  selector: 'app-mission-drop-table',
  imports: [DecimalPipe, TitleCasePipe, TableModule, Button],
  templateUrl: './mission-drop-table.component.html',
  styleUrl: './mission-drop-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class MissionDropTableComponent {
  @Input() drops: MissionDrop[] = [];
  @Input() hasMore = false;
  @Input() loading = false;

  readonly loadMore = output<void>();
}
