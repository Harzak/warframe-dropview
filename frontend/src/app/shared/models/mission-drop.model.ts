import { Drop } from './drop.model';

export interface MissionDrop extends Drop {
  missionName: string;   /** Node name, e.g. 'Apollodorus', 'Lares' */
  missionType: string;  /** Mission type, e.g. 'Assassination', 'Defense', 'Survival', 'Spy' */
  planet: string;  /** Planet name, e.g. 'Earth', 'Mars', 'Saturn' */
  rotation: string;  /** Reward rotation: 'A', 'B', 'C' */
}