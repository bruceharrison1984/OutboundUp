import { interval, startWith, switchMap, retry, timer, Observable } from 'rxjs';
import { REFRESH_INTERVAL } from './app.module';

export const pollingWithRetry = <T>(
  refreshInterval: number,
  requestFunc: () => Observable<T>
) =>
  interval(refreshInterval).pipe(
    startWith(0),
    switchMap(() => requestFunc()),
    retry({
      count: Infinity,
      delay: (error, count) => {
        console.log(error);
        return timer(Math.min(60000, 2 ^ (count * REFRESH_INTERVAL)));
      },
    })
  );
