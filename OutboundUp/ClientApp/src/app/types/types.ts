export interface SpeedTestResultsResponse {
  bandwidth: SpeedTestLine[];
  latency: SpeedTestLine[];
}

export interface SpeedTestLine {
  name: string;
  series: NgxLineChartSeriesData[];
}

export interface NgxLineChartSeriesData {
  name: string;
  value: number;
  min: number;
  max: number;
}

export interface RawSpeedTestResultsResponse {
  data: RawSpeedTestResult[];
}

export interface RawSpeedTestResult {
  id: number;
  timestamp: string;
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
