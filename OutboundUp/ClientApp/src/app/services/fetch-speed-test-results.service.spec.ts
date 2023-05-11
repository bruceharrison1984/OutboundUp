import { TestBed } from '@angular/core/testing';

import { FetchSpeedTestResultsService } from './fetch-speed-test-results.service';

describe('FetchSpeedTestResultsService', () => {
  let service: FetchSpeedTestResultsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FetchSpeedTestResultsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
