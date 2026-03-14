import { ChangeDetectionStrategy, ChangeDetectorRef, Component, inject } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { Select } from 'primeng/select';
import { InputTextModule } from 'primeng/inputtext';

type SearchContext = 'prime-parts' | 'relics' | 'mods';

@Component({
  selector: 'app-search-bar',
  imports: [FormsModule, Select, InputTextModule],
  templateUrl: './search-bar.component.html',
  styleUrl: './search-bar.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})

export class SearchBarComponent {

  private readonly _router = inject(Router);
  private readonly _cdr = inject(ChangeDetectorRef);

  public readonly contexts: { value: SearchContext; label: string }[] = [
    { value: 'prime-parts', label: 'Prime Parts' },
    { value: 'relics', label: 'Relics' },
    { value: 'mods', label: 'Mods' },
  ];

  public searchTerm = '';
  public activeContext: SearchContext = this.contextFromUrl(this._router.url);

  constructor() {
    this.syncFromUrl();
    this._router.events
      .pipe(filter(e => e instanceof NavigationEnd))
      .subscribe(() => {
        this.syncFromUrl();
        this._cdr.markForCheck();
      });
  }

  onContextChange(context: SearchContext): void {
    this.NavigateToActiveContext();
  }

  onSearch(): void {
    this.NavigateToActiveContext();
  }

  private contextFromUrl(url: string): SearchContext {
    if (url.startsWith('/relics')) return 'relics';
    if (url.startsWith('/mods')) return 'mods';
    return 'prime-parts';
  }

  private NavigateToActiveContext(): void {
    if (!this.searchTerm.trim()) return;
    const queryParams: Record<string, string> = {'itemName': this.searchTerm};
    this._router.navigate([`/${this.activeContext}`], { queryParams });
  }

  private syncFromUrl(): void {
    this.activeContext = this.contextFromUrl(this._router.url);
  }
}
