import { PaginatedQuery } from './paginated-query.model';

export interface RelicsQuery extends PaginatedQuery {
  itemName?: string;
  relicTiers?: string;
  dropRarities?: string;
  missionTypes?: string;
}