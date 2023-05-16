import { Component, OnDestroy } from '@angular/core';
import { WebHooksService } from '../services/webhooks.service';
import { Subscription } from 'rxjs';
import { pollingWithRetry } from '../utils';
import { REFRESH_INTERVAL } from '../app.module';
import { OutboundWebHook } from '../types/types';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'webhooks',
  templateUrl: './webhooks.component.html',
})
export class WebHooksComponent implements OnDestroy {
  webhooks?: OutboundWebHook[];
  webhooksListInterval: Subscription;
  createModalClasses = ['modal'];

  formGroup: FormGroup = new FormGroup({ targetUrl: new FormControl('') });

  constructor(public webhookService: WebHooksService) {
    this.webhooksListInterval = pollingWithRetry(REFRESH_INTERVAL, () =>
      webhookService.getWebHooks()
    ).subscribe((results) => {
      this.webhooks = results.data;
    });
  }

  ngOnDestroy(): void {}

  createWebHook() {
    console.log(this.formGroup.invalid);
  }

  showCreateModal() {
    this.createModalClasses.push('modal-open');
    console.log(this.createModalClasses);
  }

  closeCreateModal() {
    this.createModalClasses = this.createModalClasses.filter(
      (x) => x !== 'modal-open'
    );
  }
}
