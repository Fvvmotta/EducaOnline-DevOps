import { Component, inject, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { ModalInfoModel } from './modal-info.interface';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'lib-modal-info',
  templateUrl: 'modal-info.component.html',
  imports: [MatDialogModule, MatButtonModule, CommonModule]
})

export class ModalInfoComponent implements OnInit {

  dialogRef = inject(MatDialogRef<ModalInfoComponent>)
  data = inject(MAT_DIALOG_DATA) as ModalInfoModel;

  constructor() { }

  ngOnInit() { }

  voltar(): void {
    this.dialogRef.close(false);
  }

  confirmar(): void {
    this.dialogRef.close(true);
  }

}
