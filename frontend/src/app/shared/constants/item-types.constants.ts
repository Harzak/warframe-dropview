export const ITEM_TYPES = ['Mod', 'Arcane'] as const;

export type ItemType = (typeof ITEM_TYPES)[number];
