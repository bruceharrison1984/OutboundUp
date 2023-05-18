import { Component, OnDestroy } from '@angular/core';
import { WebHooksService } from '../services/webhooks.service';
import { Subscription, take } from 'rxjs';
import { pollingWithRetry } from '../utils';
import { REFRESH_INTERVAL } from '../app.module';
import { OutboundWebHook } from '../types/types';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'webhooks',
  templateUrl: './webhooks.component.html',
})
export class WebHooksComponent implements OnDestroy {
  webhooks?: OutboundWebHook[];
  webhooksListInterval: Subscription;
  createModalClasses = ['modal'];

  formGroup: FormGroup = new FormGroup({
    targetUrl: new FormControl('', WebHooksComponent.validateUrl),
  });

  constructor(public webhookService: WebHooksService) {
    this.webhooksListInterval = pollingWithRetry(REFRESH_INTERVAL, () =>
      webhookService.getWebHooks()
    ).subscribe((results) => {
      this.webhooks = results.data;
    });
  }

  ngOnDestroy(): void {}

  createWebHook() {
    this.webhookService
      .createWebHook(this.formGroup.value.targetUrl)
      .pipe(take(1))
      .subscribe((x) => {
        this.closeCreateModal();
      });
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

  static validateUrl: ValidatorFn = (
    control: AbstractControl
  ): ValidationErrors | null => {
    try {
      let str = control.value;
      new URL(str);
      return null;
    } catch (_) {
      return { invalidUrl: true };
    }
  };

  deleteWebHook(id: number) {
    console.log('test');
    this.webhookService.deleteWebHook(id).pipe(take(1)).subscribe();
  }
}
