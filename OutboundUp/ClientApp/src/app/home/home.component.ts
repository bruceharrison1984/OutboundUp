import { Component, OnDestroy } from '@angular/core';
import { FetchDataService } from '../services/fetch-data.service';
import { Subscription } from 'rxjs';
import { SpeedTestLine, Statistics } from '../types/types';
import { REFRESH_INTERVAL } from '../app.module';
import { pollingWithRetry } from '../utils';
import { LegendPosition } from '@swimlane/ngx-charts';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnDestroy {
  bandwidthResults: SpeedTestLine[] = [];
  latencyResults: SpeedTestLine[] = [];
  statistics?: Statistics;
  successRate?: number;
  speedTestResultsInterval: Subscription;
  statisticsInterval: Subscription;
  legendPosition = LegendPosition.Below;
  colorScheme = { domain: ['#5AA454', '#5AA454'] };

  constructor(public speedTestResultService: FetchDataService) {
    this.speedTestResultsInterval = pollingWithRetry(REFRESH_INTERVAL, () =>
      speedTestResultService.getSpeedTestChartData()
    ).subscribe((results) => {
      this.bandwidthResults = results.data.bandwidth;
      this.latencyResults = results.data.latency;
    });

    this.statisticsInterval = pollingWithRetry(REFRESH_INTERVAL, () =>
      speedTestResultService.getStatistics()
    ).subscribe((results) => {
      this.statistics = results.data;
      this.successRate = Math.round(
        (results.data.successfulHealthChecks /
          results.data.failedHealthChecks) *
          10
      );
    });
  }

  ngOnDestroy(): void {
    this.speedTestResultsInterval.unsubscribe();
  }
}
