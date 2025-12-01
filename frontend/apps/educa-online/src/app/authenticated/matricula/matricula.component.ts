import { Component, inject, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertComponent, AlertOptions } from '@educa-online/components';
import { CursoResponseModel } from '@educa-online/data';
import { CartaoFormGroup, MatriculaFormGroup } from '@educa-online/forms';
import { AlunoBffService, ConteudoService, PedidoBffService } from '@educa-online/services';
import { of, switchMap, take } from 'rxjs';
import * as moment from 'moment';
import { MatDatepicker } from '@angular/material/datepicker';


@Component({
    selector: 'app-matricula',
    templateUrl: 'matricula.component.html',
    styleUrls: ['./matricula.component.scss'],
    standalone: false
})

export class MatriculaComponent implements OnInit {

    form: MatriculaFormGroup;
    formCartao: CartaoFormGroup;
    curso?: CursoResponseModel | null;
    private _snackBar = inject(MatSnackBar);

    constructor(
        private alunoBffService: AlunoBffService,
        private pedidoBffService: PedidoBffService,
        private conteudoService: ConteudoService,
        private route: ActivatedRoute,
        private router: Router
    ) {
        this.form = new MatriculaFormGroup();
        this.formCartao = new CartaoFormGroup();

        this.route.paramMap
            .pipe(
                take(1),
                switchMap(params => {
                    const cursoId = params.get('cursoId');
                    if (cursoId)
                        return this.conteudoService.getById(cursoId);
                    return of(null)
                }))
            .subscribe((curso) => {
                this.curso = curso
            });
    }

    ngOnInit() {

    }

    realizarPagamento(): void {
        const { valid, value } = this.formCartao;

        if (valid) {
            this.alunoBffService.matricular({ cursoId: this.curso?.id })
                .pipe(
                    take(1),
                    switchMap(data => {
                        if (data.errors) {
                            this._snackBar.openFromComponent(AlertComponent, {
                                duration: 5000,
                                data: {
                                    title: 'Erro!',
                                    subtitle: 'Usuário ou senha inválidos',
                                    status: 'erro'
                                } as AlertOptions
                            });

                            return of(false)
                        }

                        else {
                            return this.pedidoBffService.realizarPedido(value)
                        }
                    }))
                .subscribe({
                    next: (data) => {
                        this._snackBar.openFromComponent(AlertComponent, {
                            duration: 5000,
                            data: {
                                title: 'Sucesso!',
                                subtitle: 'Pagamento realizado!',
                                status: 'sucesso'
                            } as AlertOptions
                        });

                        this.router.navigate(['/inicio']);
                    },
                    error: (err) => {

                    }
                });
        }
    }
}