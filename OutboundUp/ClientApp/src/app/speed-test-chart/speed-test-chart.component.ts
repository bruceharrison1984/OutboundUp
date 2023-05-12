import { Component, Input } from '@angular/core';

@Component({
  selector: 'speed-test-chart',
  templateUrl: './speed-test-chart.component.html',
})
export class SpeedTestChartComponent {
  @Input() data: any[] = [];

  constructor() {
    Object.assign(this, { multi: this.data });
  }

  dateTickFormatting(val: string) {
    return new Date(val).toLocaleString();
  }
}
