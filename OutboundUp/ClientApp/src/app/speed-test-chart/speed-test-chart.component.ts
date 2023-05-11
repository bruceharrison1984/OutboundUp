import { Component, Input } from '@angular/core';

@Component({
  selector: 'speed-test-chart',
  templateUrl: './speed-test-chart.component.html',
  styleUrls: ['./speed-test-chart.component.css'],
})
export class SpeedTestChartComponent {
  @Input() data: any[] = [];

  // options
  legend: boolean = true;
  showLabels: boolean = true;
  animations: boolean = true;
  xAxis: boolean = true;
  yAxis: boolean = true;
  showYAxisLabel: boolean = false;
  showXAxisLabel: boolean = false;
  timeline: boolean = true;

  // colorScheme = {
  //   domain: ['#5AA454', '#E44D25', '#CFC0BB', '#7aa3e5', '#a8385d', '#aae3f5'],
  // };

  constructor() {
    Object.assign(this, { multi: this.data });
  }
}
