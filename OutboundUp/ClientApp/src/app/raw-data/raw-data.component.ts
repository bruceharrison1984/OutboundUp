import { Component } from '@angular/core';
import { take } from 'rxjs';
import { FetchDataService } from '../services/fetch-data.service';
import { RawTableData } from '../types/types';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './raw-data.component.html',
})
export class RawDataComponent {
  speedTestResults?: RawTableData;
  pageSize = 10;
  pageNumber = 0;
  pages: number[] = [];

  constructor(public speedTestResultService: FetchDataService) {
    this.getPage();
  }

  onPageSizeChange(pageSize: string) {
    this.pageSize = Number.parseInt(pageSize);
    this.pageNumber = 0;

    this.getPage();
  }

  onPageNumberChange(pageNumber: string) {
    this.pageNumber = Number.parseInt(pageNumber);
    this.getPage();
  }

  getPage() {
    this.speedTestResultService
      .getRawSpeedTestData(this.pageNumber, this.pageSize)
      .pipe(take(1))
      .subscribe((results) => {
        results.data.tableData = results.data.tableData.map((x) => {
          x.unixTimestampMs = `${new Date(x.unixTimestampMs).toLocaleDateString(
            'en-us',
            { month: '2-digit', day: '2-digit', year: 'numeric' }
          )} ${new Date(x.unixTimestampMs).toLocaleTimeString('en-us', {
            hour: '2-digit',
            minute: '2-digit',
            second: '2-digit',
          })}`;
          return x;
        });

        this.speedTestResults = results.data;

        const totalPages = this.speedTestResults?.totalCount / this.pageSize;
        this.pages = Array.from({ length: totalPages }, (v, i) => i + 1).slice(
          0,
          totalPages
        );
      });
  }

  greaterThanPrevious(currentValue?: number, previousValue?: number) {
    if (!currentValue) return false;
    if (!previousValue) return true;
    return currentValue < previousValue;
  }

  getPreviousRow(index: number) {
    return this.speedTestResults?.tableData[index - 1];
  }
}
