import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { DecimalPipe } from '@angular/common';

import { MissionDrop } from '../../models/mission-drop.model';

@Component({
  selector: 'app-mission-drop-table',
  imports: [DecimalPipe],
  templateUrl: './mission-drop-table.component.html',
  styleUrl: './mission-drop-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class MissionDropTableComponent {
  @Input() drops: MissionDrop[] = [];
}
