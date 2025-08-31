import { LegalMatter } from './legal-matter';

export interface Lawyer {
  id: string;
  firstName: string;
  lastName: string;
  companyName: string;
  createdAt: string;
  lastModified: string | null;
  fullName: string;
  isValid: boolean;
  assignedMatters?: LegalMatter[];
}
