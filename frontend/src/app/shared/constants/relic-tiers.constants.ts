export const RELIC_TIERS = ['Lith', 'Meso', 'Neo', 'Axi'] as const;

export type RelicTier = (typeof RELIC_TIERS)[number];
