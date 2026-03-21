import { PaginatedQuery } from './paginated-query.model';

export interface ModsQuery extends PaginatedQuery {
  itemName?: string;
  dropRarities?: string;
  itemTypes?: string;
  missionTypes?: string;
}
