import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { HealthCheckResult } from '../types/types';

@Injectable({
  providedIn: 'root',
})
export class FetchHealthCheckService {
  constructor(
    protected http: HttpClient,
    @Inject('BASE_URL') protected baseUrl: string
  ) {}

  getHealthCheck() {
    return this.http.get<HealthCheckResult>(this.baseUrl + 'healthCheck');
  }
}
