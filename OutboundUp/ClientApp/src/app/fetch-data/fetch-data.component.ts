import { Component, OnDestroy } from '@angular/core';
import {
  Subscription,
  interval,
  retry,
  startWith,
  switchMap,
  timer,
} from 'rxjs';
import { FetchDataService } from '../services/fetch-data.service';
import { RawSpeedTestResult } from '../types/types';
import { REFRESH_INTERVAL } from '../app.module';
import { pollingWithRetry } from '../utils';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
})
export class FetchDataComponent implements OnDestroy {
  speedTestResults: RawSpeedTestResult[] = [];
  speedTestResultsInterval: Subscription;

  constructor(public speedTestResultService: FetchDataService) {
    this.speedTestResultsInterval = pollingWithRetry(REFRESH_INTERVAL, () =>
      speedTestResultService.getRawSpeedTestData()
    ).subscribe((results) => {
      this.speedTestResults = results.data; //TODO fix this data.data
    });
  }

  ngOnDestroy(): void {
    this.speedTestResultsInterval.unsubscribe();
  }
}
