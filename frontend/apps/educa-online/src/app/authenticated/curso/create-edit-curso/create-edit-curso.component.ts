import { Component, inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { CursoFormGroup } from "@educa-online/forms";
import { ConteudoService } from "@educa-online/services";
import { take } from "rxjs";

@Component({
  selector: 'app-create-edit-curso',
  templateUrl: './create-edit-curso.component.html',
  styleUrls: ['create-edit-curso.component.scss'],
  standalone: false
})

export class CreateEditCursoComponent implements OnInit {
  form: CursoFormGroup;

  dialogRef = inject(MatDialogRef<CreateEditCursoComponent>)
  data = inject(MAT_DIALOG_DATA) as string;

  constructor(
    private conteudoService: ConteudoService
  ) {
    this.form = new CursoFormGroup();
  }

  ngOnInit() {
    if(this.data) {
      this.conteudoService.getById(this.data)
      .pipe(take(1))
      .subscribe({
        next: (response) => {
          if(!!response)
            this.form.patchValue(response);
        }
      });
    }
  }

  salvar(): void {
    const { valid, value } = this.form;
    if(valid && this.data) {
      this.editar();
    } else if(valid) {
      this.cadastrar();
    }
  }

  editar(){
    this.conteudoService.put(this.data, this.form.value)
    .pipe(take(1))
    .subscribe({
      next: (response) => {
        this.dialogRef.close(true);
      }
    });
  }

  cadastrar(){
    this.conteudoService.post(this.form.value)
    .pipe(take(1))
    .subscribe({
      next: (response) => {
        this.dialogRef.close(true);
      }
    });
  }

  voltar(): void {
    this.dialogRef.close(false);
  }
}
