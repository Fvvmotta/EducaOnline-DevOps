import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticatedComponent } from './authenticated.component';
import { AuthenticatedGuard } from '../guards/authenticated.guard';
import { AdmindGuard } from '../guards/admin.guard';

const routes: Routes = [
    {
      path: '',
      component: AuthenticatedComponent,
      // canActivate: [AuthenticatedGuard],
      children: [
        {
          path: '',
          pathMatch: 'full',
          redirectTo: 'inicio',
        },
        {
          path: 'aluno',
          loadChildren: () =>
            import('./usuario/usuario.module').then((x) => x.UsuarioModule),
          canActivate: [AdmindGuard],
        },
        {
          path: 'curso',
          loadChildren: () =>
            import('./curso/curso.module').then((x) => x.CursoModule),
          canActivate: [AuthenticatedGuard],
        },
        {
          path: 'matricula/:cursoId',
          loadChildren:() => 
            import('./matricula/matricula.module').then((x) => x.MatriculaModule),
          canActivate: [AuthenticatedGuard],
        }
      ],
    },
  ];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})

export class AuthenticatedRouting { }
