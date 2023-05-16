import { Component, OnDestroy } from '@angular/core';
import { FetchDataService } from '../services/fetch-data.service';
import { WebHooksService } from '../services/webhooks.service';
import { Subscription } from 'rxjs';
import { pollingWithRetry } from '../utils';
import { REFRESH_INTERVAL } from '../app.module';
import { OutboundWebHook } from '../types/types';

@Component({
  selector: 'webhooks',
  templateUrl: './webhooks.component.html',
})
export class WebHooksComponent implements OnDestroy {
  webhooks?: OutboundWebHook[];
  webhooksListInterval: Subscription;

  constructor(public webhookService: WebHooksService) {
    this.webhooksListInterval = pollingWithRetry(REFRESH_INTERVAL, () =>
      webhookService.getWebHooks()
    ).subscribe((results) => {
      this.webhooks = results.data;
    });
  }

  ngOnDestroy(): void {}
}
