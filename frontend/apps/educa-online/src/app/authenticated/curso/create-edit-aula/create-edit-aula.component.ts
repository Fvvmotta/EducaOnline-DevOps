import { Component, inject, OnInit } from "@angular/core";
import { MAT_DIALOG_DATA, MatDialogRef } from "@angular/material/dialog";
import { AulaResponseModel } from "@educa-online/data";
import { AulaFormGroup, CursoFormGroup } from "@educa-online/forms";
import { ConteudoService } from "@educa-online/services";
import { take } from "rxjs";

@Component({
  selector: 'app-create-edit-aula',
  templateUrl: './create-edit-aula.component.html',
  styleUrls: ['create-edit-aula.component.scss'],
  standalone: false
})

export class CreateEditAulaComponent implements OnInit {
  form: AulaFormGroup;

  dialogRef = inject(MatDialogRef<CreateEditAulaComponent>)
  data = inject(MAT_DIALOG_DATA) as AulaResponseModel;

  constructor(
    private conteudoService: ConteudoService
  ) {
    this.form = new AulaFormGroup();
  }

  ngOnInit() {
    if(this.data.id) {
      this.form.patchValue(this.data);
    }
    this.form.cursoId.setValue(this.data.cursoId);
  }

  salvar(): void {
    const { valid, value } = this.form;
    if(valid && this.data.id) {
      this.editar();
    } else if(valid) {
      this.cadastrar();
    }
  }

  editar(){
    this.conteudoService.alterarAula(this.data.cursoId, this.data.id, this.form.value)
    .pipe(take(1))
    .subscribe({
      next: (response) => {
        this.dialogRef.close(true);
      }
    });
  }

  cadastrar(){
    this.conteudoService.adicionarAula(this.data.cursoId, this.form.value)
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
