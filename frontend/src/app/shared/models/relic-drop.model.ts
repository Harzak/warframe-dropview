import { Drop } from './drop.model';

export interface RelicDrop extends Drop {
  partType: string;   /** Warframe part category: 'Blueprint', 'Neuroptics', 'Chassis', 'Systems', 'Barrel' … */
  relicTier: string;   /** 'Lith', 'Meso', 'Neo', 'Axi', 'Requiem' */
  relicCode: string;  /** Relic identifier code, e.g. 'A1', 'S3', 'Requiem S' */
}
