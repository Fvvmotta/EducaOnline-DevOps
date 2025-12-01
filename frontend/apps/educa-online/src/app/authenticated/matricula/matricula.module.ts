import { NgModule } from '@angular/core';

import { MatriculaComponent } from './matricula.component';
import { AlunoBffService, ConteudoService, PedidoBffService } from '@educa-online/services';
import { MatriculaRouting } from './matricula.routing';
import { MatFormFieldModule } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { CommonModule } from '@angular/common';

@NgModule({
    imports: [
        CommonModule,
        MatriculaRouting, MatFormFieldModule, ReactiveFormsModule, MatInputModule, MatButtonModule, MatDatepickerModule,   NgxMaskDirective, NgxMaskPipe
    ],
    exports: [],
    declarations: [MatriculaComponent],
    providers: [AlunoBffService, ConteudoService, PedidoBffService],
})
export class MatriculaModule { }
