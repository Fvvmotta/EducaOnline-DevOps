import { MatInputModule } from '@angular/material/input';
import { NgModule } from '@angular/core';

import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button'
import { MatFormFieldModule } from '@angular/material/form-field';
import { ValidationMessagePipe } from '@educa-online/forms';
import { NgxMaskDirective } from 'ngx-mask';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { HomeComponent } from './home.component';
import { HomeRouting } from './home.routing';
import { AuthService, ConteudoService } from '@educa-online/services';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [
    MatInputModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    ValidationMessagePipe,
    NgxMaskDirective,
    MatSnackBarModule,
    HomeRouting,
    CommonModule
  ],
  exports: [],
  declarations: [HomeComponent],
  providers: [ConteudoService, AuthService]
})
export class HomeModule { }
