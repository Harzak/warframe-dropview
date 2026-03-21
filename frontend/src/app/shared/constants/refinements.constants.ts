export const REFINEMENTS = ['Intact', 'Exceptional', 'Flawless', 'Radiant'] as const;

export type Refinement = (typeof REFINEMENTS)[number];
