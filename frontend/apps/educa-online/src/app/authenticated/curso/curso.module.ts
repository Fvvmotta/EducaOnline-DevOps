import { NgModule } from "@angular/core";
import { CursoRouting } from "./curso.routing";
import { MatTableModule } from "@angular/material/table";
import { MatInputModule } from "@angular/material/input";
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from "@angular/material/form-field";
import { ReactiveFormsModule } from "@angular/forms";
import { ValidationMessagePipe } from "@educa-online/forms";
import { MatDialogModule } from "@angular/material/dialog";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { CursoComponent } from "./curso.component";
import { ListaCursoComponent } from "./lista-curso/lista-curso.component";
import { CreateEditCursoComponent } from "./create-edit-curso/create-edit-curso.component";
import { MatSelectModule } from "@angular/material/select";
import { AlunoService, ConteudoService } from "@educa-online/services";
import { ListaAulaComponent } from "./lista-aula/lista-aula.component";
import { CreateEditAulaComponent } from "./create-edit-aula/create-edit-aula.component";

@NgModule({
  imports: [CursoRouting, MatTableModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    ValidationMessagePipe,
    MatDialogModule,
    MatSnackBarModule,
    MatSelectModule,
  ],
  exports: [],
  declarations: [CursoComponent, ListaCursoComponent, CreateEditCursoComponent, ListaAulaComponent, CreateEditAulaComponent],
  providers: [ConteudoService, AlunoService],
})
export class CursoModule { }