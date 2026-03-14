import { ChangeDetectionStrategy, Component, Input, output } from '@angular/core';
import { DecimalPipe, TitleCasePipe } from '@angular/common';
import { TableModule } from 'primeng/table';
import { Button } from 'primeng/button';

import { EnemyDrop } from '../../models/enemy-drop.model';

@Component({
  selector: 'app-enemy-drop-table',
  imports: [DecimalPipe, TitleCasePipe, TableModule, Button],
  templateUrl: './enemy-drop-table.component.html',
  styleUrl: './enemy-drop-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class EnemyDropTableComponent {
  @Input() drops: EnemyDrop[] = [];
  @Input() hasMore = false;
  @Input() loading = false;

  readonly loadMore = output<void>();
}
