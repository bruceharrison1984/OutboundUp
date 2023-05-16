import { Component } from '@angular/core';
import { take } from 'rxjs';
import { FetchDataService } from '../services/fetch-data.service';
import { RawTableData } from '../types/types';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
})
export class FetchDataComponent {
  speedTestResults?: RawTableData;
  pageSize = 10;
  pageNumber = 0;
  pages: number[] = [];

  constructor(public speedTestResultService: FetchDataService) {
    this.getPage();
    // const totalPages = this.speedTestResults?.totalCount / this.pageSize;
    // this.pages = Array.from({ length: totalPages }, (v, i) => i);
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
        this.speedTestResults = results.data;
        const totalPages = this.speedTestResults?.totalCount / this.pageSize;
        this.pages = Array.from({ length: totalPages }, (v, i) => i + 1).slice(
          0,
          totalPages
        );
      });
  }
}
