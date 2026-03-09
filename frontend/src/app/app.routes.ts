import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'prime-parts',
    loadChildren: () =>
      import('./features/prime-parts/prime-parts.routes').then(
        (m) => m.PRIME_PARTS_ROUTES,
      ),
  },
  {
    path: 'relics',
    loadChildren: () =>
      import('./features/relics/relics.routes').then((m) => m.RELICS_ROUTES),
  },
  {
    path: 'mods',
    loadChildren: () =>
      import('./features/mods/mods.routes').then((m) => m.MODS_ROUTES),
  }
];
