import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LegalMattersComponent } from './legal-matters/legal-matters.component';
import { LogExplorerComponent } from './log-explorer/log-explorer.component';
import { CurrenciesComponent } from './currencies/currencies.component';
import { EventTypesComponent } from './event-types/event-types.component';
import { PeopleComponent } from './people/people.component';
import { LegalMatterCategoriesComponent } from './legal-matter-categories/legal-matter-categories.component';
import { LawyersComponent } from './lawyers/lawyers.component';
import { ContractExtractionComponent } from './contract-extraction/contract-extraction.component';

export const routes: Routes = [
  {
    path: 'legal-matters',
    component: LegalMattersComponent,
  },
  {
    path: 'log',
    component: LogExplorerComponent,
  },
  {
    path: 'lawyers',
    component: LawyersComponent,
  },
  {
    path: 'contract-extraction',
    component: ContractExtractionComponent,
  },
  {
    path: 'currencies',
    component: CurrenciesComponent,
  },
  {
    path: 'event-types',
    component: EventTypesComponent,
  },
  {
    path: 'people',
    component: PeopleComponent,
  },
  {
    path: 'categories',
    component: LegalMatterCategoriesComponent,
  },
  {
    path: 'lawyers',
    component: LawyersComponent,
  },
  {
    path: 'contract-extraction',
    component: ContractExtractionComponent,
  },
  {
    path: 'requirements',
    component: HomeComponent,
  },
  {
    path: '**',
    redirectTo: 'requirements',
  },
];
