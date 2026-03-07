import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { DecimalPipe } from '@angular/common';

import { EnemyDrop } from '../../models/enemy-drop.model';

@Component({
  selector: 'app-enemy-drop-table',
  imports: [DecimalPipe],
  templateUrl: './enemy-drop-table.component.html',
  styleUrl: './enemy-drop-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EnemyDropTableComponent {
  @Input() drops: EnemyDrop[] = [];
}
