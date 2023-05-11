import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SpeedTestChartComponent } from './speed-test-chart.component';

describe('SpeedTestChartComponent', () => {
  let component: SpeedTestChartComponent;
  let fixture: ComponentFixture<SpeedTestChartComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SpeedTestChartComponent]
    });
    fixture = TestBed.createComponent(SpeedTestChartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
