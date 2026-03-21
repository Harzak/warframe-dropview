export const MISSION_TYPES = [
  'Assassination',
  'Defense',
  'Extermination',
  'Interception',
  'Mobile Defense',
  'Rescue',
  'Sabotage',
  'Survival',
  'Spy',
  'Hive',
  'Disruption',
] as const;

export type MissionType = (typeof MISSION_TYPES)[number];
