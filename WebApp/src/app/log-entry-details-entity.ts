import { LogEntryDetailsEntityType } from './log-entry-details-entity-type';

export const LOG_ENTRY_DETAILS_ENTITY = {
  [LogEntryDetailsEntityType.LegalMatter]: {
    embedCodeId: 'legal_matter',
    textValueExtractor: (data: any) => data.name,
  },
  [LogEntryDetailsEntityType.LegalMatterCategory]: {
    embedCodeId: 'legal_matter_category',
    textValueExtractor: (data: any) => data.name,
  },
  [LogEntryDetailsEntityType.Person]: {
    embedCodeId: 'person',
    textValueExtractor: (data: any) => data.full_name,
  },
};
