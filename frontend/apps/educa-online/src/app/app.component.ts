import { Component, DEFAULT_CURRENCY_CODE, inject, LOCALE_ID, OnDestroy, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';

import ptBr from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ErrorHandler } from '@educa-online/services';
import { AlertComponent, AlertOptions } from '@educa-online/components';
import { Subject, takeUntil } from 'rxjs';

registerLocaleData(ptBr);

@Component({
  standalone: true,
  imports: [RouterModule, MatSnackBarModule],
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  providers: [
    { provide: LOCALE_ID, useValue: 'pt' },
    { provide: DEFAULT_CURRENCY_CODE, useValue: 'BRL' },
  ],
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'gestao-compra';

  handle = inject(ErrorHandler);
  private _snackBar = inject(MatSnackBar);
  destroyed$ = new Subject<void>();

  ngOnInit() {
    this.handle.erros$.pipe(takeUntil(this.destroyed$)).subscribe(erros => {
      this._snackBar.openFromComponent(AlertComponent, {
        duration: 5000,
        data: {
          title: 'Erro!',
          subtitle: erros.join("<br>"),
          status: 'erro'
        } as AlertOptions
      });
    });
  }

  ngOnDestroy(): void {
    this.destroyed$.next();
  }
}
