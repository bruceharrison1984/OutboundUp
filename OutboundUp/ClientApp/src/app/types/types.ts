export interface SpeedTestResultsResponse
  extends ApiResponse<{
    bandwidth: SpeedTestLine[];
    latency: SpeedTestLine[];
  }> {}

export interface SpeedTestLine {
  name: string;
  series: NgxLineChartSeriesData[];
}

export interface NgxLineChartSeriesData {
  name: number;
  value: number;
  min: number;
  max: number;
}

export interface RawSpeedTestResultsResponse
  extends ApiResponse<RawSpeedTestResult[]> {}

export interface RawSpeedTestResult {
  id: number;
  timestamp: number;
  isSuccess: boolean;
  serverHostName: string;
  downloadSpeed: number;
  uploadSpeed: number;
  pingAverage: number;
  pingHigh: number;
  pingLow: number;
  downloadLatencyAverage: number;
  downloadLatencyHigh: number;
  downloadLatencyLow: number;
  uploadLatencyAverage: number;
  uploadLatencyHigh: number;
  uploadLatencyLow: number;
}

export interface HealthCheckResponse
  extends ApiResponse<{ isJobRunning: boolean }> {}

export interface StatisticsResponse extends ApiResponse<Statistics> {}

export interface Statistics {
  failedHealthChecks: number;
  successfulHealthChecks: number;
  averagePing: number;
  averageDownloadSpeed: number;
  averageUploadSpeed: number;
}

export type ApiResponse<T> = {
  data: T;
  errors: string[];
};
