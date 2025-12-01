import { Component, inject, OnInit } from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { AlunoService } from "@educa-online/services";
import { take } from "rxjs";

@Component({
  selector: 'app-lista-usuario',
  templateUrl: 'lista-usuario.component.html',
  styleUrls: ['lista-usuario.component.scss'],
  standalone: false
})
export class ListaUsuarioComponent implements OnInit {
  readonly dialog = inject(MatDialog);
  private _snackBar = inject(MatSnackBar);

  displayedColumns: string[] = ['nome', 'email', 'dataCadastro'];

  usuarios: any[] = [];

  constructor(
    private alunoService: AlunoService
  ) {
  }

  ngOnInit() {
    this.alunoService.getAll()
    .pipe(take(1))
    .subscribe({
      next: (data) => {
        this.usuarios = data;
      }
    });
  }
}
