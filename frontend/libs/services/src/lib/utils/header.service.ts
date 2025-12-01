import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({providedIn: 'root'})
export class HeaderService {

  private titulo = new BehaviorSubject<string>("");
  private titulo$ = this.titulo.asObservable();


  alterarTitulo(titulo: string) : void {
    this.titulo.next(titulo);
  }

  obterNovoTitulo(): Observable<string> {
    return this.titulo$;
  }
}
