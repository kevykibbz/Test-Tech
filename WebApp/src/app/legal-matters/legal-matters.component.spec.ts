import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LegalMattersComponent } from './legal-matters.component';
import { commonTestProviders } from '../../test-helpers';

describe('LegalMattersComponent', () => {
  let component: LegalMattersComponent;
  let fixture: ComponentFixture<LegalMattersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LegalMattersComponent],
      providers: [...commonTestProviders]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LegalMattersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
