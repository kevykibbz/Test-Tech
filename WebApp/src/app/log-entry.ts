export class LogEntry {
  id?: number;
  details?: string;
  // tslint:disable-next-line:variable-name
  entity_id?: string;
  // tslint:disable-next-line:variable-name
  type_id?: string;
  // tslint:disable-next-line:variable-name
  created_at?: Date | string;

  constructor(params?: Partial<LogEntry>) {
    Object.assign(this, params || {});

    if (typeof this.created_at === 'string') {
      this.created_at = new Date(this.created_at);
    }
  }
}
