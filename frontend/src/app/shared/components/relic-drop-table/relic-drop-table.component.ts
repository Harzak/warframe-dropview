import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { TableModule } from 'primeng/table';

import { RelicDrop } from '../../models/relic-drop.model';

@Component({
  selector: 'app-relic-drop-table',
  imports: [DecimalPipe, TableModule],
  templateUrl: './relic-drop-table.component.html',
  styleUrl: './relic-drop-table.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RelicDropTableComponent {
  @Input() drops: RelicDrop[] = [];
}
