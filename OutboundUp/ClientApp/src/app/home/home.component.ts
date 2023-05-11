import { Component, OnDestroy, OnInit } from '@angular/core';
import { FetchSpeedTestResultsService } from '../services/fetch-speed-test-results.service';
import {
  Subscription,
  interval,
  retry,
  startWith,
  switchMap,
  timer,
} from 'rxjs';
import { SpeedTestLine } from '../types/types';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnDestroy {
  bandwidthResults: SpeedTestLine[] = [];
  latencyResults: SpeedTestLine[] = [];
  speedTestResultsInterval: Subscription;

  constructor(public speedTestResultService: FetchSpeedTestResultsService) {
    this.speedTestResultsInterval = interval(1000)
      .pipe(
        startWith(0),
        switchMap(() => speedTestResultService.getSpeedTestChartData()),
        retry({
          count: Infinity,
          delay: (error, count) => timer(Math.min(60000, 2 ^ (count * 1000))),
        })
      )
      .subscribe((results) => {
        this.bandwidthResults = results.bandwidth;
        this.latencyResults = results.latency;
      });
  }

  ngOnDestroy(): void {
    this.speedTestResultsInterval.unsubscribe();
  }
}
