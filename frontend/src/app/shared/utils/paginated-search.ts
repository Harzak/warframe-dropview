import { DestroyRef, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Observable, Subscription } from 'rxjs';

export interface PaginatedSearchConfig<TRaw, TItem> {
  limit: number;
  fetchFn: (offset: number) => Observable<TRaw>;
  extractItems: (raw: TRaw) => TItem[];
  hasMore: (raw: TRaw, limit: number) => boolean;
  destroyRef: DestroyRef;
  onResult?: (raw: TRaw) => void;
}

export class PaginatedSearch<TRaw, TItem> {
  readonly items = signal<TItem[]>([]);
  readonly loading = signal(false);
  readonly hasMore = signal(false);
  readonly error = signal<string | null>(null);

  private _offset = 0;
  private _subscription: Subscription | null = null;

  constructor(private readonly _config: PaginatedSearchConfig<TRaw, TItem>) {}

  search(): void {
    this._cancelPending();
    this._offset = 0;
    this.items.set([]);
    this.hasMore.set(false);
    this.error.set(null);
    this.loading.set(true);
    this._fetch(0);
  }

  loadMore(): void {
    if (this.loading()) return;

    this._offset += this._config.limit;
    this.loading.set(true);
    this._fetch(this._offset);
  }

  reset(): void {
    this._cancelPending();
    this._offset = 0;
    this.items.set([]);
    this.hasMore.set(false);
    this.loading.set(false);
    this.error.set(null);
  }

  private _fetch(offset: number): void {
    this._cancelPending();
    this._subscription = this._config.fetchFn(offset).pipe(
      takeUntilDestroyed(this._config.destroyRef),
    ).subscribe({
      next: (raw) => {
        this.loading.set(false);
        this.items.update(current => [...current, ...this._config.extractItems(raw)]);
        this.hasMore.set(this._config.hasMore(raw, this._config.limit));
        this._config.onResult?.(raw);
      },
      error: (err: unknown) => {
        this.loading.set(false);
        this.error.set(err instanceof Error ? err.message : 'An unexpected error occurred');
      },
    });
  }

  private _cancelPending(): void {
    this._subscription?.unsubscribe();
    this._subscription = null;
  }
}
