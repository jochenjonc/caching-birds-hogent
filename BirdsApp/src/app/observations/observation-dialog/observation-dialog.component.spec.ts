import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ObservationDialogComponent } from './observation-dialog.component';

describe('ObservationDialogComponent', () => {
  let component: ObservationDialogComponent;
  let fixture: ComponentFixture<ObservationDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ObservationDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ObservationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
