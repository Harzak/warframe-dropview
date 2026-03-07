import { Routes } from '@angular/router';

export const PRIME_PARTS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./prime-parts.component').then(m => m.PrimePartsComponent),
  },
];
