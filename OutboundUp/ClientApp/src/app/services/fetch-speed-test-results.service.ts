import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {
  RawSpeedTestResultsResponse,
  SpeedTestResultsResponse,
} from '../types/types';

@Injectable({
  providedIn: 'root',
})
export class FetchSpeedTestResultsService {
  constructor(
    protected http: HttpClient,
    @Inject('BASE_URL') protected baseUrl: string
  ) {}

  getSpeedTestChartData() {
    return this.http.get<SpeedTestResultsResponse>(
      this.baseUrl + 'speedtestresults'
    );
  }

  getRawSpeedTestData() {
    return this.http.get<RawSpeedTestResultsResponse>(
      this.baseUrl + 'speedtestresults/raw'
    );
  }
}
