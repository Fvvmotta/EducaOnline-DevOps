import { Component, inject, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AlertComponent, AlertOptions } from '@educa-online/components';
import { NovaContaFormGroup } from '@educa-online/forms';
import { AuthService } from '@educa-online/services';
import { take } from 'rxjs';

@Component({
  selector: 'app-form-nova-conta',
  templateUrl: 'form-nova-conta.component.html',
  styleUrls: ['form-nova-conta.component.scss'],
  standalone: false
})

export class FormaNovaContaComponent implements OnInit {
  private _snackBar = inject(MatSnackBar);
  
  form: NovaContaFormGroup;

  constructor(
    private router: Router,
    private authService: AuthService
  ) {
    this.form = new NovaContaFormGroup();
  }

  ngOnInit() {
    return;
  }

  salvarConta(){
    if(this.form.valid){
      this.authService.novaConta(this.form.value)
      .pipe(take(1))
      .subscribe({
        next: () => {
          this._snackBar.openFromComponent(AlertComponent, {
            duration: 5000,
            data: {
              title: 'Sucesso!',
              subtitle: 'conta criada!',
              status: 'sucesso'
            } as AlertOptions
          });
    
          this.voltar();
        },
        error: (err) => {
          this._snackBar.openFromComponent(AlertComponent, {
            duration: 5000,
            data: {
              title: 'Erro!',
              subtitle: 'E-mail ou senha inávlidos',
              status: 'erro'
            } as AlertOptions
          });
        }
      });
    } 
    else {
      this._snackBar.openFromComponent(AlertComponent, {
        duration: 5000,
        data: {
          title: 'Erro!',
          subtitle: "Campos inválidos!",
          status: 'erro'
        } as AlertOptions
      });
    }
  }

  voltar(): void {
    this.router.navigate(['/login']);
  }
}
