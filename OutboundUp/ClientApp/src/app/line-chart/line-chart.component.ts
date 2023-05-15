import { Component, Input } from '@angular/core';

@Component({
  selector: 'line-chart',
  templateUrl: './line-chart.component.html',
})
export class SpeedTestChartComponent {
  @Input() data: any[] = [];
  @Input() chartTitle: string = 'CHART TITLE';

  constructor() {
    Object.assign(this, { multi: this.data });
  }

  dateTickFormatting(val: string) {
    return new Date(val).toLocaleString();
  }
}
