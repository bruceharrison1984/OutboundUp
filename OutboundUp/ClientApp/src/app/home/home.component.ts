import { Component, OnDestroy } from '@angular/core';
import { FetchDataService } from '../services/fetch-data.service';
import { Subscription } from 'rxjs';
import { SpeedTestLine, Statistics, StatisticsResponse } from '../types/types';
import { REFRESH_INTERVAL } from '../app.module';
import { pollingWithRetry } from '../utils';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnDestroy {
  bandwidthResults: SpeedTestLine[] = [];
  latencyResults: SpeedTestLine[] = [];
  statistics?: Statistics;
  speedTestResultsInterval: Subscription;
  statisticsInterval: Subscription;

  constructor(public speedTestResultService: FetchDataService) {
    this.speedTestResultsInterval = pollingWithRetry(REFRESH_INTERVAL, () =>
      speedTestResultService.getSpeedTestChartData()
    ).subscribe((results) => {
      console.log('test');
      this.bandwidthResults = results.data.bandwidth;
      this.latencyResults = results.data.latency;
    });

    this.statisticsInterval = pollingWithRetry(REFRESH_INTERVAL, () =>
      speedTestResultService.getStatistics()
    ).subscribe((results) => {
      this.statistics = results.data;
    });
  }

  ngOnDestroy(): void {
    this.speedTestResultsInterval.unsubscribe();
  }
}
