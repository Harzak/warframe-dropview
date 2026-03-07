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

  readonly partTypes = ['Blueprint', 'Neuroptics', 'Chassis', 'Systems', 'Barrel'];
  readonly relicTiers = ['Lith', 'Meso', 'Neo', 'Axi', 'Requiem'];
  readonly rarities = ['Common', 'Uncommon', 'Rare', 'Legendary'];

  activeContext: SearchContext = this.contextFromUrl(this.router.url);

  // Filter values — form state only, source of truth is the URL
  partType = '';
  relicTier = '';
  dropRarity = '';
  relicName = '';

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
    this.partType = '';
    this.relicTier = '';
    this.dropRarity = '';
    this.relicName = '';
    this.router.navigate([`/${context}`]);
  }

  onSearch(): void {
    const queryParams: Record<string, string> = {};

    if (this.activeContext === 'prime-parts') {
      if (this.partType) queryParams['partType'] = this.partType;
      if (this.relicTier) queryParams['relicTier'] = this.relicTier;
      if (this.dropRarity) queryParams['dropRarity'] = this.dropRarity;
    } else if (this.activeContext === 'relics') {
      if (this.relicName) queryParams['relicName'] = this.relicName;
    }
    // mods: no query params, always fetch all

    this.router.navigate([`/${this.activeContext}`], { queryParams });
  }

  private contextFromUrl(url: string): SearchContext {
    if (url.startsWith('/relics')) return 'relics';
    if (url.startsWith('/mods')) return 'mods';
    return 'prime-parts';
  }

  private syncFromUrl(): void {
    const params = this.router.parseUrl(this.router.url).queryParams;
    this.activeContext = this.contextFromUrl(this.router.url);
    this.partType = params['partType'] ?? '';
    this.relicTier = params['relicTier'] ?? '';
    this.dropRarity = params['dropRarity'] ?? '';
    this.relicName = params['relicName'] ?? '';
  }
}
