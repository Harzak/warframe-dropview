import { ChangeDetectionStrategy, Component, DestroyRef, inject, signal } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { filter } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { Select } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';

import { SearchContext } from '../../../shared/models';

@Component({
  selector: 'app-search-bar',
  imports: [FormsModule, Select, InputTextModule],
  templateUrl: './search-bar.component.html',
  styleUrl: './search-bar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SearchBarComponent {
  private readonly _router = inject(Router);
  private readonly _destroyRef = inject(DestroyRef);

  readonly contexts: { value: SearchContext; label: string }[] = [
    { value: 'prime-parts', label: 'Prime Parts' },
    { value: 'relics', label: 'Relics' },
    { value: 'mods', label: 'Mods' },
  ];

  readonly searchTerm = signal('');
  readonly activeContext = signal<SearchContext>(this.contextFromUrl(this._router.url));

  constructor() {
    this._router.events.pipe(
      filter((e): e is NavigationEnd => e instanceof NavigationEnd),
      takeUntilDestroyed(this._destroyRef),
    ).subscribe(() => {
      this.activeContext.set(this.contextFromUrl(this._router.url));
    });
  }

  onContextChange(): void {
    this.navigateToActiveContext();
  }

  onSearch(): void {
    this.navigateToActiveContext();
  }

  private contextFromUrl(url: string): SearchContext {
    if (url.startsWith('/relics')) return 'relics';
    if (url.startsWith('/mods')) return 'mods';
    return 'prime-parts';
  }

  private navigateToActiveContext(): void {
    const term = this.searchTerm().trim();
    if (!term) return;
    const queryParams: Record<string, string> = { itemName: term };
    this._router.navigate([`/${this.activeContext()}`], { queryParams });
  }
}
