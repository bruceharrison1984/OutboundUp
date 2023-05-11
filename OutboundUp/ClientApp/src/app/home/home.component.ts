import { Component, OnDestroy, OnInit } from '@angular/core';
import { FetchSpeedTestResultsService } from '../services/fetch-speed-test-results.service';
import { Subscription, interval, startWith, switchMap } from 'rxjs';
import { SpeedTestLine } from '../types/types';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnDestroy {
  speedTestResults: SpeedTestLine[] = [];
  speedTestResultsInterval: Subscription;

  constructor(public speedTestResultService: FetchSpeedTestResultsService) {
    this.speedTestResultsInterval = interval(1000)
      .pipe(
        startWith(0),
        switchMap(() => speedTestResultService.getSpeedTestResults())
      )
      .subscribe((results) => {
        this.speedTestResults = results.lineChartData;
      });
  }

  ngOnDestroy(): void {
    this.speedTestResultsInterval.unsubscribe();
  }
}
