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
  extends ApiResponse<RawTableData> {}

export interface RawTableData {
  totalCount: number;
  tableData: RawSpeedTestResult[];
  currentPage: number;
}

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

export interface WebHookListResponse extends ApiResponse<OutboundWebHook[]> {}

export interface OutboundWebHook {
  id: number;
  targetUrl: string;
  httpMethod: string;
  results: OutboundWebHookResult[];
}

export interface OutboundWebHookResult {
  id: number;
  responseCode: number;
  responseBody: string;
  isSuccess: boolean;
}

export type ApiResponse<T> = {
  data: T;
  errors: string[];
};
