import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BirdOverviewComponent } from './bird-overview.component';

describe('BirdsComponent', () => {
  let component: BirdOverviewComponent;
  let fixture: ComponentFixture<BirdOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BirdOverviewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BirdOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
