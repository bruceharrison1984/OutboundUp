export interface SpeedTestResultsResponse {
  lineChartData: SpeedTestLine[];
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
  responseTime: number;
  downloadSpeed: number;
  uploadSpeed: number;
}
