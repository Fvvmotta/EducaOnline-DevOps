import { AuthService } from '@educa-online/services';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'educa-online-menu',
  standalone: false,
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.scss']
})

export class MenuComponent {

  perfil = "";

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService
  ) {
    this.perfil = this.authService.getPerfil();
  }

  ativo(menu: string): boolean {
    if(this.router.url.indexOf('aluno') >= 0 && menu === 'aluno')
      return true;
    else if(this.router.url.indexOf('curso') >= 0 && menu === 'curso')
      return true;

    return false;
  }
}
