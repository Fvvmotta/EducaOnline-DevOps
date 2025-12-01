import { NgModule } from "@angular/core";
import { ReactiveFormsModule } from "@angular/forms";
import { MatButtonModule } from "@angular/material/button";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSnackBarModule } from "@angular/material/snack-bar";
import { MatTableModule } from '@angular/material/table';
import { ValidationMessagePipe } from "@educa-online/forms";
import { MatDialogModule } from '@angular/material/dialog';
import { UsuarioRouting } from "./usuario.routing";
import { UsuarioComponent } from "./usuario.component";
import { ListaUsuarioComponent } from "./lista-usuario/lista-usuario.component";
import { AlunoService } from "@educa-online/services";

@NgModule({
  imports: [UsuarioRouting, MatTableModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    ReactiveFormsModule,
    ValidationMessagePipe,
    MatDialogModule,
    MatSnackBarModule
  ],
  exports: [],
  declarations: [UsuarioComponent, ListaUsuarioComponent],
  providers: [AlunoService],
})
export class UsuarioModule { }