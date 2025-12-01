import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CursoResponseModel } from '@educa-online/data';
import { AuthService, ConteudoService } from '@educa-online/services';
import { take } from 'rxjs';

@Component({
    selector: 'app-home',
    templateUrl: 'home.component.html',
    styleUrls: ['./home.component.scss'],
    standalone: false
})

export class HomeComponent implements OnInit {
    cursos: CursoResponseModel[] = [];
    usuario: any;
    role: string;

    constructor(private conteudoService: ConteudoService, private authService: AuthService, private router: Router) {
        this.usuario = this.authService.decodeToken();
        this.role = this.usuario?.role;
    }

    ngOnInit() {

        this.conteudoService.get()
            .pipe(take(1))
            .subscribe(data => this.cursos = data)
    }

    redirectToLogin(matricular: boolean = false, cursoId: string = ''): void {
        if (!this.usuario && matricular)
            this.router.navigate([`/login`], { queryParams: { cursoId } });
        else if(!this.usuario && !matricular)
            this.router.navigate([`/login`]);
        else
            this.router.navigate(['/matricula', cursoId]);
    }

    redirectToPanel(): void {
        this.router.navigate(['/curso']);
    }
}