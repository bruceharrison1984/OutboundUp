import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { WebHookListResponse } from '../types/types';

@Injectable({
  providedIn: 'root',
})
export class WebHooksService {
  constructor(
    protected http: HttpClient,
    @Inject('BASE_URL') protected baseUrl: string
  ) {}

  getWebHooks() {
    return this.http.get<WebHookListResponse>(this.baseUrl + 'WebHook');
  }
}
