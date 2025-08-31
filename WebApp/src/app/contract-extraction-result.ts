export interface ContractExtractionResult {
  parties: string[];
  effectiveDate: string | null;
  expirationDate: string | null;
  governingLaw: string | null;
  contractValue: number | null;
  contractType: string | null;
  keyTerms: string[];
  risks: string[];
  recommendations: string[];
  extractedText: string;
  processingTime: string;
}
