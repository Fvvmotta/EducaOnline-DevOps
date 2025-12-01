import { Injectable } from '@angular/core';
import {
  CanActivate,
} from '@angular/router';
import { Router } from '@angular/router';
import { AuthService } from '@educa-online/services';
import { Observable } from 'rxjs';

@Injectable({providedIn: 'root'})
export class LoginGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  canActivate(): boolean | Observable<boolean> | Promise<boolean> {
    const url = this.authService.getUrl();

    if (!url && this.authService.isLoggedIn()) {
      this.authService.logout();
      this.router.navigate(['/login']);
      return false;
    }

    if (this.authService.isLoggedIn()) {
      this.router.navigate([url]);
      return false;
    }

    return true;
  }
}
