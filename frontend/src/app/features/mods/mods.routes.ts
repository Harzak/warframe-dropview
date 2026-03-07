import { Routes } from '@angular/router';

export const MODS_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./mods.component').then(m => m.ModsComponent),
  },
];
