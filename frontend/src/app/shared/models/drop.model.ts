export interface Drop {
  name: string; 
  type: string; /** e.g. 'Resource', 'Blueprint', 'Mod', 'Relic', 'Warframe Part'...*/
  subtype?: string;
  dropRate: number; /** Drop probability as a percentage (e.g. 25.33) */
  rarity: string; /** e.g. 'Common', 'Uncommon', 'Rare', 'Legendary' */
}
