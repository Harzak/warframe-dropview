export const RARITIES = ['Common', 'Uncommon', 'Rare', 'Legendary'] as const;

export type Rarity = (typeof RARITIES)[number];
