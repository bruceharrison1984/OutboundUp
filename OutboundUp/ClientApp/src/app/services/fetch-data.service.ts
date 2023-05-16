import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import {
  RawSpeedTestResultsResponse,
  SpeedTestResultsResponse,
  StatisticsResponse,
} from '../types/types';

@Injectable({
  providedIn: 'root',
})
export class FetchDataService {
  constructor(
    protected http: HttpClient,
    @Inject('BASE_URL') protected baseUrl: string
  ) {}

  getSpeedTestChartData() {
    return this.http.get<SpeedTestResultsResponse>(
      this.baseUrl + 'TestResults/ChartData'
    );
  }

  getRawSpeedTestData(pageNumber: number, pageSize: number) {
    return this.http.get<RawSpeedTestResultsResponse>(
      this.baseUrl +
        `TestResults/Raw?pageNumber=${pageNumber}&pageSize=${pageSize}`
    );
  }

  getStatistics() {
    return this.http.get<StatisticsResponse>(
      this.baseUrl + 'TestResults/Statistics'
    );
  }
}
