import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { CursoComponent } from "./curso.component";
import { ListaCursoComponent } from "./lista-curso/lista-curso.component";
import { ListaAulaComponent } from "./lista-aula/lista-aula.component";

const routes: Routes = [
  {
    path: '',
    component: CursoComponent,
    children: [
      {
        path: '',
        component: ListaCursoComponent
      },
      {
        path: ':id/aula',
        component: ListaAulaComponent
      }
    ]
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  declarations: [],
  providers: [],
})
export class CursoRouting { }