export class LegalMatter {
  id?: string;
  matterName?: string;
  contractType?: string | null = null;
  parties?: string[] | null = null;
  effectiveDate?: Date | string | null = null;
  expirationDate?: Date | string | null = null;
  governingLaw?: string | null = null;
  contractValue?: number | null = null;
  status?: string | null = null;
  description?: string | null = null;
  createdAt?: Date | string;
  lastModified?: Date | string | null = null;
  lawyerId?: string | null = null;
  hasParties?: boolean = false;
  hasFinancialTerms?: boolean = false;
  isActive?: boolean = false;
  isExpired?: boolean = false;

  // Legacy properties for backward compatibility
  get name(): string | undefined {
    return this.matterName;
  }

  set name(value: string | undefined) {
    this.matterName = value;
  }

  get created_at(): Date | string | undefined {
    return this.createdAt;
  }

  set created_at(value: Date | string | undefined) {
    this.createdAt = value;
  }

  get updated_at(): Date | string | null | undefined {
    return this.lastModified;
  }

  set updated_at(value: Date | string | null | undefined) {
    this.lastModified = value;
  }

  constructor(params?: Partial<LegalMatter>) {
    Object.assign(this, params || {});

    if (typeof this.createdAt === 'string') {
      this.createdAt = new Date(this.createdAt);
    }

    if (typeof this.lastModified === 'string') {
      this.lastModified = new Date(this.lastModified);
    }

    if (typeof this.effectiveDate === 'string') {
      this.effectiveDate = new Date(this.effectiveDate);
    }

    if (typeof this.expirationDate === 'string') {
      this.expirationDate = new Date(this.expirationDate);
    }
  }
}
