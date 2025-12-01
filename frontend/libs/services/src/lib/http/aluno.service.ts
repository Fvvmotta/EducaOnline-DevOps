import { Injectable } from "@angular/core";
import { environment } from "../configuration";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { AlunoMatriculaResponseModel, AlunoResponseModel, MatriculaResponseModel } from "@educa-online/data";

@Injectable()
export class AlunoService {

  apiBase: string;

  constructor(
    private httpCliente: HttpClient
  ) { 
    this.apiBase = `${environment.apiAluno}/Alunos`;
  }

  getbyId(id: string): Observable<AlunoMatriculaResponseModel> {
    return this.httpCliente.get<AlunoMatriculaResponseModel>(`${this.apiBase}/${id}`);
  }

  getAll(): Observable<AlunoResponseModel[]> {
    return this.httpCliente.get<AlunoResponseModel[]>(`${this.apiBase}`);
  }
}
