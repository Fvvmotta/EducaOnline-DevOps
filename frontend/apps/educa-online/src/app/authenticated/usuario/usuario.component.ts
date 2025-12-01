import { Component } from "@angular/core";
import { HeaderService } from "@educa-online/services";

@Component({
  selector: 'app-usuario',
  template: '<router-outlet></router-outlet>',
  standalone: false
})

export class UsuarioComponent {

  constructor(
    private headerService: HeaderService
  ) {
    this.headerService.alterarTitulo('Usuários');
  }
}