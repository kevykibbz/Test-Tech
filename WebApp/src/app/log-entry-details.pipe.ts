import { Pipe, PipeTransform } from '@angular/core';
import { LOG_ENTRY_DETAILS_ENTITY } from './log-entry-details-entity';

@Pipe({
  name: 'logEntryDetails',
})
export class LogEntryDetailsPipe implements PipeTransform {

  transform(value: string): unknown {
    let processedValue = value;
    Object.keys(LOG_ENTRY_DETAILS_ENTITY)
      .forEach(entityType => {
        const entityDescriptor = (LOG_ENTRY_DETAILS_ENTITY as any)[entityType];

        processedValue = processedValue.replace(
          new RegExp(`\\[>${entityDescriptor.embedCodeId}:(.*?)\\]`, 'gi'),
          function replacer(fullMatch: string, serializedData: string): any {
            try {
              return entityDescriptor.textValueExtractor(JSON.parse(serializedData));
            } catch (e) {
              console.error('Error extracting data');
            }


            return fullMatch;
          });
      });

    return processedValue;
  }

}
