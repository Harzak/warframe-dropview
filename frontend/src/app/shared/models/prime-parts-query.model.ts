import { PaginatedQuery } from './paginated-query.model';

export interface PrimePartsQuery extends PaginatedQuery {
  itemName?: string;
  relicTiers?: string;
  dropRarities?: string;
  refinements?: string;
}