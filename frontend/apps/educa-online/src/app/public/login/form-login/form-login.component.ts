import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertComponent, AlertOptions } from '@educa-online/components';
import { AuthService } from '@educa-online/services';
import { take } from 'rxjs';

@Component({
  selector: 'app-form-login',
  templateUrl: 'form-login.component.html',
  styleUrls: ['form-login.component.scss'],
  standalone: false
})

export class FormLoginComponent implements OnInit {

  private _snackBar = inject(MatSnackBar);

  form: FormGroup;
  cursoId: any;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private authService: AuthService,
    private route: ActivatedRoute
  ) {
    this.form = fb.group({
      email: ['', [Validators.required, Validators.email]],
      senha: ['', [Validators.required]]
    });

    this.route.queryParams.subscribe(params => {
      this.cursoId = params['cursoId'];
    });
  }

  ngOnInit() { }

  login(): void {
    const { value, valid } = this.form;

    if (valid) {
      this.authService.login(value)
        .pipe(take(1))
        .subscribe({
          next: (token) => {
            if (token) {
              console.log(token);
              this.authService.setToken(JSON.stringify(token));
              const role = token.userToken.claims.find((p: any) => p.type == "role");
              this.authService.setPerfil(role?.value!);
              if (!this.cursoId) {
                this.authService.setUrl('curso');
                this.router.navigate(['/curso']);
              }else {
                this.authService.setUrl('matricula');
                this.router.navigate(['/matricula', this.cursoId]);
              }
            }
          },
          error: (err) => {
            this._snackBar.openFromComponent(AlertComponent, {
              duration: 5000,
              data: {
                title: 'Erro!',
                subtitle: 'Usuário ou senha inválidos',
                status: 'erro'
              } as AlertOptions
            });
          }
        });
    }
  }

  novaConta(): void {
    this.router.navigate(['/nova-conta']);
  }
}
