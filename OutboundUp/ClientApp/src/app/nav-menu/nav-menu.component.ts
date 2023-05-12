import { Component } from '@angular/core';
import { FetchHealthCheckService } from '../services/fetch-health-check.service';
import {
  Subscription,
  interval,
  retry,
  startWith,
  switchMap,
  timer,
} from 'rxjs';
import { HealthCheckResult } from '../types/types';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
})
export class NavMenuComponent {
  healthCheckResult: HealthCheckResult | undefined;
  statusMessage?: string;
  healthCheckResultsInterval: Subscription;

  constructor(fetchHealthCheckService: FetchHealthCheckService) {
    this.healthCheckResultsInterval = interval(1000)
      .pipe(
        startWith(0),
        switchMap(() => fetchHealthCheckService.getHealthCheck()),
        retry({
          count: Infinity,
          delay: (error, count) => {
            this.statusMessage = 'Could not contact API server';
            return timer(Math.min(60000, 2 ^ (count * 1000)));
          },
        })
      )
      .subscribe((results) => {
        this.healthCheckResult = results;

        if (results.isJobRunning) {
          this.statusMessage =
            'API connection is healthy\nSpeed test currently running';
        } else {
          this.statusMessage = 'API connection is healthy\nJob runner is idle';
        }
      });
  }
}
