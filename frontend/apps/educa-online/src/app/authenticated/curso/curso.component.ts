import { Component } from "@angular/core";
import { HeaderService } from "@educa-online/services";

@Component({
  selector: 'app-curso',
  template: '<router-outlet></router-outlet>',
  standalone: false
})

export class CursoComponent {

  constructor(
    private headerService: HeaderService
  ) {
    this.headerService.alterarTitulo('Cursos');
  }
}