import { Drop } from './drop.model';

export interface EnemyDrop extends Drop {
  enemyName: string;   /** Enemy name, e.g. 'Grineer Lancer', 'Corpus Tech' */
}
