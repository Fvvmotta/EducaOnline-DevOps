import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../configuration";
import { Observable } from "rxjs";
import { AulaModel, CursoModel, CursoResponseModel } from "@educa-online/data";

@Injectable()
export class ConteudoService {

  apiBase: string;

  constructor(
    private httpCliente: HttpClient
  ) { 
    this.apiBase = `${environment.apiConteudo}/conteudos`;
  }


  get(): Observable<CursoResponseModel[]> {
    return this.httpCliente.get<CursoResponseModel[]>(`${this.apiBase}`);
  }

  getById(cursoId: string): Observable<CursoResponseModel> {
    return this.httpCliente.get<CursoResponseModel>(`${this.apiBase}/${cursoId}`);
  }

  post(curso: CursoModel): Observable<any> {
    return this.httpCliente.post<any>(`${this.apiBase}`, curso);
  }

  put(cursoId: string, curso: CursoModel): Observable<any> {
    return this.httpCliente.put<any>(`${this.apiBase}/${cursoId}`, curso);
  }

  desativarCurso(cursoId: string): Observable<any> {
    return this.httpCliente.put<any>(`${this.apiBase}/${cursoId}/desativar`, {});
  }

  adicionarAula(cursoId: string, aula: AulaModel): Observable<CursoResponseModel> {
    return this.httpCliente.post<CursoResponseModel>(`${this.apiBase}/${cursoId}/aula`, aula);
  }

  alterarAula(cursoId: string, aulaId: string, aula: AulaModel): Observable<CursoResponseModel> {
    return this.httpCliente.put<CursoResponseModel>(`${this.apiBase}/${cursoId}/aula/${aulaId}`, aula);
  }

  removerAula(cursoId: string, aulaId: string): Observable<CursoResponseModel> {
    return this.httpCliente.delete<CursoResponseModel>(`${this.apiBase}/${cursoId}/aula/${aulaId}`);
  }
}
