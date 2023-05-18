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

  createWebHook(targetUrl: string) {
    return this.http.post(this.baseUrl + 'WebHook', {
      TargetUrl: targetUrl,
    });
  }

  deleteWebHook(id: number) {
    return this.http.delete(this.baseUrl + `WebHook?id=${id}`);
  }
}
