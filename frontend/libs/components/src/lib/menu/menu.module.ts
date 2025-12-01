import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MenuComponent } from './menu.component';
import {MatIconModule} from '@angular/material/icon';

@NgModule({
  imports: [RouterModule, CommonModule, MatIconModule],
  declarations: [MenuComponent],
  exports: [MenuComponent],
  providers: [],
})
export class MenuModule { }
