import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { MatriculaComponent } from "./matricula.component";

const routes: Routes = [
  {
    path: '',
    component: MatriculaComponent
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  declarations: [],
  providers: [],
})
export class MatriculaRouting { }
