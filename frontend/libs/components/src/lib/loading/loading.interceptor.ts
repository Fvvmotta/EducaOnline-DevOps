import { HttpEvent, HttpInterceptorFn, HttpResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, finalize } from 'rxjs';
import { LoadingComponent } from './loading.component';
import { LoadingService } from './loading.service';

export const LoadingInterceptor: HttpInterceptorFn = (req, next) => {
  const loading = inject(LoadingService);
  const ref = loading.open(LoadingComponent);
  loading.aberto();
  return new Observable(observer => {
    const subscription = next(req)
      .pipe(finalize(() => {
        ref.close();
        loading.fechado();
      }))
      .subscribe({
        next: (event: HttpEvent<any> | undefined) => {
          if (event instanceof HttpResponse) {
            ref.close();
            loading.fechado();
            observer.next(event);
          }
        },
        error: (err: any) => {
          ref.close();
          loading.fechado();
          observer.error(err);
        }
      });
    return () => {
      subscription.unsubscribe();
      loading.fechado();
    };
  });
};
