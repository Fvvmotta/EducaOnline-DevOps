import { Component, inject, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { CreateEditCursoComponent } from "../create-edit-curso/create-edit-curso.component";
import { filter, switchMap, take } from "rxjs";
import { AlertComponent, AlertOptions, ModalInfoComponent, ModalInfoModel } from "@educa-online/components";
import { AlunoService, AuthService, ConteudoService } from "@educa-online/services";
import { CursoResponseModel, MatriculaResponseModel } from "@educa-online/data";
import { Router } from "@angular/router";

@Component({
  selector: 'app-lista-curso',
  templateUrl: 'lista-curso.component.html',
  styleUrls: ['lista-curso.component.scss'],
  standalone: false
})
export class ListaCursoComponent implements OnInit {
  readonly dialog = inject(MatDialog);
  private _snackBar = inject(MatSnackBar);

  displayedColumns: string[] = ['nome', 'titulo', 'cargaHoraria', 'ativo', 'acoes'];
  displayedColumnsMatricula: string[] = ['cursoNome', 'totalAulas', 'cargaHorariaTotal', 'status', 'aulasConcluidas', 'acoes'];

  cursos: CursoResponseModel[] = [];
  matriculas: MatriculaResponseModel[] = [];

  usuarioAdmin = false;

  constructor(
    private conteudoService: ConteudoService,
    private router: Router,
    private authService: AuthService,
    private alunoService: AlunoService
  ) {
    this.usuarioAdmin = this.authService.getPerfil() == "ADM";
  }

  ngOnInit() {
    if(this.usuarioAdmin)
      this.getCursos();
    else
      this.getMatriculasAluno()
  }

  getCursos(){
    this.conteudoService.get()
    .pipe(take(1))
    .subscribe({
      next: (data) => {
        this.cursos = data;
      }
    });
  }

  getMatriculasAluno(){
    this.alunoService.getbyId(this.authService.getId())
    .pipe(take(1))
    .subscribe({
      next: (data) => {
        this.matriculas = data.matriculas;
      }
    });
  }

  novoCurso(): void {
    const ref = this.dialog.open(CreateEditCursoComponent, {
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
            subtitle: 'Curso criado',
            status: 'sucesso'
          } as AlertOptions
        });

        this.getCursos();
      });
  }

  abrirControleAulas(id: string){
    this.router.navigate([`curso/${id}/aula`]);
  }

  editarCurso(id: string): void {
    const ref = this.dialog.open(CreateEditCursoComponent, {
      width: '40rem',
      data: id
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
            subtitle: 'Curso alterado',
            status: 'sucesso'
          } as AlertOptions
        });

        this.getCursos();
      });
  }

  desativarCurso(id: string){
    const ref = this.dialog.open(ModalInfoComponent, {
      width: '50rem',
      data: {
        titulo: 'Desativar Curso',
        texto: 'Esta ação não pode ser desfeita, deseja confirma-la?',
        btnOk: 'Confirmar',
        btnCancel: 'Voltar'
      } as ModalInfoModel
    });

    ref.afterClosed()
      .pipe(
        take(1),
        filter(data => data),
        switchMap(data => this.conteudoService.desativarCurso(id)))
      .subscribe(_ => {
        this._snackBar.openFromComponent(AlertComponent, {
          duration: 5000,
          data: {
            title: 'Sucesso!',
            subtitle: 'Curso desativado',
            status: 'sucesso'
          } as AlertOptions
        });

        this.getCursos();
      });
  }
}
