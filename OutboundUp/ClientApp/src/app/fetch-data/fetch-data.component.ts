import { Component, OnDestroy } from '@angular/core';
import { Subscription, interval, startWith, switchMap } from 'rxjs';
import { FetchSpeedTestResultsService } from '../services/fetch-speed-test-results.service';
import { RawSpeedTestResult } from '../types/types';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
})
export class FetchDataComponent implements OnDestroy {
  speedTestResults: RawSpeedTestResult[] = [];
  speedTestResultsInterval: Subscription;

  constructor(public speedTestResultService: FetchSpeedTestResultsService) {
    this.speedTestResultsInterval = interval(1000)
      .pipe(
        startWith(0),
        switchMap(() => speedTestResultService.getRawSpeedTestData())
      )
      .subscribe((results) => {
        this.speedTestResults = results.data;
      });
  }

  ngOnDestroy(): void {
    this.speedTestResultsInterval.unsubscribe();
  }
}
