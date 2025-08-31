export interface LegalMatterCreateUpdate {
  matterName: string;
  contractType: string;
  parties: string[];
  effectiveDate: string;
  expirationDate: string;
  governingLaw: string;
  contractValue: number;
  status: string;
  description: string;
  lawyerId: string | null;
}

export interface LegalMatterDetailed {
  id: string;
  matterName: string;
  contractType: string;
  parties: string[];
  effectiveDate: string;
  expirationDate: string;
  governingLaw: string;
  contractValue: number;
  status: string;
  description: string;
  lawyerId: string | null;
  createdAt: string;
  lastModified: string;
}
