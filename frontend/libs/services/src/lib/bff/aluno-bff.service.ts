import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../configuration';
import { BffResponseModel, MatriculaModel } from '@educa-online/data';

@Injectable()
export class AlunoBffService {

    constructor(private httpClient: HttpClient) { }


    matricular(model: MatriculaModel): Observable<BffResponseModel> {
        return this.httpClient.post<BffResponseModel>(`${environment.alunoBff}/aluno-bff/matricular`, model)
    }
    
}