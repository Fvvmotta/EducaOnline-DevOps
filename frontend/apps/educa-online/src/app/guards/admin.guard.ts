import { Injectable } from "@angular/core";
import { CanActivate, Router } from "@angular/router";
import { AuthService } from "@educa-online/services";
import { Observable } from "rxjs";

@Injectable({ providedIn: 'root' })
export class AdmindGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  canActivate(): boolean | Observable<boolean> | Promise<boolean> {
    const url = this.authService.getUrl();

    if(!url) {
      this.authService.logout();
      this.router.navigate(['/login']);
      return false;
    }

    if (this.authService.isLoggedIn() && this.authService.getPerfil() == "ADM") {
      return true;
    }

    this.router.navigate(['/curso']);
    return false;
  }
}