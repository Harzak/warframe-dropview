import { Routes } from '@angular/router';

export const RELICS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./relics.component').then(m => m.RelicsComponent),
  },
];
