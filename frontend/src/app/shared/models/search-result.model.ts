import { EnemyDrop } from './enemy-drop.model';
import { MissionDrop } from './mission-drop.model';
import { RelicDrop } from './relic-drop.model';

export interface SearchResult {
  relicDrops: RelicDrop[];
  missionDrops: MissionDrop[];
  enemyDrops: EnemyDrop[];
}
