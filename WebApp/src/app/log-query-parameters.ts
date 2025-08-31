export interface LogQueryParameters {
  after?: number;
  after_ts?: string;
  before?: number;
  before_ts?: string;
  entity_id?: string;
  event_type?: string;
  take?: number;
}
