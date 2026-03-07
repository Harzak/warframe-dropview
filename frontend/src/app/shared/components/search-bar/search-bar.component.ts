import { ChangeDetectionStrategy, ChangeDetectorRef, Component, inject } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { FormsModule } from '@angular/forms';

type SearchContext = 'prime-parts' | 'relics' | 'mods';

@Component({
  selector: 'app-search-bar',
  imports: [FormsModule],
  templateUrl: './search-bar.component.html',
  styleUrl: './search-bar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SearchBarComponent {
  private readonly router = inject(Router);
  private readonly cdr = inject(ChangeDetectorRef);

  readonly contexts: { value: SearchContext; label: string }[] = [
    { value: 'prime-parts', label: 'Prime Parts' },
    { value: 'relics', label: 'Relics' },
    { value: 'mods', label: 'Mods' },
  ];

  searchTerm = '';
  activeContext: SearchContext = this.contextFromUrl(this.router.url);

  constructor() {
    this.syncFromUrl();
    this.router.events
      .pipe(filter(e => e instanceof NavigationEnd))
      .subscribe(() => {
        this.syncFromUrl();
        this.cdr.markForCheck();
      });
  }

  onContextChange(context: SearchContext): void {
    this.router.navigate([`/${context}`]);
  }

  onSearch(): void {
    const queryParams: Record<string, string> = {};
    this.router.navigate([`/${this.activeContext}`], { queryParams });
  }

  private contextFromUrl(url: string): SearchContext {
    if (url.startsWith('/relics')) return 'relics';
    if (url.startsWith('/mods')) return 'mods';
    return 'prime-parts';
  }

  private syncFromUrl(): void {
    this.activeContext = this.contextFromUrl(this.router.url);
  }
}
