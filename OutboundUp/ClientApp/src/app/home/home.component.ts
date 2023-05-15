import { Component, OnDestroy, OnInit } from '@angular/core';
import { FetchDataService } from '../services/fetch-data.service';
import {
  Subscription,
  interval,
  retry,
  startWith,
  switchMap,
  timer,
} from 'rxjs';
import { SpeedTestLine } from '../types/types';
import { REFRESH_INTERVAL } from '../app.module';
import { pollingWithRetry } from '../utils';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnDestroy {
  bandwidthResults: SpeedTestLine[] = [];
  latencyResults: SpeedTestLine[] = [];
  speedTestResultsInterval: Subscription;

  constructor(public speedTestResultService: FetchDataService) {
    this.speedTestResultsInterval = pollingWithRetry(REFRESH_INTERVAL, () =>
      speedTestResultService.getSpeedTestChartData()
    ).subscribe((results) => {
      this.bandwidthResults = results.data.bandwidth;
      this.latencyResults = results.data.latency;
    });
  }

  ngOnDestroy(): void {
    this.speedTestResultsInterval.unsubscribe();
  }
}
