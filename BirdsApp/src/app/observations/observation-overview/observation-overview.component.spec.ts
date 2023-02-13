import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationOverviewComponent } from './observation-overview.component';

describe('ObservationOverviewComponent', () => {
  let component: ObservationOverviewComponent;
  let fixture: ComponentFixture<ObservationOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ObservationOverviewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ObservationOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
