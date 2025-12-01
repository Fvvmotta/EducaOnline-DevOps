import { AuthenticatedComponent } from './../../authenticated.component';
import { Component, inject, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { filter, switchMap, take } from "rxjs";
import { AuthService, ConteudoService, HeaderService } from "@educa-online/services";
import { AulaResponseModel } from "@educa-online/data";
import { ActivatedRoute } from "@angular/router";
import { CreateEditAulaComponent } from "../create-edit-aula/create-edit-aula.component";
import { AlertComponent, AlertOptions, ModalInfoComponent, ModalInfoModel } from "@educa-online/components";

@Component({
  selector: 'app-lista-aula',
  templateUrl: 'lista-aula.component.html',
  styleUrls: ['lista-aula.component.scss'],
  standalone: false
})
export class ListaAulaComponent implements OnInit {
  readonly dialog = inject(MatDialog);
  private _snackBar = inject(MatSnackBar);

  displayedColumns: string[] = ['titulo', 'descricao', 'totalHoras', 'acoes'];

  aulas: AulaResponseModel[] = [];

  usuarioAdmin = false;

  cursoId!: string;

  constructor(
    private headerService: HeaderService,
    private conteudoService: ConteudoService,
    private route: ActivatedRoute,
    private authService: AuthService
  ) {
    this.route.params.subscribe(params => {
		  this.cursoId = params['id'];
	  });

    this.usuarioAdmin = this.authService.getPerfil() == "ADM";
  }

  ngOnInit() {
    this.conteudoService.getById(this.cursoId)
    .pipe(take(1))
    .subscribe({
      next: (data) => {
        this.aulas = data.aulas;
        this.headerService.alterarTitulo('Aulas do curso ' + data.nome);
      }
    });
  }

  getCurso(){
    this.conteudoService.getById(this.cursoId)
    .pipe(take(1))
    .subscribe({
      next: (data) => {
        this.aulas = data.aulas;
      }
    });
  }

  novaAula(){
    const ref = this.dialog.open(CreateEditAulaComponent, {
      width: '40rem',
    });

    ref.afterClosed()
      .pipe(
        take(1),
        filter(data => data))
      .subscribe(_ => {
        this._snackBar.openFromComponent(AlertComponent, {
          duration: 5000,
          data: {
            title: 'Sucesso!',
            subtitle: 'Aula criada',
            status: 'sucesso'
          } as AlertOptions
        });

        this.getCurso();
      });
  }

  editarAula(aula: AulaResponseModel){
    const ref = this.dialog.open(CreateEditAulaComponent, {
      width: '40rem',
      data: aula
    });

    ref.afterClosed()
      .pipe(
        take(1),
        filter(data => data))
      .subscribe(_ => {
        this._snackBar.openFromComponent(AlertComponent, {
          duration: 5000,
          data: {
            title: 'Sucesso!',
            subtitle: 'Aula alterada',
            status: 'sucesso'
          } as AlertOptions
        });

        this.getCurso();
      });
  }

  removerAula(id: string){
    const ref = this.dialog.open(ModalInfoComponent, {
      width: '50rem',
      data: {
        titulo: 'Remover aula',
        texto: 'Esta ação não pode ser desfeita, deseja confirma-la?',
        btnOk: 'Confirmar',
        btnCancel: 'Voltar'
      } as ModalInfoModel
    });

    ref.afterClosed()
      .pipe(
        take(1),
        filter(data => data),
        switchMap(data => this.conteudoService.removerAula(this.cursoId, id)))
      .subscribe(_ => {
        this._snackBar.openFromComponent(AlertComponent, {
          duration: 5000,
          data: {
            title: 'Sucesso!',
            subtitle: 'Aula removida',
            status: 'sucesso'
          } as AlertOptions
        });

        this.getCurso();
      });
  }
}
